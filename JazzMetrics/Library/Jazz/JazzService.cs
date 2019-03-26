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
                            if(names.Count > 0)
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
            int count = results.Count, accepted = 0;
            foreach (XmlNode result in results)
            {
                XmlNode reference = SelectSingleNodeSpecial(result, "REFERENCE_ID1"), name = SelectSingleNodeSpecial(result, "NAME2");
                if (reference != null && name != null)
                {
                    accepted = !string.IsNullOrEmpty(reference.InnerText) || !string.IsNullOrEmpty(name.InnerText) ? accepted + 1 : accepted;
                }
            }

            Console.WriteLine("{0} / {1} = {2}", accepted, count, accepted / (double)count);
        }

        private void ParseXmlForNumberMetric(XmlNodeList results)
        {
            List<string> columns = new List<string> { "Reviewed", "Under construction", "" };
            //List<string> columns = new List<string> { "Implemented", "Ready to review", "Tested" };

            if (results.Count == 1)
            {
                if (columns.Count > 1)
                {
                    //log
                }

                XmlNode value = SelectSingleNodeSpecial(results[0], "REFERENCE_ID");
                if (ParseNodeValue(value, out int numberValue))
                {
                    Console.WriteLine("{0} {1}", "XXX", numberValue); //TODO ulozeni
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
                    XmlNode name = SelectSingleNodeSpecial(result, "LITERAL_NAME"); //TODO doplnit ostatnim sloupcum, ktere tam nebudou 0 a zapsat do logu
                    if (name != null && columns.Contains(name.InnerText))
                    {
                        XmlNode value = SelectSingleNodeSpecial(result, "LITERAL_NAME1");
                        if (ParseNodeValue(value, out int numberValue))
                        {
                            Console.WriteLine("{0} {1}", name.InnerText, numberValue); //TODO ulozeni
                        }
                        else
                        {
                            //log
                        }
                    }
                }
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
