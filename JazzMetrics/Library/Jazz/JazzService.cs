using Library.Networking;
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

        public async Task CreateSnapshot(string url, string username, string password)
        {
            try
            {
                string xml = await GetDataFromJazzReportingService(url, username, password);
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

                        string type = "SYRS";
                        XmlNodeList results = null;
                        if (names.Distinct().Count() == 1)
                        {
                            results = document.SelectNodes("/ns:results/ns:result", _namespaces);
                        }
                        else
                        {
                            results = document.SelectNodes($"/ns:results/ns:result[ns:NAME='{type}']", _namespaces); //TODO typ 
                        }

                        if (results.Count > 0)
                        {
                            bool r = false;
                            if (r) //TODO rozdeleni
                            {
                                ParseXmlForNumberMetric(results);
                            }
                            else
                            {
                                ParseXmlForCoverageMetric(results);
                            }
                        }
                        else
                        {
                            //log
                        }
                    }
                    else
                    {
                        //log
                    }
                }
                else
                {
                    //log
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                //log + DB log
            }
        }

        private void ParseXmlForCoverageMetric(XmlNodeList results)
        {
            string type = "abc";

            int count = results.Count, accepted = 0;
            if(type != ALL_VALUES) 
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
            else if(results.Item(0).SelectSingleNode("//ns:LITERAL_NAME", _namespaces) != null) //M03, M06, M60
            {
                string values = "Reviewed;Under construction;";
                string[] columns = values.Split(';');
                foreach (XmlNode result in results)
                {
                    XmlNode name = SelectSingleNodeSpecial(result, COVERAGE_FIELD_VALUE);
                    if (name != null && columns.Contains(name.InnerText))
                    {
                        accepted++;
                    }
                }
            }
            else
            {
                Console.WriteLine("unknown type");
                //log - neznamy typ metriky
            }

            Console.WriteLine("{0} / {1} = {2}", accepted, count, accepted / (double)count);
        }

        private void ParseXmlForNumberMetric(XmlNodeList results)
        {
            Dictionary<string, int> columns = new Dictionary<string, int> { { "Reviewed", 0 }, { "Under construction", 0 }, { "", 0 } };
            //Dictionary<string, int> columns = new Dictionary<string, int> { { "Implemented", 0 }, { "Ready to review", 0 }, { "Tested", 0 } };

            if (results.Count == 1)
            {
                if (columns.Count > 1)
                {
                    //log
                }

                XmlNode value = SelectSingleNodeSpecial(results[0], "REFERENCE_ID");
                if (ParseNodeValue(value, out int numberValue))
                {
                    columns[columns.First().Key] = numberValue;
                }
                else
                {
                    //log
                }
            }
            else
            {
                foreach (XmlNode result in results)
                {
                    XmlNode name = SelectSingleNodeSpecial(result, NUMBER_FIELD_VALUE); //TODO doplnit ostatnim sloupcum, ktere tam nebudou 0 a zapsat do logu
                    if (name != null && columns.ContainsKey(name.InnerText)) //TODO vice hodnot na jednom sloupci
                    {
                        XmlNode value = SelectSingleNodeSpecial(result, NUMBER_FIELD_COUNT);
                        if (ParseNodeValue(value, out int numberValue))
                        {
                            columns[value.InnerText] += numberValue;
                        }
                        else
                        {
                            columns[value.InnerText]++;
                        }
                    }
                }
            }

            foreach (var item in columns)
            {
                Console.WriteLine("{0} {1}", item.Key, item.Value); //TODO ulozeni
            }
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
