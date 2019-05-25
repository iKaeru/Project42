using View = Client.Models.Training;
using Model = Models.Training;

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
        public static Model.MemorizationLevels ConvertLevels(View.MemorizationLevels viewLevels)
        {
            return (Model.MemorizationLevels) viewLevels;
        }
    }
}