using System.Runtime.Serialization;

namespace Client.Models.Training
{
    /// <summary>
    /// Уровни сложности запоминания
    /// </summary>
    [DataContract]
    public enum MemorizationLevels
    {
        /// <summary>
        /// Не выученные карты
        /// </summary>
        NotLearned = 0,

        /// <summary>
        /// Частично выученные карты
        /// </summary>
        PartlyLearned = 1,

        /// <summary>
        /// Выученные карты
        /// </summary>
        FullyLearned = 2
    }
}