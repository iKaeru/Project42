﻿using System.Runtime.Serialization;

namespace Client.Models.CardItem
{
    /// <summary>
    /// Карта
    /// </summary>
    [DataContract]
    public class CardItem
    {
        /// <summary>
        /// Вопрос/задача карты
        /// </summary>
        [DataMember(IsRequired = true)]
        public CardContent Question { get; set; }

        /// <summary>
        /// Ответ на вопрос карты
        /// </summary>
        [DataMember(IsRequired = true)]
        public CardContent Answer { get; set; }
    }
}