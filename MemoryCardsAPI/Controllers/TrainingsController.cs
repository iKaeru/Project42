using System;
using System.Threading;
using System.Threading.Tasks;
using Converters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CardItem.Services;
using Models.Errors;
using Models.Training;
using Models.Training.Services;
using View = Client.Models;

namespace MemoryCardsAPI.Controllers
{
    /// <summary>
    /// Cards
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class TrainingsController : Controller
    {
        private readonly ICardService cardsService;
        private readonly ITrainingService trainingService;
        
        public TrainingsController(ICardService cardsService, ITrainingService trainingService)
        {
            this.cardsService = cardsService;
            this.trainingService = trainingService;
        }

        /// <summary>
        /// Create Card Training
        /// </summary>
        /// <param name="id">Идентификатор карты, с которой происходит тренировка</param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> CreateCardTraining(string id, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                Guid.TryParse(id, out var cardGuid);
                var training = trainingService.CreateTraining(uId, cardGuid);
                await trainingService.AddToRepositoryAsync(training);
                return Ok(training);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        /// <summary>
        /// Get Card Training
        /// </summary>
        /// <param name="id">Идентификатор карты, с которой происходит тренировка</param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCardTraining(string id, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                Guid.TryParse(id, out var cardGuid);
                var card = await cardsService.GetCardByIdAsync(cardGuid, cancellationToken);
                cardsService.CheckOwnership(card, uId);

                var training = trainingService.GetTrainingAsync(card, uId);
                return Ok(training);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update Card Training
        /// </summary>
        /// <param name="training">Информация для обновления тренировки</param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpPut]
        [Route("train")]
        public async Task<IActionResult> TrainWithCard([FromBody]View.Training.TrainingPatchInfo training, 
            CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                Guid.TryParse(training.СardId, out var cardGuid);
                var card = await cardsService.GetCardByIdAsync(cardGuid, cancellationToken);
                cardsService.CheckOwnership(card, uId);
                var completeTraining = await trainingService.GetTrainingAsync(card, uId);
                completeTraining = trainingService.CompleteTraining(completeTraining, 
                    TrainingConverter.ConvertLevels(training.MemorizationLevel));
                return Ok(completeTraining);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}