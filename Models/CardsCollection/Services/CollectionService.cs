using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.CardsCollection.Repositories;
using Models.Errors;
using System.Linq;
using Models.Training.Repositories;

namespace Models.CardsCollection.Services
{
    public class CollectionService : ICollectionService
    {
        private const int MinimumNameLength = 4;
        private const int MaximumNameLength = 120;
        private readonly ICollectionsRepository repository;
        private readonly ITrainingRepository trainingRepository;

        public CollectionService(ICollectionsRepository repository)
        {
            this.repository = repository;
        }
        
        public CardsCollection CreateCollection(Guid userId, string collectionName)
        {
            FieldsAreFilled(userId, collectionName);

            var cardCollection = new CardsCollection
            {
                UserId = userId,
                CreationDate = DateTime.UtcNow,
                Name = collectionName,
                CardItems = new List<Guid>()                
            };

            return cardCollection;
        }

        public async Task<bool> AddCollectionAsync(CardsCollection collectionToAdd)
        {
            ValidateCollection(collectionToAdd);
            
            try
            {
                await repository.CreateAsync(collectionToAdd);
            }
            catch
            {
                return false;
            }

            return true;
        }
        
        public async Task<bool> AddCardToCollectionAsync(string collectionName, Guid cardId, Guid userId)
        {
            FieldsAreFilled(userId, collectionName);

            if (cardId == Guid.Empty)
                throw new AppException(nameof(userId) + " is required");
            
            var desiredCollection = await repository.FindByNameAsync(collectionName, userId);

            if (desiredCollection.CardItems.Contains(cardId))
            {
                throw new AppException("Collection doesn't contain this card");
            }

            desiredCollection.CardItems.Add(cardId);
            
            try
            {
                await repository.UpdateAsync(desiredCollection);
            }
            catch
            {
                return false;
            }
            
            return true;
        }

        public async Task<CardsCollection> FindCollectionByNameAsync(string collectionName, Guid userId)
        {
            FieldsAreFilled(userId, collectionName);
            
            return await repository.FindByNameAsync(collectionName, userId);
        }

        public async Task<bool> IsNameExistAsync(string collectionName, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new AppException(nameof(userId) + " is required");

            return await repository.FindNameAsync(collectionName, userId);
        }
        
        public async Task<IEnumerable<CardsCollection>> GetAllCollectionsAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new AppException(nameof(userId) + " is required");
            
            return await repository.FindCollections(userId);
        }

        public async Task<IEnumerable<CardsCollection>> GetLearnedCollectionsAsync(Guid uId)
        {
            if (uId == Guid.Empty)
                throw new AppException(nameof(uId) + " is required");

            var allCollections = await repository.FindCollections(uId);

            return allCollections.Where(collection => collection.CardItems.All(card => CardLearnedAsync(card).Result));
        }

        private async Task<bool> CardLearnedAsync(Guid card)
        {
            var cardTraining = await trainingRepository.GetCardTrainingAsync(card);
            var box = cardTraining.Box;

            if (box == Training.MemorizationBoxes.FullyLearned)
                return true;
            else
                return false;
        }


        #region private helper methods

        private void FieldsAreFilled(Guid userId, string collectionName)
        {
            if (userId == Guid.Empty)
                throw new AppException(nameof(userId) + " is required");

            if (CheckStringFilled(collectionName))
                throw new AppException(nameof(collectionName) + " is required");
        }
        
        private void ValidateCollection(CardsCollection collectionToValidate)
        {
            if (!FieldsAreFilled(collectionToValidate))
                throw new AppException("Not enough information");
            if (CheckStringFilled(collectionToValidate.Name))
                throw new AppException(nameof(collectionToValidate.Name) + " is required");
            if (!LengthIsCorrect(collectionToValidate.Name, MinimumNameLength, MaximumNameLength))
                throw new AppException("Collection name length \"" + collectionToValidate.Name + "\" is incorrect");
        }

        private bool CheckStringFilled(string input)
        {
            return string.IsNullOrWhiteSpace(input) || string.IsNullOrEmpty(input);
        }
        
        private bool LengthIsCorrect(string stringToCheck, int minValue, int maxValue)
        {
            if (stringToCheck.Length < minValue || stringToCheck.Length > maxValue)
            {
                return false;
            }

            return true;
        }
        
        private bool FieldsAreFilled(CardsCollection collectionToCheck)
        {
            var type = collectionToCheck.GetType();
            foreach (var property in type.GetProperties())
            {
                if (property.Name != "Id" && property.GetValue(collectionToCheck) == null)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}