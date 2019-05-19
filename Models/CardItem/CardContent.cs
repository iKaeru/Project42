using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Models.CardItem
{
    /// <summary>
    /// Содержимое карты
    /// </summary>
    [Table("CardsContent")]
    public class CardContent
    {
        /// <summary>
        /// Текст на карте
        /// </summary>
        [Key, Column("Text")]
        public string Text { get; set; }
        
        /// <summary>
        /// Код на карте
        /// </summary>
        [Column("Code")]
        public string Code { get; set; }
    }
}