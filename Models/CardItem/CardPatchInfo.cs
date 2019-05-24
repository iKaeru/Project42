using System;
using System.Runtime.Serialization;

namespace Models.CardItem
{
    /// <summary>
    /// Информация для редактирования карты
    /// </summary>
    [DataContract]
    public class CardPatchInfo
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }
        
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