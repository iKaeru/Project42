using System.Runtime.Serialization;

namespace Models.Training
{
    /// <summary>
    /// Уровни сложности запоминания
    /// </summary>
    [DataContract]
    public enum MemorizationBoxes
    {
        /// <summary>
        /// Легко
        /// </summary>
        NotLearned = 0,

        /// <summary>
        /// Нормально
        /// </summary>
        PartlyLearned = 1,

        /// <summary>
        /// Сложно
        /// </summary>
        FullyLearned = 2
    }
}