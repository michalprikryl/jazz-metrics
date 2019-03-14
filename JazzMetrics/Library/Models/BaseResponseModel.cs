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

        public BaseResponseModel()
        {
            Success = true;
        }
    }

    public class BaseResponseModelGetAll<T> : BaseResponseModel
    {
        public List<T> Values { get; set; }
    }

    public class BaseResponseModelGet<T> : BaseResponseModel
    {
        public T Value { get; set; }
    }

    public class BaseResponseModelPost : BaseResponseModel
    {
        public int Id { get; set; }
    }
}
