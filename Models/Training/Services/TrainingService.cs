using Models.CardItem;
using Models.Errors;
using Models.Training.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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
                Box = MemorizationBoxes.NotLearned,
                CompletedAt = DateTime.Now
                
            };

            return training;
        }

        public async Task<Training> AddToRepositoryAsync(Training training)
        {
            ValidateTraining(training);
            return await repository.AddAsync(training);
        }

        public Training UpdateTraining(Training training, MemorizationBoxes box)
        {
            training.CompletedAt = DateTime.Now;
            training.Box = box;
            return training;
        }

        public async Task<Training> GetTrainingAsync(CardItem.CardItem card, Guid userId)
        {
            var found = await repository.GetCardTrainingAsync(card.Id);
            if (found == null)
                throw new AppException("Could not find created training for this card");
            if (found.UserId != userId)
                throw new AppException("Not allowed for this user");
            return found;
        }

        /// <summary>
        /// Gets id's of cards for specified user for the date
        /// </summary>
        /// <param name="date"> Date of training </param>
        /// <param name="uId"> User who is training </param>
        /// <returns> GUIDs of cards that require training </returns>
        public async Task<List<Guid>> GetDateTrainingAsync(DateTime date, Guid uId)
        {
            var cardList = await Task.Run( () => repository.GetDateTrainingCards(date)
                .Where(u => u.UserId == uId)
                .Select(t => t.CardId)
                .ToList());
            return cardList;
        }

        public async Task<int> GetCardsFromBoxAsync(MemorizationBoxes box, Guid uId)
        {
            return await Task.Run(() => repository.GetTrainingsFromBox(box)
               .Where(u => u.UserId == uId)
               .Select(t => t.CardId)
               .Count());
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