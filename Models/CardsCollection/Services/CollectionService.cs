using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.CardsCollection.Repositories;
using Models.Errors;
using System.Linq;
using Models.CardItem.Repositories;
using Models.Training;
using Models.Training.Repositories;

namespace Models.CardsCollection.Services
{
    public class CollectionService : ICollectionService
    {
        private const int MinimumNameLength = 4;
        private const int MaximumNameLength = 120;
        private readonly ICollectionsRepository repository;
        private readonly ITrainingRepository trainingRepository;
        private readonly ICardsRepository cardsRepository;


        public CollectionService(ICollectionsRepository repository, ITrainingRepository trainingRepository,
            ICardsRepository cardsRepository)
        {
            this.repository = repository;
            this.trainingRepository = trainingRepository;
            this.cardsRepository = cardsRepository;
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

        public async Task<bool> AddCardToCollectionAsync(Guid collectionId, Guid cardId, Guid userId)
        {
            if (collectionId == Guid.Empty)
                throw new AppException($"Поле {nameof(collectionId)} не указано");

            if (cardId == Guid.Empty)
                throw new AppException($"Поле {nameof(cardId)} не указано");

            if (userId == Guid.Empty)
                throw new AppException($"Поле {nameof(userId)} не указано");

            var desiredCollection = await repository.FindByIdAsync(collectionId, userId);

            if (desiredCollection.CardItems.Contains(cardId))
            {
                throw new AppException("В коллекции нет даной карты");
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

        public async void UpdateByIdAsync(CardsCollectionPatchInfo collection, Guid collectionId, Guid userId)
        {
            var collectionFromRepository = await repository.FindByIdAsync(collectionId, userId);

            if (collectionFromRepository == null)
                throw new AppException("Коллекция не найдена");

            UpdateCollectionInfo(collection, collectionFromRepository);
            ValidateCollection(collectionFromRepository);

            await repository.PatchAsync(collectionFromRepository);
        }

        public async Task<bool> Delete(Guid userId, Guid collectionId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Incorrect value", nameof(userId));
            }

            if (collectionId == Guid.Empty)
            {
                throw new ArgumentException("Incorrect value", nameof(collectionId));
            }

            return await repository.DeleteCollectionAsync(userId, collectionId);
        }

        public async Task<CardsCollection> FindCollectionByNameAsync(string collectionName, Guid userId)
        {
            FieldsAreFilled(userId, collectionName);

            return await repository.FindByNameAsync(collectionName, userId);
        }

        public async Task<CardsCollection> FindCollectionByIdAsync(Guid collectionId, Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Incorrect value", nameof(userId));
            }

            if (collectionId == Guid.Empty)
            {
                throw new ArgumentException("Incorrect value", nameof(collectionId));
            }

            return await repository.FindByIdAsync(collectionId, userId);
        }

        public async Task<bool> IsNameExistAsync(string collectionName, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new AppException($"Поле {nameof(userId)} не указано");

            return await repository.FindNameAsync(collectionName, userId);
        }

        public async Task<bool> IsIdExistAsync(Guid collectionId, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new AppException($"Поле {nameof(userId)} не указано");

            if (collectionId == Guid.Empty)
                throw new AppException($"Поле {nameof(userId)} не указано");

            return await repository.FindIdAsync(collectionId, userId);
        }

        public async Task<IEnumerable<CardsCollection>> GetAllCollectionsAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new AppException($"Поле {nameof(userId)} не указано");

            return await repository.FindCollections(userId);
        }

        public async Task<IEnumerable<CardItem.CardItem>> GetAllLearnedCardsAsync(Guid collectionId, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new AppException($"Поле {nameof(userId)} не указано");

            if (collectionId == Guid.Empty)
                throw new AppException($"Поле {nameof(collectionId)} не указано");

            var cardsInCollectionIdList = (await repository.FindByIdAsync(collectionId, userId)).CardItems;
            var cardsLearned = await trainingRepository.GetCardsIdFromBoxAsync(MemorizationBoxes.FullyLearned,
                userId);
            var resultIds = cardsInCollectionIdList.Intersect(cardsLearned).ToList();
            var cardsList = await cardsRepository.GetCardsFromListAsync(resultIds);
            return cardsList;
        }

        public async Task<IEnumerable<CardItem.CardItem>> GetAllUnlearnedCardsAsync(Guid collectionId, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new AppException($"Поле {nameof(userId)} не указано");

            if (collectionId == Guid.Empty)
                throw new AppException($"Поле {nameof(collectionId)} не указано");

            var cardsInCollectionIdList = (await repository.FindByIdAsync(collectionId, userId)).CardItems;
            var cardsUnlearned = await trainingRepository.GetCardsIdFromBoxAsync(MemorizationBoxes.NotLearned,
                userId);
            var resultIds = cardsInCollectionIdList.Intersect(cardsUnlearned).ToList();
            var cardsList = await cardsRepository.GetCardsFromListAsync(resultIds);
            return cardsList;
        }

        public async Task<IEnumerable<CardsCollection>> GetNotLearnedCollectionsAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new AppException($"Поле {nameof(userId)} не указано");

            var allCollections = await repository.FindCollections(userId);

            var notLearnedList = allCollections.Where(collection =>
            {
                if (IsEmpty(collection.CardItems))
                    return false;

                return collection.CardItems.Any(card => !CardLearned(card, MemorizationBoxes.FullyLearned));
            });

            return notLearnedList;
        }

        public async Task<IEnumerable<CardsCollection>> GetLearnedCollectionsAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new AppException($"Поле {nameof(userId)} не указано");

            var allCollections = await repository.FindCollections(userId);

            return allCollections.Where(collection =>
            {
                if (IsEmpty(collection.CardItems))
                    return false;

                return collection.CardItems.All(card => CardLearned(card, MemorizationBoxes.FullyLearned));
            });
        }

        private bool CardLearned(Guid cardId, MemorizationBoxes level)
        {
            var cardTrainings = trainingRepository.GetCardTrainingsAsync(cardId).Result;

            if (cardTrainings.Count() == 0) 
                return false;

            var lastCardTrainingDate = cardTrainings.Max(t => t.CompletedAt);
            var lastСardTraining = cardTrainings.First(t => t.CompletedAt == lastCardTrainingDate);

            if (lastСardTraining == null)
                return false;

            if (lastСardTraining.Box == level)
                return true;

            return false;
        }

        private static bool IsEmpty<T>(IEnumerable<T> source)
        {
            if (source == null)
                return true;
            return !source.Any();
        }

        #region private helper methods

        private void FieldsAreFilled(Guid userId, string collectionName)
        {
            if (userId == Guid.Empty)
                throw new AppException($"Поле {nameof(userId)} не указано");

            if (CheckStringFilled(collectionName))
                throw new AppException($"Поле {nameof(collectionName)} не указано");
        }

        private void ValidateCollection(CardsCollection collectionToValidate)
        {
            if (!FieldsAreFilled(collectionToValidate))
                throw new AppException("Недостаточно информации про пользователя");
            if (CheckStringFilled(collectionToValidate.Name))
                throw new AppException(nameof(collectionToValidate.Name) + " не указано");
            if (!LengthIsCorrect(collectionToValidate.Name, MinimumNameLength, MaximumNameLength))
                throw new AppException($"Некорректная длина в \"имени\" коллекциии "+
                                       $", должна быть от {MinimumNameLength} до {MaximumNameLength}");
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