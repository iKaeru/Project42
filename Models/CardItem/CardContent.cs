using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Models.CardItem
{
    /// <summary>
    /// Содержимое карты
    /// </summary>
    [Table("CardsContent")]
    [DataContract]
    public class CardContent
    {
        /// <summary>
        /// Текст на карте
        /// </summary>
        [Key, Column("Text")]
        [DataMember(IsRequired = true)]
        public string Text { get; set; }
        
        /// <summary>
        /// Код на карте
        /// </summary>
        [Column("Code")]
        [DataMember(IsRequired = true)]
        public string Code { get; set; }
    }
}