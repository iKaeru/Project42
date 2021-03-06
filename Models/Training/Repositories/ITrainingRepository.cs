﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Models.Training.Repositories
{
    public interface ITrainingRepository
    {
        Task<Training> AddAsync(Training training);
        Task<IEnumerable<Training>> GetCardTrainingsAsync(Guid id);
        Task<Training> GetCardTrainingByTrainIdAsync(Guid trainingId);
        List<Training> GetDateTrainingCards(DateTime date);
        List<Training> GetTrainingsFromBox(MemorizationBoxes box);
        Task<IEnumerable<Guid>> GetCardsIdFromBoxAsync(MemorizationBoxes box, Guid uId);
        Task<bool> DeleteTrainAsync(Guid id);
        Training getLastTraining(Guid userId);
    }
}

