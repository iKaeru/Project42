using System;
using System.ComponentModel.DataAnnotations;

namespace Models.User
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

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
        [StringLength(20)]
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Почтовый адрес пользователя
        /// </summary>
        [EmailAddress]
        public string EMailAdress { get; set; }

        /// <summary>
        /// Дата регистрации пользователя
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }
    }
}