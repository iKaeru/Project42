using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        
        public async Task<Training> AddAsync(Training trainingToAdd)
        {
            var id = Guid.NewGuid();
            var training = trainingToAdd;
            training.Id = id;
            
            await context.Trainings.AddAsync(training);
            await context.SaveChangesAsync();

            return training;
        }

        public async Task<Training> GetCardTrainingAsync(Guid id)
        {
            return await context.Trainings.FirstOrDefaultAsync(x => x.CardId == id);            
        }
    }
}