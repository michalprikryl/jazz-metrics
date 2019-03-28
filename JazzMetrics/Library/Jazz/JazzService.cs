using Database.DAO;
using Library.Networking;
using Library.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;

namespace Library.Jazz
{
    public class JazzService : IJazzService
    {
        public const string ANY_VALUE = "*any*";
        public const string ALL_VALUES = "*all*";

        public const string NUMBER_FIELD_VALUE = "LITERAL_NAME";
        public const string NUMBER_FIELD_COUNT = "LITERAL_NAME1";

        public const string COVERAGE_FIELD_VALUE = "LITERAL_NAME";

        private XmlNamespaceManager _namespaces;

        public async Task CreateSnapshot(ProjectMetric projectMetric)
        {
            string xml = await GetDataFromJazzReportingService(projectMetric.DataUrl, projectMetric.DataUsername, PasswordHelper.Base64Decode(projectMetric.DataPassword));
            if (!string.IsNullOrEmpty(xml))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);

                XmlNode schemaLocationAttribute = document.DocumentElement.SelectSingleNode("//@*[local-name()='schemaLocation']");
                if (schemaLocationAttribute != null)
                {
                    _namespaces = new XmlNamespaceManager(document.NameTable);
                    _namespaces.AddNamespace("ns", schemaLocationAttribute.Value.Split(null)[0]);

                    int suffix = 0;
                    List<string> names = new List<string>();
                    do
                    {
                        if (names.Count > 0)
                        {
                            suffix++;
                            names.Clear();
                        }

                        foreach (XmlNode name in document.SelectNodes($"/ns:results/ns:result/ns:NAME{(suffix > 0 ? suffix.ToString() : string.Empty)}[text()]", _namespaces))
                        {
                            names.Add(name.InnerText);
                        }
                    } while (names.Any(n => n.Length > 4));

                    XmlNodeList results = null;
                    if (names.Distinct().Count() == 1)
                    {
                        results = document.SelectNodes("/ns:results/ns:result", _namespaces);
                    }
                    else
                    {
                        results = document.SelectNodes($"/ns:results/ns:result[ns:NAME='{projectMetric.Metric.RequirementGroup}']", _namespaces);
                    }

                    if (results.Count > 0)
                    {
                        string metricTypeName = projectMetric.Metric.MetricType.Name.ToLower();
                        if (metricTypeName.Contains("number"))
                        {
                            ParseXmlForNumberMetric(projectMetric, results);
                        }
                        else if (metricTypeName.Contains("coverage"))
                        {
                            ParseXmlForCoverageMetric(projectMetric, results);
                        }
                        else
                        {
                            projectMetric.ProjectMetricLog.Add(new ProjectMetricLog($"Metric #{projectMetric.MetricId} has unknown metric type {projectMetric.Metric.MetricType.Name}!"));
                        }
                    }
                    else
                    {
                        projectMetric.ProjectMetricLog.Add(new ProjectMetricLog($"Project metric #{projectMetric.Id} data XML does not have proper format (results(1) > result(1..X))!"));
                    }
                }
                else
                {
                    projectMetric.ProjectMetricLog.Add(new ProjectMetricLog($"Project metric #{projectMetric.Id} data XML does not have proper format (XML schmema or namespace is missing)!"));
                }
            }
            else
            {
                projectMetric.ProjectMetricLog.Add(new ProjectMetricLog($"Project metric #{projectMetric.Id} data XML is empty, propably bad URL."));
            }
        }

        private void ParseXmlForCoverageMetric(ProjectMetric projectMetric, XmlNodeList results)
        {
            ProjectMetricSnapshot snapshot = new ProjectMetricSnapshot
            {
                InsertionDate = DateTime.Now,
                ProjectMetric = projectMetric,
                ProjectMetricColumnValue = new List<ProjectMetricColumnValue>()
            };

            foreach (var column in projectMetric.Metric.MetricColumn)
            {
                ProjectMetricColumnValue columnValue = new ProjectMetricColumnValue
                {
                    Value = 0,
                    MetricColumnId = column.Id,
                    ProjectMetricSnapshot = snapshot
                };

                int count = results.Count, accepted = 0;
                if (column.DivisorValue != ALL_VALUES)
                {
                    //results = document.SelectNodes("/ns:results/ns:result", _namespaces); TODO all
                }


                if (results.Item(0).SelectSingleNode("//ns:REFERENCE_ID1", _namespaces) != null && results.Item(0).SelectSingleNode("//ns:NAME2", _namespaces) != null)
                {
                    foreach (XmlNode result in results)
                    {
                        XmlNode reference = SelectSingleNodeSpecial(result, "REFERENCE_ID1"), name = SelectSingleNodeSpecial(result, "NAME2");
                        if (reference != null && name != null)
                        {
                            XmlNode additionalColumn = SelectSingleNodeSpecial(result, "NAME3");
                            if ((!string.IsNullOrEmpty(reference.InnerText) || !string.IsNullOrEmpty(name.InnerText)) && (additionalColumn == null || !string.IsNullOrEmpty(additionalColumn.InnerText)))
                            {
                                accepted++;
                            }
                        }
                    }
                }
                else if (results.Item(0).SelectSingleNode("//ns:LITERAL_NAME", _namespaces) != null) //M03, M06, M60
                {
                    string[] values = column.Value.Split(';');
                    foreach (XmlNode result in results)
                    {
                        XmlNode name = SelectSingleNodeSpecial(result, column.FieldName) ?? SelectSingleNodeSpecial(result, COVERAGE_FIELD_VALUE);
                        if (name != null && (values.Contains(name.InnerText) || values[0] == ANY_VALUE))
                        {
                            accepted++;
                        }
                    }
                }
                else
                {
                    projectMetric.ProjectMetricLog.Add(new ProjectMetricLog($"Metric #{projectMetric.MetricId} coverage column '{column.Value}' is unknown type!"));
                }

                columnValue.Value = accepted / (decimal)count;

                snapshot.ProjectMetricColumnValue.Add(columnValue);
            }

            projectMetric.ProjectMetricSnapshot.Add(snapshot);

            projectMetric.ProjectMetricLog.Add(new ProjectMetricLog($"Snapshot of project metric #{projectMetric.Id} was successfully created!"));
        }

        private void ParseXmlForNumberMetric(ProjectMetric projectMetric, XmlNodeList results)
        {
            ProjectMetricSnapshot snapshot = new ProjectMetricSnapshot
            {
                InsertionDate = DateTime.Now,
                ProjectMetric = projectMetric,
                ProjectMetricColumnValue = new List<ProjectMetricColumnValue>()
            };

            if (results.Count == 1)
            {
                if (projectMetric.Metric.MetricColumn.Count > 1)
                {
                    projectMetric.ProjectMetricLog.Add(new ProjectMetricLog($"Metric #{projectMetric.MetricId} has {projectMetric.Metric.MetricColumn.Count} columns, but in data XML was only one column!"));
                }

                MetricColumn column = projectMetric.Metric.MetricColumn.First();

                ProjectMetricColumnValue columnValue = new ProjectMetricColumnValue
                {
                    Value = 0,
                    MetricColumnId = column.Id,
                    ProjectMetricSnapshot = snapshot
                };

                XmlNode value = SelectSingleNodeSpecial(results[0], column.NumberFieldName) ?? SelectSingleNodeSpecial(results[0], "REFERENCE_ID") ?? SelectSingleNodeSpecial(results[0], column.FieldName);
                if (ParseNodeValue(value, out int numberValue))
                {
                    columnValue.Value = numberValue;
                }
                else
                {
                    projectMetric.ProjectMetricLog.Add(new ProjectMetricLog($"Column #{column.Id} of metric #{column.MetricId}, does not have proper tag with numeric value in data XML!"));
                }
            }
            else
            {
                foreach (var column in projectMetric.Metric.MetricColumn)
                {
                    ProjectMetricColumnValue columnValue = new ProjectMetricColumnValue
                    {
                        Value = 0,
                        MetricColumnId = column.Id,
                        ProjectMetricSnapshot = snapshot
                    };

                    string[] values = column.Value.Split(';');
                    foreach (XmlNode result in results)
                    {
                        XmlNode name = SelectSingleNodeSpecial(result, column.FieldName) ?? SelectSingleNodeSpecial(result, NUMBER_FIELD_VALUE);
                        if (name != null && (values.Contains(name.InnerText) || values[0] == ANY_VALUE))
                        {
                            XmlNode value = SelectSingleNodeSpecial(result, column.NumberFieldName) ?? SelectSingleNodeSpecial(result, NUMBER_FIELD_COUNT);
                            if (ParseNodeValue(value, out int numberValue))
                            {
                                columnValue.Value += numberValue;
                            }
                            else
                            {
                                columnValue.Value++;
                            }
                        }
                    }

                    snapshot.ProjectMetricColumnValue.Add(columnValue);
                }
            }

            projectMetric.ProjectMetricSnapshot.Add(snapshot);

            projectMetric.ProjectMetricLog.Add(new ProjectMetricLog($"Snapshot of project metric #{projectMetric.Id} was successfully created!", false));
        }

        private XmlNode SelectSingleNodeSpecial(XmlNode node, string xpath) => node.SelectSingleNode($"ns:{xpath}", _namespaces);

        private bool ParseNodeValue(XmlNode node, out int numberValue)
        {
            numberValue = -1;
            return node != null && int.TryParse(node.InnerText, out numberValue);
        }

        private async Task<string> GetDataFromJazzReportingService(string url, string username, string password)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true; //pro self signed SSL certifikat - upusti od kontroly certifikatu

                using (HttpClient client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", ClientApi.GetHttpBasicHeader(username, password));

                    HttpResponseMessage response = await client.GetAsync(url);

                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
