using System;

namespace Models.CardItem
{
    /// <summary>
    /// Информация о карте
    /// </summary>
    public class CardItemInfo
    {
        /// <summary>
        /// Уникальный идентификатор карты
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Дата создания карты
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Дата последнего решения задачи карты
        /// </summary>
        public DateTime LastCompletedAt { get; set; }
    }
}