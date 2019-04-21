namespace Client.Models.User
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Информация для регистрации пользователя
    /// </summary>
    [DataContract]
    public class UserRegistrationInfo
    {
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
    }
}