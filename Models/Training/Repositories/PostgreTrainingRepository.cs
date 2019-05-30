using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Errors;

namespace Models.Training.Repositories
{
    public class PostgreTrainingRepository : ITrainingRepository
    {
        private PostgreContext context;
        
        public PostgreTrainingRepository(PostgreContext context)
        {
            this.context = context;
        }
        
        public async Task<Training> AddAsync(Training training)
        {
            await context.Trainings.AddAsync(training);
            await context.SaveChangesAsync();

            return training;
        }

        public async Task<Training> GetCardTrainingAsync(Guid id)
        {
            return await context.Trainings.FirstOrDefaultAsync(x => x.CardId == id);            
        }

        public async Task<Training> GetCardTrainingByTrainIdAsync(Guid trainingId)
        {
            return await context.Trainings.FirstOrDefaultAsync(x => x.Id == trainingId);
        }

        public List<Training> GetDateTrainingCards(DateTime date)
        {
            return context.Trainings
                .Where(x => GetNextTrainingDay(x) == date).ToList();
        }

        public List<Training> GetTrainingsFromBox(MemorizationBoxes box)
        {
            return context.Trainings
                .Where(x => x.Box == box).ToList();
        }

        public async Task<IEnumerable<Guid>> GetCardsIdFromBoxAsync(MemorizationBoxes box, Guid uId)
        {
            return await Task.Run( () => context.Trainings
                .Where(x => x.Box == box)
                .Where(u => u.UserId == uId)
                .Select(t => t.CardId));
        }

        public async Task<bool> DeleteTrainAsync(Guid id)
        {
            var training = await context.Trainings.FindAsync(id);
            if (training != null)
            {
                context.Trainings.Remove(training);
                context.SaveChanges();
                return true;
            }

            return false;
        }
        
        private DateTime GetNextTrainingDay(Training x)
        {
            var daysToAdd = GetDaysCountFromBox(x.Box);
            return x.CompletedAt.AddDays(daysToAdd);
        }

        private int GetDaysCountFromBox(MemorizationBoxes box)
        {
            switch (box)
            {
                case MemorizationBoxes.NotLearned:
                    return 1;
                case MemorizationBoxes.PartlyLearned:
                    return 3;
                case MemorizationBoxes.FullyLearned:
                    return 5;
            }

            throw new AppException("Unknown card box");
        }

        public Training getLastTraining(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}