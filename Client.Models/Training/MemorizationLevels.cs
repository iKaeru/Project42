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
        /// Легко
        /// </summary>
        Easy = 0,

        /// <summary>
        /// Нормально
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Сложно
        /// </summary>
        Hard = 2
    }
}