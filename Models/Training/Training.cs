using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Training
{
    /// <summary>
    /// Тренировка пользователя
    /// </summary>
    [Table("Training")]
    public class Training
    {
        public virtual CardItem.CardItemInfo Card { get; set; }
        public virtual User.UserInfo User { get; set; }

        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        
        /// <summary>
        /// К какому пользователю относится
        /// </summary>
        [Column("User", Order = 1)]
        public Guid UserId { get; set; }

        /// <summary>
        /// К какой карте относится
        /// </summary>
        [Column("Card", Order = 2)]
        public Guid CardId { get; set; }

        /// <summary>
        /// Даты решения задачи с карты
        /// </summary>
        [Column("CompletionDate", Order = 3)]
        public DateTime[] CompletedAt { get; set; }

        /// <summary>
        /// Уровень сложности запоминания карты
        /// </summary>
        [Column("MemorizationLevel", Order = 4)]
        public MemorizationLevels Level { get; set; }
    }
}