using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.CardsCollection.Repositories;
using Models.Errors;

namespace Models.CardsCollection.Services
{
    public class CollectionService : ICollectionService
    {
        private const int MinimumNameLength = 4;
        private const int MaximumNameLength = 120;
        private readonly ICollectionsRepository repository;

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

        public async void UpdateByIdAsync(CardsCollectionPatchInfo collection, string collectionName, Guid userId)
        {
            var collectionFromRepository = await repository.FindByNameAsync(collectionName, userId);

            if (collectionFromRepository == null)
                throw new AppException("Collection not found");
            
            UpdateCollectionInfo(collection, collectionFromRepository);
            ValidateCollection(collectionFromRepository);

            await repository.PatchAsync(collectionFromRepository);
        }
        
        public async Task<bool> Delete(Guid userId, string collectionName)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Incorrect value", nameof(userId));
            }

            return await repository.DeleteCollectionAsync(userId, collectionName);
        }
        
        public async Task<CardsCollection> FindCollectionByName(string collectionName, Guid userId)
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

        public async Task<IEnumerable<CardsCollection>> GetAllCollections(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new AppException(nameof(userId) + " is required");

            return await repository.FindCollections(userId);
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

        private void UpdateCollectionInfo(CardsCollectionPatchInfo collectionToUpdate,
            CardsCollection cardFromRepository)
        {
            cardFromRepository.Name = collectionToUpdate.Name;
        }

        #endregion
    }
}