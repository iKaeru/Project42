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
        /// Get Card Training By Id
        /// </summary>
        /// <param name="id">Идентификатор тренировки</param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCardTrainingById(string id, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                Guid.TryParse(id, out var trainId);

                var training = await trainingService.GetTrainingByIdAsync(trainId, uId);
                return Ok(training);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Card Training By Card Id
        /// </summary>
        /// <param name="id">Идентификатор карты, с которой происходит тренировка</param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [Route("card/{id}")]
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
        /// Update Card Training By Card Id
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
                completeTraining = trainingService.UpdateTraining(completeTraining, 
                    TrainingConverter.ConvertLevels(training.MemorizationLevel));
                return Ok(completeTraining);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete Training By Id
        /// </summary>
        /// <param name="id">Идентификатор тренировки</param>
        /// <returns code="200"></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var trainId = Guid.Parse(id);

                if (await trainingService.Delete(trainId))
                {
                    return Ok();
                }

                throw new AppException("Couldn't delete training");
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Get Cards That Require Training For Selected Day
        /// </summary>
        /// <param name="date">Дата к которой получить тренировку (обычно надо указывать ту дату, которая сегодня) </param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [Route("today")]
        public async Task<IActionResult> GetTodaysTraining(DateTime date, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                var cardList = await trainingService.GetDateTrainingAsync(date, uId);
                return Ok(cardList);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get count of cards in the box
        /// </summary>
        /// <param name="box"> box where to count cards </param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [Route("countLearnedCards")]
        public async Task<IActionResult> GetNumberOfCardsInBox(MemorizationBoxes box, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                var cardNumber = await trainingService.GetCardsFromBoxAsync(box, uId);
                return Ok(cardNumber);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
