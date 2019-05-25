﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.CardItem;

namespace Models.Training.Services
{
    public interface ITrainingService
    {
        Training CreateTraining(Guid userId, Guid cardId);
        Training UpdateTraining(Training training, MemorizationBoxes level);
        Task<Training> AddToRepositoryAsync(Training training);
        Task<Training> GetTrainingAsync(CardItem.CardItem card, Guid userId);
        Task<List<Guid>> GetDateTrainingAsync(DateTime date, Guid uId);
        Task<int> GetCardsFromBoxAsync(MemorizationBoxes box, Guid uId);
    }
}
