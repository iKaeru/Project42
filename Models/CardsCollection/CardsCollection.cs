using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.CardsCollection
{
    /// <summary>
    /// Коллекция карт
    /// </summary>
    public class CardsCollection
    {
        /// <summary>
        /// Уникальный идентификатор коллекции
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Название коллекции
        /// </summary>
        [StringLength(20)]
        public string Name { get; set; }
        
        /// <summary>
        /// Множество карт
        /// </summary>
        public ICollection<CardItem.CardItem> CardItems;
    }
}