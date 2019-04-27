namespace Client.Models.CardItem
{
    /// <summary>
    /// Карта
    /// </summary>
    public class CardItem
    {
        /// <summary>
        /// Вопрос/задача карты
        /// </summary>
        public string Question { get; set; }

        
        
        /// <summary>
        /// Ответ на вопрос карты
        /// </summary>
        public string Answer { get; set; }
    }
}