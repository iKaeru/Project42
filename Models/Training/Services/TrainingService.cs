using Client.Models.Training;
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

        public Training CreateTraining(Guid userId, Guid cardId, MemorizationBoxes box)
        {
            if (userId == Guid.Empty || cardId == Guid.Empty)
                throw new AppException("ID is empty");

            var training = new Training
            {
                CardId = cardId,
                UserId = userId,
                Box = box,
                CompletedAt = DateTime.Now
                
            };

            return training;
        }

        public async Task<Training> GetTrainingByIdAsync(Guid trainId, Guid userId)
        {
            var found = await repository.GetCardTrainingByTrainIdAsync(trainId);
            if (found == null)
                throw new AppException("Training not found");
            if (found.UserId != userId)
                throw new AppException("Not allowed for this user");
            return found;
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

        public async Task<IEnumerable<Training>> GetTrainingsAsync(CardItem.CardItem card, Guid userId)
        {
            var found = await repository.GetCardTrainingsAsync(card.Id);
            if (found.Count()==0)
                throw new AppException("Не получилось найти тренировку для данной карты");
            if (found.Any(t => t.UserId != userId))
                throw new AppException("Не разрешено для этого юзера");
            return found;
        }

        /// <summary>
        /// Gets id's of cards for specified user for the date
        /// </summary>
        /// <param name="date"> Date of training </param>
        /// <param name="userId"> User who is training </param>
        /// <returns> GUIDs of cards that require training </returns>
        public async Task<List<Guid>> GetDateTrainingAsync(DateTime date, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new AppException(nameof(userId) + " is required");
            
            var cardList = await Task.Run( () => repository.GetDateTrainingCards(date)
                .Where(u => u.UserId == userId)
                .Select(t => t.CardId)
                .ToList());
            return cardList;
        }

        public async Task<int> GetCardsCountFromBoxAsync(MemorizationBoxes box, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new AppException(nameof(userId) + " is required");
            
            return await Task.Run(() => repository.GetTrainingsFromBox(box)
               .Where(u => u.UserId == userId)
               .Select(t => t.CardId)
               .Count());
        }
        
        public async Task<IEnumerable<Guid>> GetCardsIdFromBoxAsync(MemorizationBoxes box, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new AppException(nameof(userId) + " is required");
            
            return await Task.Run(() => repository.GetTrainingsFromBox(box)
                .Where(u => u.UserId == userId)
                .Select(u => u.CardId));
        }
        
        

        public async Task<bool> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Incorrect value", nameof(id));
            }

            return await repository.DeleteTrainAsync(id);
        }

        public async Task<Training> GetLastTrainingAsync(Guid userId)
        {
            return await Task.Run(() => repository.getLastTraining(userId));
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
