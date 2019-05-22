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
        public async Task<ActionResult<CardItem>> CreateAsync([FromBody] CardCreationInfo cardCreationInfo,
            CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);

                var card = cardsService.CreateCard(cardCreationInfo, uId);
                await cardsService.AddCardAsync(card, cancellationToken);
                var training = trainingService.CreateTraining(uId, card.Id);
                await trainingService.AddToRepositoryAsync(training);
                return Ok(card);
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
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
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                var result = cardsService.GetAllUserCards(uId, cancellationToken);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Get Card By Id
        /// </summary>
        /// <param name="id">Идентификатор карты</param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCardByIdAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                Guid.TryParse(id, out var cardGuid);
                var result = await cardsService.GetCardByIdAsync(cardGuid, cancellationToken);
                cardsService.CheckOwnership(result, uId);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}