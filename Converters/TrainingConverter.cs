using Client.Models.Training;
using View = Client.Models;
using Model = Models;

namespace Converters
{
    /// <summary>
    /// Предоставляет методы конвертирования тренировок между серверной и клиентской моделями
    /// </summary>
    public class TrainingConverter
    {
        /// <summary>
        /// Переводит уровень сложности запоминания из клиентской модели в серверную
        /// </summary>
        /// <param name="viewLevels">Тренировка в клиентской модели</param>
        /// <returns>Тренировка в серверной модели</returns>
        public static Model.Training.MemorizationLevels ConvertLevels(MemorizationLevels viewLevels)
        {
            return (Model.Training.MemorizationLevels) viewLevels;
        }
    }
}