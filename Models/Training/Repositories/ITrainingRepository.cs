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
    }
}

