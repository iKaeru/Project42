using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Models.User
{
    /// <summary>
    /// Пользователь
    /// </summary>
    [Table("UsersInfo")]
    [DataContract]
    public class UserInfo
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        [Key]
        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Хэш пароля пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public byte[] PasswordHash { get; set; }
        
        /// <summary>
        /// Соль пароля пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public byte[] PasswordSalt { get; set; }
        
        /// <summary>
        /// Дата регистрации пользователя
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DataMember(IsRequired = true)]
        public DateTime RegistrationDate { get; set; }
    }
}
