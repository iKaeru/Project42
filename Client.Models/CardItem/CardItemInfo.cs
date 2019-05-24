using System;
using System.Runtime.Serialization;

namespace Client.Models.CardItem
{
    /// <summary>
    /// Информация о карте
    /// </summary>
    [DataContract]
    public class CardItemInfo
    {
        /// <summary>
        /// Уникальный идентификатор карты
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Id { get; set; }
        
        /// <summary>
        /// Дата создания карты
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Дата последнего решения задачи карты
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime LastCompletedAt { get; set; }
    }
}