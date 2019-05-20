namespace Models.CardItem
{
    public class CardCreationInfo
    {
        /// <summary>
        /// Вопрос/задача карты
        /// </summary>
        public CardContent Question { get; set; }

        /// <summary>
        /// Ответ на вопрос карты
        /// </summary>
        public CardContent Answer { get; set; }
    }
}