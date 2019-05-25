using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Models.CardItem
{
    /// <summary>
    /// Информация о карте
    /// </summary>
    [Table("CardsInfo")]
    [DataContract]
    public class CardItemInfo
    {
        /// <summary>
        /// Уникальный идентификатор карты
        /// </summary>
        [Key]
        [Column("Id", Order = 1)]
        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }

        /// <summary>
        /// Пользователь создавший карту
        /// </summary>
        [Column("User", Order = 2)]
        [DataMember(IsRequired = true)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Дата создания карты
        /// </summary>
        [Column("CreationDate", Order = 3)]
        [DataMember(IsRequired = true)]
        public DateTime CreatedAt { get; set; }
    }
}