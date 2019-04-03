using System.Collections.Generic;

namespace WebAPI.Classes.Setting
{
    /// <summary>
    /// trida zaobalujici cele nastaveni ze souboru 'settings.json'
    /// </summary>
    public class SettingModel
    {
        /// <summary>
        /// parametry - napr. URL k LDAP - a jine
        /// </summary>
        public List<Parameter> Parameters { get; set; }
    }

    /// <summary>
    /// parametr pro nastaveni aplikace
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// jedinecny identifikator nastaveni
        /// </summary>
        public string Identificator { get; set; }
        /// <summary>
        /// hodnota nastaveni
        /// </summary>
        public string Value { get; set; }
    }
}