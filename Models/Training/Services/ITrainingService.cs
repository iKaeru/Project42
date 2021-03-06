﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Client.Models.Training;
using Models.CardItem;

namespace Models.Training.Services
{
    public interface ITrainingService
    {
        Training CreateTraining(Guid userId, Guid cardId, MemorizationBoxes box);
        Training UpdateTraining(Training training, MemorizationBoxes box);
        Task<Training> AddToRepositoryAsync(Training training);
        Task<IEnumerable<Training>> GetTrainingsAsync(CardItem.CardItem card, Guid userId);
        Task<Training> GetTrainingByIdAsync(Guid trainId, Guid userId);
        Task<List<Guid>> GetDateTrainingAsync(DateTime date, Guid userId);
        Task<IEnumerable<Guid>> GetCardsIdFromBoxAsync(MemorizationBoxes box, Guid userId);
        Task<int> GetCardsCountFromBoxAsync(MemorizationBoxes box, Guid userId);
        Task<bool> Delete(Guid id);
        Task<Training> GetLastTrainingAsync(Guid userId);
    }
}
