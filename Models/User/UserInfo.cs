using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.User
{
    /// <summary>
    /// Пользователь
    /// </summary>
    [Table("UsersInfo")]
    public class UserInfo
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Хэш пароля пользователя
        /// </summary>
        public byte[] PasswordHash { get; set; }
        
        /// <summary>
        /// Соль пароля пользователя
        /// </summary>
        public byte[] PasswordSalt { get; set; }
        
        /// <summary>
        /// Дата регистрации пользователя
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }
    }
}
