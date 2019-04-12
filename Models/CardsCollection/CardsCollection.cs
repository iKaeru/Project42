using System;
using System.Collections.Generic;

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
        public string Name { get; set; }
        
        /// <summary>
        /// Множество карт
        /// </summary>
        public ICollection<CardItem.CardItem> CardItems;
    }
}