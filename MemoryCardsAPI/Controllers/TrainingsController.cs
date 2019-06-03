using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Converters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CardItem;
using Models.CardItem.Services;
using Models.Errors;
using Models.Training;
using Models.Training.Services;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerResponse(200, Type=typeof(Training))]
        [Route("{id}")]
        public async Task<IActionResult> CreateCardTraining(string id, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                Guid.TryParse(id, out var cardGuid);
                var training = trainingService.CreateTraining(uId, cardGuid, MemorizationBoxes.NotLearned);
                await trainingService.AddToRepositoryAsync(training);
                return Ok(training);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Make new training with card
        /// </summary>
        /// <param name="training">Информация для cоздания тренировки</param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpPost]
        [SwaggerResponse(200, Type = typeof(Training))]
        [Route("train")]
        public async Task<IActionResult> MakeTrainingForCard([FromBody]View.Training.TrainingPatchInfo training,
            CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                Guid.TryParse(training.СardId, out var cardGuid);
                Console.WriteLine(1);
                var card = await cardsService.GetCardByIdAsync(cardGuid, cancellationToken);
                Console.WriteLine(2);
                cardsService.CheckOwnership(card, uId);
                Console.WriteLine(3);
                var createdTraining = trainingService.CreateTraining(uId, cardGuid, 
                    TrainingConverter.ConvertLevels(training.MemorizationLevel));
                Console.WriteLine(4);
                await trainingService.AddToRepositoryAsync(createdTraining);
                Console.WriteLine(5);
                return Ok(createdTraining);
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
        [SwaggerResponse(200, Type=typeof(Training))]
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
        [SwaggerResponse(200, Type=typeof(Training))]
        [Route("card/{id}")]
        public async Task<IActionResult> GetCardTrainings(string id, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                Guid.TryParse(id, out var cardGuid);
                var card = await cardsService.GetCardByIdAsync(cardGuid, cancellationToken);
                cardsService.CheckOwnership(card, uId);

                var training = await trainingService.GetTrainingsAsync(card, uId);
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
        /// <param name="trainingId"> ID тренировке, которуб нужно обновить </param>
        /// <param name="training">Информация для обновления тренировки</param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpPut]
        [SwaggerResponse(200, Type=typeof(Training))]
        [Route("{trainingId}")]
        public async Task<IActionResult> TrainWithCard(Guid trainingId, [FromBody]View.Training.TrainingPatchInfo training, 
            CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                Guid.TryParse(training.СardId, out var cardGuid);
                var card = await cardsService.GetCardByIdAsync(cardGuid, cancellationToken);
                cardsService.CheckOwnership(card, uId);
                var completeTraining = await trainingService.GetTrainingByIdAsync(trainingId, uId);
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
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [SwaggerResponse(200, Type=typeof(List<CardItem>))]
        [Route("today")]
        public async Task<IActionResult> GetTodaysTraining(CancellationToken cancellationToken)
        {
            try
            {
                List<CardItem> cardsList = await GetListOfCardsThatRequireTraining(cancellationToken);
                return Ok(cardsList);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private async Task<List<CardItem>> GetListOfCardsThatRequireTraining(CancellationToken cancellationToken)
        {
            Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
            var cardGuids = await trainingService.GetDateTrainingAsync(DateTime.Now.Date, userId);
            var cards = await cardsService.GetAllCardsFromList(cardGuids);
            var cardsList = cards.ToList();
            var allCards = await cardsService.GetAllUserCards(userId, cancellationToken);
            foreach (var card in allCards)
            {
                try
                {
                    await trainingService.GetTrainingsAsync(card, userId);
                }
                catch (AppException ex)
                {
                    if (ex.Message == "Could not find created training for this card") cardsList.Add(card);
                    else throw ex;
                }
            }

            return cardsList;
        }

        /// <summary>
        /// Gets last training date for current user
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(DateTime))]
        [Route("last")]
        public async Task<IActionResult> GetLastTraining(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                Training lastTraining = await trainingService.GetLastTrainingAsync(userId);

                if (lastTraining == null || lastTraining.CompletedAt == null)
                {
                    return Ok("none");
                }

                return Ok(lastTraining.CompletedAt);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Cards That Require Training For Selected Day
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [SwaggerResponse(200, Type=typeof(int))]
        [Route("todayAmount")]
        public async Task<IActionResult> GetTodaysTrainingAmount(CancellationToken cancellationToken)
        {
            try
            {
                var cardList = await GetListOfCardsThatRequireTraining(cancellationToken);
                return Ok(cardList.Count);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        /// <summary>
        /// Get amount of cards in the box
        /// </summary>
        /// <param name="box"> box where to count cards </param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [SwaggerResponse(200, Type=typeof(int))]
        [Route("countCardsInBox")]
        public async Task<IActionResult> GetNumberOfCardsInBox(MemorizationBoxes box, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                var cardNumber = await trainingService.GetCardsCountFromBoxAsync(box, uId);
                return Ok(cardNumber);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
