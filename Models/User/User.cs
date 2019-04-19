using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.User
{
    /// <summary>
    /// Пользователь
    /// </summary>
    [Table("Users")]
    public class User : UserInfo
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [StringLength(20)]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [StringLength(20)]
        public string LastName { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        [MinLength(5), MaxLength(20)]
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// Почтовый адрес пользователя
        /// </summary>
        [EmailAddress]
        public string EMailAdress { get; set; }
    }
}