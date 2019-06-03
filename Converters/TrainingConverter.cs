using Client.Models.Training;
using View = Client.Models;
using Model = Models;
using Models.Errors;

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
        public static Model.Training.MemorizationBoxes ConvertLevels(MemorizationLevels viewLevels)
        {
            switch (viewLevels)
            {
                case MemorizationLevels.Easy:
                    return Model.Training.MemorizationBoxes.FullyLearned;
                case MemorizationLevels.Normal:
                    return Model.Training.MemorizationBoxes.PartlyLearned;
                case MemorizationLevels.Hard:
                    return Model.Training.MemorizationBoxes.NotLearned;
            }
            throw new AppException("Don't know this memorizationLevel");
        }
    }
}
