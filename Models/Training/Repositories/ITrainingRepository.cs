using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Models.Training.Repositories
{
    public interface ITrainingRepository
    {
        Task<Training> AddAsync(Training training);
        Task<Training> GetCardTrainingAsync(Guid id);
        Task<Training> GetCardTrainingByTrainIdAsync(Guid trainingId);
        List<Training> GetDateTrainingCards(DateTime date);
        List<Training> GetTrainingsFromBox(MemorizationBoxes box);
        Task<bool> DeleteTrainAsync(Guid id);
    }
}

