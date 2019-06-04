using System.Runtime.Serialization;

namespace Client.Models.CardItem
{
    [DataContract]
    public class CardCreationInfo
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