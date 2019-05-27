using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Models.CardsCollection
{
    /// <summary>
    /// Коллекция карт
    /// </summary>
    [Table("CardsCollections")]
    [DataContract]
    public class CardsCollection
    {
        // public virtual CardItem.CardItemInfo Card { get; set; }

        /// <summary>
        /// Уникальный идентификатор коллекции
        /// </summary>
        [Key]
        [Column("Id", Order = 1)]
        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }

        /// <summary>
        /// Название коллекции
        /// </summary>
        [StringLength(20)]
        [Column("Name", Order = 2)]
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Множество карт
        /// </summary>
        [Column("CardsId", Order = 3)] 
        [DataMember(IsRequired = true)]
        public ICollection<Guid> CardItems;

        /// <summary>
        /// Пользователь создавший коллекцию
        /// </summary>
        [Column("User", Order = 4)]
        [DataMember(IsRequired = true)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Дата создания коллекции
        /// </summary>
        [Column("Date", Order = 5)]
        [DataMember(IsRequired = true)]
        public DateTime CreationDate { get; set; }
    }
}