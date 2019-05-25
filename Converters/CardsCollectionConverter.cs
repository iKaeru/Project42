using View = Client.Models.CardsCollection;
using Model = Models.CardsCollection;

namespace Converters
{
    /// <summary>
    /// Предоставляет методы конвертирования коллекций между серверной и клиентской моделями
    /// </summary>
    public class CardsCollectionConverter
    {
        /// <summary>
        /// Переводит информацию для редактирования коллекции из клиентской модели в серверную
        /// </summary>
        /// <param name="viewCollection">Тренировка в клиентской модели</param>
        /// <returns>Тренировка в серверной модели</returns>
        public static Model.CardsCollectionPatchInfo ConvertPatchInfo(View.CardsCollectionPatchInfo viewCollection)
        {
            var collection = new Model.CardsCollectionPatchInfo
            {
                Name = viewCollection.Name
            };

            return collection;
        }
    }
}