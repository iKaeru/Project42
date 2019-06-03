using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Models.Token
{
    [Table("PasswordResetToken")]
    public class PasswordResetToken
    {
        [Key]
        [DataMember, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Токен
        /// </summary>
        [DataMember(IsRequired = true)]
        public readonly string token;

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public readonly Guid userId;

        /// <summary>
        /// Время до которого токен годен
        /// </summary>
        [DataMember(IsRequired = true)]
        public readonly DateTime validUntil;

        private PasswordResetToken()
        {
            token = Guid.NewGuid().ToString() + new Random().NextDouble().ToString();
            this.validUntil = DateTime.Now.AddHours(3);
        }

        public PasswordResetToken(Guid userId) : this()
        {
            this.userId = userId;
        }
    }
}
