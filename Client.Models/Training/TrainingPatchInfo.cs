using System.Runtime.Serialization;

namespace Client.Models.Training
{
    /// <summary>
    /// Информация для редактирования тренировки
    /// </summary>
    [DataContract]
    public class TrainingPatchInfo
    {
        /// <summary>
        /// Идентификатор карты
        /// </summary>
        [DataMember(IsRequired = true)]
        public string СardId { get; set; }

        /// <summary>
        /// Сложность запоминания
        /// </summary>
        [DataMember(IsRequired = true)]
        public MemorizationLevels MemorizationLevel { get; set; }
    }
}