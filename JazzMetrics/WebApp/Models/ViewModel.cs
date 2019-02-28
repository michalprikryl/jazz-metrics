using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    /// <summary>
    /// bazova trida vsech modelu
    /// </summary>
    public class ViewModel
    {
        public ViewModel()
        {
            MessageList = new List<Tuple<string, bool>>();
            CanView = true;
        }

        /// <summary>
        /// kolekci, kde lze ukladat chyby, ktere chci zobrazit uzivately (vypis chyb je nutne pridat do view) - TRUE je chyba
        /// </summary>
        public List<Tuple<string, bool>> MessageList { get; set; }
        /// <summary>
        /// rozhoduje, zda zobrazit uzivateli nejakou cast view (taktez nutne pridat do view)
        /// </summary>
        public bool CanView { get; set; }
    }
}
