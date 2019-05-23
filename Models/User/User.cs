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
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Почтовый адрес пользователя
        /// </summary>
        public string EmailAdress { get; set; }
    }
}
