using System.Collections.Generic;

namespace Library.Models
{
    /// <summary>
    /// zakladni model, ktery obsahuje pouze informace o tom, jak se pozadavek provedl
    /// </summary>
    public class BaseResponseModel
    {
        /// <summary>
        /// zprava s chybou - v pripade Success==true je prazdna
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// true - zpracovani pozadavku dopadlo spravne, false - pri zpracovani nastala chyba
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// nastavuje Success na true
        /// </summary>
        public BaseResponseModel()
        {
            Success = true;
        }
    }

    /// <summary>
    /// trida zaobalujici vraceny seznam
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResponseModelGetAll<T> : BaseResponseModel
    {
        /// <summary>
        /// seznam objektu
        /// </summary>
        public List<T> Values { get; set; }
    }

    /// <summary>
    /// trida zaobalujici vraceny objekt
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResponseModelGet<T> : BaseResponseModel
    {
        /// <summary>
        /// objekt
        /// </summary>
        public T Value { get; set; }
    }

    /// <summary>
    /// trida zaobalujici ID nove vytvorene entity v DB
    /// </summary>
    public class BaseResponseModelPost : BaseResponseModel
    {
        /// <summary>
        /// ID nove vytvorene entity v DB
        /// </summary>
        public int Id { get; set; }
    }
}
