using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.CardItem
{
    /// <summary>
    /// Информация о карте
    /// </summary>
    [Table("CardsInfo")]
    public class CardItemInfo
    {
        /// <summary>
        /// Уникальный идентификатор карты
        /// </summary>
        [Key]
        [Column("Id", Order = 1)]
        public Guid Id { get; set; }

        /// <summary>
        /// Пользователь создавший карту
        /// </summary>
        [Column("User", Order = 2)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Дата создания карты
        /// </summary>
        [Column("CreationDate", Order = 3)]
        public DateTime CreatedAt { get; set; }
    }
}