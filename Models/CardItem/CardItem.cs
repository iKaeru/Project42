using System;

namespace Models.CardItem
{
    /// <summary>
    /// Карта
    /// </summary>
    public class CardItem
    {
        /// <summary>
        /// Уникальный идентификатор карты
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Вопрос/задача карты
        /// </summary>
        public string Question { get; set; }
        
        /// <summary>
        /// Ответ на вопрос карты
        /// </summary>
        public string Answer { get; set; }
        
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