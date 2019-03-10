﻿using System.Collections.Generic;

namespace WebApp.Models
{
    /// <summary>
    /// zakladni model, ktery obsahuje pouze informace o tom, jak se pozadavek na API provedl
    /// </summary>
    public class BaseApiResult
    {
        /// <summary>
        /// zprava s chybou - v pripade Success==true je prazdna
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// true - zpracovani pozadavku dopadlo spravne, false - pri zpracovani nastala chyba
        /// </summary>
        public string Message { get; set; }
    }

    public class BaseApiResultGet<T> : BaseApiResult
    {
        public List<T> Values { get; set; }
    }

    public class BaseApiResultPost : BaseApiResult
    {
        public int Id { get; set; }
    }
}
