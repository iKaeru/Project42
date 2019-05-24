using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Models.CardItem
{
    /// <summary>
    /// Карта
    /// </summary>
    [Table("Cards")]
    [DataContract]
    public class CardItem : CardItemInfo
    {        
        /// <summary>
        /// Вопрос/задача карты
        /// </summary>
        [Column("Question")]
        [DataMember(IsRequired = true)]
        public CardContent Question { get; set; }
        
        /// <summary>
        /// Ответ на вопрос карты
        /// </summary>
        [Column("Answer")]
        [DataMember(IsRequired = true)]
        public CardContent Answer { get; set; }
    }
}