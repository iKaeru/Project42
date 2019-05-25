using System;
using View = Client.Models.CardItem;
using Model = Models.CardItem;

namespace Converters
{
    /// <summary>
    /// Предоставляет методы конвертирования карты между серверной и клиентской моделями
    /// </summary>
    public class CardItemConverter
    {
        /// <summary>
        /// Переводит информацию для редактирования карты из клиентской модели в серверную
        /// </summary>
        /// <param name="viewCard">Карта в клиентской модели</param>
        /// <returns>Карта в серверной модели</returns>
        public static Model.CardPatchInfo ConvertPatchInfo(View.CardPatchInfo viewCard)
        {
            if (viewCard == null)
            {
                throw new ArgumentNullException(nameof(viewCard));
            }

            var modelCard = new Model.CardPatchInfo
            {
                Id = Guid.Empty,
                Answer = ConvertCardContent(viewCard.Answer),
                Question = ConvertCardContent(viewCard.Question)
            };

            return modelCard;
        }

        private static Model.CardContent ConvertCardContent(View.CardContent viewCardContent)
        {
            if (viewCardContent == null)
            {
                throw new ArgumentNullException(nameof(viewCardContent));
            }

            var modelCardContent = new Model.CardContent()
            {
                Text = viewCardContent.Text,
                Code = viewCardContent.Code
            };

            return modelCardContent;
        }
    }
}