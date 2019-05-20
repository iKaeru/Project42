using System.Runtime.Serialization;

namespace Models.User
{
    /// <summary>
    /// Информация для логина пользователя
    /// </summary>
    [DataContract]
    public class UserLoginInfo
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Password { get; set; }
        
        /// <summary>
        /// Запоминать ли пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool RememberMe { get; set; }
    }
}