using Library.Models.Users;
using System;

namespace Library.Models.ProjectUsers
{
    /// <summary>
    /// model predstavuje prirazenu uzivatele k projektu
    /// </summary>
    public class ProjectUserModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ID projektu
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// ID uzivatele
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// datum a cas, kdy byl uzivatel pridan k projektu
        /// </summary>
        public DateTime JoinDate { get; set; }
        /// <summary>
        /// uzivatel prirazeny k projektu
        /// </summary>
        public UserModel User { get; set; }
    }
}
