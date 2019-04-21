﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Models.CardItem
{
    /// <summary>
    /// Карта
    /// </summary>
    [Table("Cards")]
    public class CardItem : CardItemInfo
    {        
        /// <summary>
        /// Вопрос/задача карты
        /// </summary>
        [Column("Question")]
        public string Question { get; set; }
        
        /// <summary>
        /// Ответ на вопрос карты
        /// </summary>
        [Column("Answer")]
        public string Answer { get; set; }
    }
}