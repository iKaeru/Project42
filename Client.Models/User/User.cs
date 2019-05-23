using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Client.Models.User
{
    /// <summary>
    /// Пользователь
    /// </summary>
    [Table("Users")]
    public class User
    {
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

        /// <summary>
        /// Логин пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Login { get; set; }

        /// <summary>
        /// Почтовый адрес пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string EmailAdress { get; set; }
    }
}