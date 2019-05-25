using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Models.Training
{
    /// <summary>
    /// Тренировка пользователя
    /// </summary>
    [Table("Training")]
    [DataContract]
    public class Training
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        [Key]
        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }
        
        /// <summary>
        /// К какому пользователю относится
        /// </summary>
        [Column("User", Order = 1)]
        [DataMember(IsRequired = true)]
        public Guid UserId { get; set; }

        /// <summary>
        /// К какой карте относится
        /// </summary>
        [Column("Card", Order = 2)]
        [DataMember(IsRequired = true)]
        public Guid CardId { get; set; }

        /// <summary>
        /// Дата решения задачи с карты
        /// </summary>
        [Column("CompletionDate", Order = 3)]
        [DataMember(IsRequired = true)]
        public DateTime CompletedAt { get; set; }

        /// <summary>
        /// Уровень сложности запоминания карты
        /// </summary>
        [Column("MemorizationLevel", Order = 4)]
        [DataMember(IsRequired = true)]
        public MemorizationLevels Level { get; set; }
    }
}