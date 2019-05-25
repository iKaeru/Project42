using System.Runtime.Serialization;

namespace Models.CardsCollection
{
    /// <summary>
    /// Информация для редактирования коллекции карт
    /// </summary>
    [DataContract]
    public class CardsCollectionPatchInfo
    {
        /// <summary>
        /// Название коллекции
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }
}