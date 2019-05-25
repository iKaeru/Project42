using System.Runtime.Serialization;

namespace Client.Models.CardItem
{
    /// <summary>
    /// Содержимое карты
    /// </summary>
    [DataContract]
    public class CardContent
    {
        /// <summary>
        /// Текст на карте
        /// </summary>
        [DataMember]
        public string Text { get; set; }

        /// <summary>
        /// Код на карте
        /// </summary>
        [DataMember]
        public string Code { get; set; }
    }
}