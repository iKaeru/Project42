using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.CardsCollection
{
    /// <summary>
    /// Коллекция карт
    /// </summary>
    [Table("Collectons")]
    public class CardsCollection
    {
     // хз зачем --->   public virtual CardItem.CardItemInfo Card { get; set; }

        /// <summary>
        /// Уникальный идентификатор коллекции
        /// </summary>
        [Key]
        [Column("Id", Order = 1)]
        public Guid Id { get; set; }

        /// <summary>
        /// Название коллекции
        /// </summary>
        [StringLength(20)]
        [Column("Name", Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// Множество карт
        /// </summary>
        [Column("Id", Order = 3)] public ICollection<Guid> CardItems;

        /// <summary>
        /// Пользователь создавший коллекцию
        /// </summary>
        [Column("User", Order = 4)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Дата создания коллекции
        /// </summary>
        [Column("Date", Order = 5)]
        public DateTime CreationDate { get; set; }
    }
}