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
    /// <summary>
    /// trida pro stazeni zpracovani dat z Jazzu
    /// </summary>
    public class JazzService : IJazzService
    {
        /// <summary>
        /// kterakoli hodnota sloupce - pouziti pro hodnoty
        /// </summary>
        public const string ANY_VALUE = "*any*";
        /// <summary>
        /// kterekoli sloupce - pouzito pri vyberu entit pro coverage metriku
        /// </summary>
        public const string ALL_VALUES = "*all*";

        /// <summary>
        /// nazev xml nodu s nazvem entity
        /// </summary>
        public const string FIELD_VALUE = "LITERAL_NAME";
        /// <summary>
        /// nazev xml nodu s poctem vyskytu entity daneho nazvu 
        /// </summary>
        public const string FIELD_COUNT = "LITERAL_NAME1";

        /// <summary>
        /// node s ID pozadavku
        /// </summary>
        public const string FIELD_ID = "REFERENCE_ID";

        /// <summary>
        /// aktualne zpracovany XML dokument
        /// </summary>
        private XmlDocument _document;
        /// <summary>
        /// jmenne prostory ziskane z XML - pro pouziti pro hledani pomoci xpath
        /// </summary>
        private XmlNamespaceManager _namespaces;

        /// <summary>
        /// suffix pri hledani nazvu skupiny pozadavku
        /// </summary>
        private int _suffix;
        /// <summary>
        /// vrati suffix pro pouziti pri hledani
        /// </summary>
        public string Suffix
        {
            get
            {
                return _suffix > 0 ? _suffix.ToString() : string.Empty;
            }
        }

        public JazzService()
        {
            _suffix = 0;
        }

        /// <summary>
        /// vytvori snapshot dane projektove metriky
        /// </summary>
        /// <param name="projectMetric">projektova metrika</param>
        /// <returns></returns>
        public async Task CreateSnapshot(ProjectMetric projectMetric)
        {
            string xml = await GetDataFromJazzReportingService(projectMetric.DataUrl, projectMetric.DataUsername, PasswordHelper.Base64Decode(projectMetric.DataPassword));
            if (!string.IsNullOrEmpty(xml))
            {
                _document = new XmlDocument();
                _document.LoadXml(xml);

                XmlNode schemaLocationAttribute = _document.DocumentElement.SelectSingleNode("//@*[local-name()='schemaLocation']"); //je nutne nacist schema, jinak nejde vybirat prvky pomoci xpath
                if (schemaLocationAttribute != null)
                {
                    _namespaces = new XmlNamespaceManager(_document.NameTable);
                    _namespaces.AddNamespace("ns", schemaLocationAttribute.Value.Split(null)[0]);

                    List<string> names = new List<string>(); //vyber nazvu skupin pozadavku
                    do
                    {
                        if (names.Count > 0)
                        {
                            _suffix++; //prochazi se po indexech vyse
                            names.Clear();
                        }

                        foreach (XmlNode name in _document.SelectNodes($"/ns:results/ns:result/ns:NAME{Suffix}[text()]", _namespaces))
                        {
                            names.Add(name.InnerText);
                        }
                    } while (names.Any(n => n.Length > 4)); //dokud nejsou jen ctyrmistne (HWRS atd.)

                    XmlNodeList results = null;
                    if (names.Distinct().Count() == 1) //jen jedna skupina
                    {
                        results = _document.SelectNodes("/ns:results/ns:result", _namespaces);
                    }
                    else //vice skupin, nacte se tedy jen ta jedna specifikovana 
                    {
                        results = _document.SelectNodes($"/ns:results/ns:result[ns:NAME{Suffix}='{projectMetric.Metric.RequirementGroup}']", _namespaces);
                    }

                    if (results.Count > 0)
                    {
                        if (projectMetric.Metric.MetricType.NumberMetric)
                        {
                            ParseXmlForNumberMetric(projectMetric, results);
                        }
                        else if (projectMetric.Metric.MetricType.CoverageMetric)
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

        /// <summary>
        /// zpracuje XML pro coverage metriku
        /// </summary>
        /// <param name="projectMetric">projektova metrida</param>
        /// <param name="results">nactene elementy z XML</param>
        private void ParseXmlForCoverageMetric(ProjectMetric projectMetric, XmlNodeList results)
        {
            ProjectMetricSnapshot snapshot = new ProjectMetricSnapshot
            {
                InsertionDate = DateTime.Now,
                ProjectMetric = projectMetric,
                ProjectMetricColumnValue = new List<ProjectMetricColumnValue>()
            };

            //nekde se totiz opakuji pozadavky, pokud maji treba vice testu (napr M78), mne staci jen vedet, ze aspon jeden ma
            //takze nadbytecne nevyberu, jinak totiz zkresluje vysledek metriky
            if (results.Item(0).SelectSingleNode($"//ns:{FIELD_ID}", _namespaces) != null)
            {
                //vybiram podle referenceID, ktere ma kazdy pozadavek jedinecne
                results = _document.SelectNodes($"/ns:results/ns:result[ns:NAME{Suffix}='{projectMetric.Metric.RequirementGroup}' and ns:{FIELD_ID}[not=(.=../following-sibling::ns:result/ns:{FIELD_ID})]]", _namespaces);
            }

            foreach (var column in projectMetric.Metric.MetricColumn)
            {
                ProjectMetricColumnValue columnValue = new ProjectMetricColumnValue
                {
                    Value = 0,
                    MetricColumnId = column.Id,
                    ProjectMetricSnapshot = snapshot
                };

                int count = 0, accepted = 0;
                if (results.Item(0).SelectSingleNode($"//ns:{FIELD_COUNT}", _namespaces) == null) //pokud se nenachazi node se sumou prvku
                {
                    if (column.DivisorValue != ALL_VALUES && !string.IsNullOrEmpty(column.DivisorFieldName)) //mam specifikovane jen nektere pro delitele
                    {
                        count = _document.SelectNodes($"/ns:results/ns:result[ns:{column.DivisorFieldName}='{column.DivisorValue}']", _namespaces).Count;
                    }
                    else //vsechny nody pro delitele
                    {
                        count = results.Count;
                    }
                }
                else //existuje node s poctem prvku, tak musim secist jednotlive hodnoty
                {
                    XmlNodeList groupResults = results;
                    if (column.DivisorValue != ALL_VALUES && !string.IsNullOrEmpty(column.DivisorFieldName))
                    {
                        groupResults = _document.SelectNodes($"/ns:results/ns:result[ns:{column.DivisorFieldName}='{column.DivisorValue}']", _namespaces);  //mam specifikovane jen nektere pro delitele
                    }

                    foreach (XmlNode result in groupResults) //scitam pocty
                    {
                        XmlNode value = SelectSingleNodeSpecial(result, FIELD_COUNT);
                        if (ParseNodeValue(value, out int numberValue))
                        {
                            count += numberValue;
                        }
                        else //pro pripad, ze tam neni cislo
                        {
                            count++;
                        }
                    }
                }


                if (results.Item(0).SelectSingleNode("//ns:REFERENCE_ID1", _namespaces) != null && results.Item(0).SelectSingleNode("//ns:NAME2", _namespaces) != null)
                { //metriky, kde jen hledam, zda maji vyplneny nektery pozadovany atribut
                    foreach (XmlNode result in results)
                    {
                        XmlNode reference = SelectSingleNodeSpecial(result, column.FieldName) ?? SelectSingleNodeSpecial(result, "REFERENCE_ID1")
                            ?? SelectSingleNodeSpecial(result, "NAME2") ?? SelectSingleNodeSpecial(result, "NAME3");
                        if (reference != null && !string.IsNullOrEmpty(reference.InnerText))
                        {
                            accepted++;
                        }
                    }
                }
                else if (results.Item(0).SelectSingleNode($"//ns:{column.FieldName}", _namespaces) != null || results.Item(0).SelectSingleNode($"//ns:{FIELD_VALUE}", _namespaces) != null) //M03, M06, M60
                { //metriky, kde hledam, jestli ma vyplnenou danou hodnotu
                    string[] values = column.Value.Split(';').Select(v => v.Trim().ToLower()).ToArray();
                    foreach (XmlNode result in results)
                    {
                        XmlNode name = SelectSingleNodeSpecial(result, column.FieldName) ?? SelectSingleNodeSpecial(result, FIELD_VALUE);
                        if (name != null && (values.Contains(name.InnerText.ToLower()) || values[0] == ANY_VALUE))
                        {
                            XmlNode value = SelectSingleNodeSpecial(result, FIELD_COUNT);
                            if (ParseNodeValue(value, out int numberValue)) //znovu, pokud je suma, pricitam sumu, jinak jen jeden
                            {
                                accepted += numberValue;
                            }
                            else
                            {
                                accepted++;
                            }
                        }
                    }
                }
                else
                {
                    projectMetric.ProjectMetricLog.Add(new ProjectMetricLog($"Metric #{projectMetric.MetricId} coverage column '{column.Value}' is unknown type!"));
                }

                columnValue.Value = (accepted / (decimal)count) * 100; //v %

                snapshot.ProjectMetricColumnValue.Add(columnValue);
            }

            projectMetric.ProjectMetricSnapshot.Add(snapshot);

            projectMetric.ProjectMetricLog.Add(new ProjectMetricLog($"Snapshot of project metric #{projectMetric.Id} was successfully created!"));
        }

        /// <summary>
        /// zpracuje XML pro number metriku
        /// </summary>
        /// <param name="projectMetric">projektova metrida</param>
        /// <param name="results">nactene elementy z XML</param>
        private void ParseXmlForNumberMetric(ProjectMetric projectMetric, XmlNodeList results)
        {
            ProjectMetricSnapshot snapshot = new ProjectMetricSnapshot
            {
                InsertionDate = DateTime.Now,
                ProjectMetric = projectMetric,
                ProjectMetricColumnValue = new List<ProjectMetricColumnValue>()
            };

            if (results.Count == 1) //je jen jeden sloupec -> to znamena, ze v nem suma a neni treba nic pocitat
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

                XmlNode value = SelectSingleNodeSpecial(results[0], column.NumberFieldName) ?? SelectSingleNodeSpecial(results[0], FIELD_ID) ?? SelectSingleNodeSpecial(results[0], column.FieldName);
                if (ParseNodeValue(value, out int numberValue))
                {
                    columnValue.Value = numberValue;
                }
                else
                {
                    projectMetric.ProjectMetricLog.Add(new ProjectMetricLog($"Column #{column.Id} of metric #{column.MetricId}, does not have proper tag with numeric value in data XML!"));
                }

                snapshot.ProjectMetricColumnValue.Add(columnValue);
            }
            else //vice sloupcu
            {
                foreach (var column in projectMetric.Metric.MetricColumn)
                {
                    ProjectMetricColumnValue columnValue = new ProjectMetricColumnValue
                    {
                        Value = 0,
                        MetricColumnId = column.Id,
                        ProjectMetricSnapshot = snapshot
                    };

                    string[] values = column.Value.Split(';').Select(v => v.Trim().ToLower()).ToArray();
                    foreach (XmlNode result in results) //hledam hodnoty a jejich pocty, pripadne jen pricitam
                    {
                        XmlNode name = SelectSingleNodeSpecial(result, column.FieldName) ?? SelectSingleNodeSpecial(result, FIELD_VALUE);
                        if (name != null && (values.Contains(name.InnerText.ToLower()) || (values[0] == ANY_VALUE && name.InnerText != string.Empty)))
                        {
                            XmlNode value = SelectSingleNodeSpecial(result, column.NumberFieldName) ?? SelectSingleNodeSpecial(result, FIELD_COUNT);
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

        /// <summary>
        /// nacte jeden XML node a prida namespace - zkratka
        /// </summary>
        /// <param name="node">xml node ke zpracovani</param>
        /// <param name="xpath">xpath cesta</param>
        /// <returns></returns>
        private XmlNode SelectSingleNodeSpecial(XmlNode node, string xpath) => node.SelectSingleNode($"ns:{xpath}", _namespaces);

        /// <summary>
        /// nacte ciselnou hodnotu z daneho xml nodu
        /// </summary>
        /// <param name="node">xml node ke zpracovani</param>
        /// <param name="numberValue">ciselna hodnota</param>
        /// <returns></returns>
        private bool ParseNodeValue(XmlNode node, out int numberValue)
        {
            numberValue = -1;
            return node != null && int.TryParse(node.InnerText, out numberValue);
        }

        /// <summary>
        /// nacteni dat z instance Jazz
        /// </summary>
        /// <param name="url">URL s XML daty</param>
        /// <param name="username">prihlasovaci jmeno pro HTTP basic auth k Jazz</param>
        /// <param name="password">heslo pro HTTP basic auth k Jazz</param>
        /// <returns></returns>
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
