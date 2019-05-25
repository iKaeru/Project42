using System;
using System.Runtime.Serialization;

namespace Models.User
{
    /// <summary>
    /// Информация для редактирования пользователя
    /// </summary>
    [DataContract]
    public class UserPatchInfo
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Логин пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Login { get; set; }

        /// <summary>
        /// Почта пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string EmailAdress { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Password { get; set; }
        
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string LastName { get; set; }
    }
}