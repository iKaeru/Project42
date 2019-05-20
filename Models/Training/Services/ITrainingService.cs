using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.CardItem;

namespace Models.Training.Services
{
    public interface ITrainingService
    {
        Training CreateTraining(Guid userId, Guid cardId);
        Training CompleteTraining(Training training, MemorizationLevels level);
        Task<Training> AddToRepositoryAsync(Training training);
        Task<Training> GetTrainingAsync(CardItem.CardItem card, Guid userId);
    }
}
