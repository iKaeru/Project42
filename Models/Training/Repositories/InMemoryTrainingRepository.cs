using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models.Data;
using Models.Errors;

namespace Models.Training.Repositories
{
    public class InMemoryTrainingRepository : ITrainingRepository
    {
        private InMemoryContext context;
        public InMemoryTrainingRepository(InMemoryContext context)
        {
            this.context = context;
        }
        
        public async Task<Training> AddAsync(Training training)
        {
            await context.Trainings.AddAsync(training);
            await context.SaveChangesAsync();

            return training;
        }

        public async Task<Training> GetCardTrainingAsync(Guid id, Guid userId)
        {
            //     var found = await context.Trainings.FindAsync(id);
            var found = context.Trainings.FirstOrDefault(x => x.CardId == id);
            if (found.UserId != userId) throw new AppException("You are not user of this training");
            return found;
        }
    }
}