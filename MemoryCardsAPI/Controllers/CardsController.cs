using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CardItem;
using Models.CardItem.Services;
using Models.Errors;
using Models.Training;
using Models.Training.Services;

namespace MemoryCardsAPI.Controllers
{
    /// <summary>
    /// Cards
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class CardsController : Controller
    {
        private readonly ICardService cardsService;
        private readonly ITrainingService trainingService;

        public CardsController(ICardService cardsService, ITrainingService trainingService)
        {
            this.cardsService = cardsService;
            this.trainingService = trainingService;
        }

        /// <summary>
        /// Post Card
        /// </summary>
        /// <param name="cardCreationInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        /// <returns code="404"></returns>
        [HttpPost]
        [Route("")]
        public ActionResult<CardItem> CreateAsync([FromBody] CardCreationInfo cardCreationInfo,
            CancellationToken cancellationToken)
        {
            Guid.TryParse(HttpContext.User.Identity.Name, out var uId);

            var card = cardsService.CreateCard(cardCreationInfo, uId);
            if (cardsService.AddCardAsync(card, cancellationToken).Result)
            {
                return Ok(card);
            }

            return BadRequest(card);
        }

        /// <summary>
        /// Post Card
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [Route("userAllCards")]
        public IActionResult GetCardsForUser(CancellationToken cancellationToken)
        {
            Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
            var result = cardsService.GetAllUserCards(uId, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Get Card By Id
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCardByIdAsync(string id, CancellationToken cancellationToken)
        {
            Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
            Guid.TryParse(id, out var cardGuid);
            var result = await cardsService.GetCardByIdAsync(cardGuid, cancellationToken);
            cardsService.CheckOwnership(result, uId);
            return Ok(result);
        }

        /// <summary>
        /// Get Card Training
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [Route("{id}/training")]
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
        /// Get Card Training
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [Route("{id}/training/create")]
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
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [Route("{id}/training/train")]
        public async Task<IActionResult> TrainWithCard(string id, [FromQuery]MemorizationLevels level, CancellationToken cancellationToken)
        {
            Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
            Guid.TryParse(id, out var cardGuid);
            var card = await cardsService.GetCardByIdAsync(cardGuid, cancellationToken);
            cardsService.CheckOwnership(card, uId);

            var training = await trainingService.GetTrainingAsync(card, uId);
            training = trainingService.CompleteTraining(training, level);
            return Ok(training);
        }
    }
}