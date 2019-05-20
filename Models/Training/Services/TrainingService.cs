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
        private const int MinimumTextLength = 4;
        private const int MaximumTextLength = 500;

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
            ValidateTraining(training);         //TODO
            return await repository.AddAsync(training);
        }


        private void ValidateTraining(Training training)
        {
            if (training == null) throw new NullReferenceException();
        }

        public Training CompleteTraining(Training training, MemorizationLevels level)
        {
            training.CompletedAt = DateTime.Now;
            training.Level = level;
            return training;
        }

        public async Task<Training> GetTrainingAsync(CardItem.CardItem card, Guid userId)
        {
            return await repository.GetCardTrainingAsync(card.Id, userId);
        }
    }
}
