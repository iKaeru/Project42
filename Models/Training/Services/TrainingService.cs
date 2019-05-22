using Models.CardItem;
using Models.Errors;
using Models.Training.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Models.Training.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository repository;

        public TrainingService(ITrainingRepository repository)
        {
            this.repository = repository;
        }

        public Training CreateTraining(Guid userId, Guid cardId)
        {
            if (userId == Guid.Empty || cardId == Guid.Empty)
                throw new AppException("ID is empty");

            var training = new Training
            {
                CardId = cardId,
                UserId = userId,
                Level = MemorizationLevels.Hard
            };

            return training;
        }

        public async Task<Training> AddToRepositoryAsync(Training training)
        {
            ValidateTraining(training);
            return await repository.AddAsync(training);
        }

        public Training CompleteTraining(Training training, MemorizationLevels level)
        {
            training.CompletedAt = DateTime.Now;
            training.Level = level;
            return training;
        }

        public async Task<Training> GetTrainingAsync(CardItem.CardItem card, Guid userId)
        {
            var found = await repository.GetCardTrainingAsync(card.Id);
            if (found.UserId != userId)
                throw new AppException("Not allowed for this user");
            return found;
        }

        #region private helper methods

        private void ValidateTraining(Training training)
        {
            if (training.CompletedAt == null)
                throw new AppException(nameof(training) + "data is not filled");
            if (training.CardId == null)
                throw new AppException(nameof(training) + "card id is not filled");
        }

        #endregion
    }
}