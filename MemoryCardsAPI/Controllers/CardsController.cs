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
using View = Client.Models.CardItem;

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
        private readonly ITrainingService trainingsService;

        public CardsController(ICardService cardsService, ITrainingService trainingService)
        {
            this.cardsService = cardsService;
            this.trainingsService = trainingService;
        }

        /// <summary>
        /// Create Card
        /// </summary>
        /// <param name="cardCreationInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        /// <returns code="404"></returns>
        [HttpPost]
        [SwaggerResponse(200, Type=typeof(CardItem))]
        [Route("")]
        public async Task<ActionResult<CardItem>> CreateAsync([FromBody] CardCreationInfo cardCreationInfo,
            CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);

                var card = cardsService.CreateCard(cardCreationInfo, userId);
                await cardsService.AddCardAsync(card, cancellationToken);
                return Ok(card);
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Get All Cards For User
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [SwaggerResponse(200, Type=typeof(IEnumerable<CardItem>))]
        [Route("userAllCards")]
        public async Task<IActionResult> GetCardsForUser(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                var result =  await cardsService.GetAllUserCards(userId, cancellationToken);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Get All Cards Amount For User
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpGet]
        [SwaggerResponse(200, Type=typeof(int))]
        [Route("userAllCardsAmount")]
        public async Task<IActionResult> GetCardsAmountForUser(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                var result =  await cardsService.GetAllUserCards(userId, cancellationToken);
                return Ok(result.Count());
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
        [SwaggerResponse(200, Type=typeof(CardItem))]
        [Route("{id}")]
        public async Task<IActionResult> GetCardByIdAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                Guid.TryParse(id, out var cardGuid);
                var result = await cardsService.GetCardByIdAsync(cardGuid, cancellationToken);
                cardsService.CheckOwnership(result, userId);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Shows All Learned Cards Amount For User
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(int))]
        [Route("learnedAmount")]
        public async Task<ActionResult> CollectionLearnedCards(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);

                var cardsIdFromTrainings = await trainingsService.GetCardsIdFromBoxAsync(
                    MemorizationBoxes.FullyLearned, userId);

                var resultCards = await cardsService.GetAllCardsFromList(cardsIdFromTrainings);
                
                return Ok(resultCards.Count());
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
        
        /// <summary>
        /// Shows All Unlearned Cards Amount For User
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(int))]
        [Route("unlearnedAmount")]
        public async Task<ActionResult> CollectionUnlearnedCards(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);

                var cardsIdFromTrainings = await trainingsService.GetCardsIdFromBoxAsync(
                    MemorizationBoxes.NotLearned, userId);

                var resultCards = await cardsService.GetAllCardsFromList(cardsIdFromTrainings);
                
                return Ok(resultCards.Count());
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
        
        /// <summary>
        /// Shows All Need To Repeat Cards Amount For User
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(int))]
        [Route("repeatAmount")]
        public async Task<ActionResult> CollectionRepeatCards(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);

                var notLearnedCardsId = await trainingsService.GetCardsIdFromBoxAsync(
                    MemorizationBoxes.NotLearned, userId);
                var partlyLearnedCardsId = await trainingsService.GetCardsIdFromBoxAsync(
                    MemorizationBoxes.PartlyLearned, userId);
                
                var notLearnedCardsAmount = (await cardsService.GetAllCardsFromList(notLearnedCardsId)).Count();
                var partlyLearnedCardsAmount = (await cardsService.GetAllCardsFromList(partlyLearnedCardsId)).Count();

                return Ok(partlyLearnedCardsAmount + notLearnedCardsAmount);
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
        
        /// <summary>
        /// Update Card By Id
        /// </summary>
        /// <param name="id">Идентификатор карты</param>
        /// <param name="cardToUpdate">Информация о карте для редактирования</param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCardByIdAsync(string id, [FromBody] View.CardPatchInfo cardToUpdate,
            CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                Guid.TryParse(id, out var cardId);
                var card = CardItemConverter.ConvertPatchInfo(cardToUpdate);
                card.Id = cardId;

                var result = await cardsService.GetCardByIdAsync(cardId, cancellationToken);
                cardsService.CheckOwnership(result, userId);
                
                cardsService.UpdateCardByIdAsync(card, cancellationToken);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
        
        /// <summary>
        /// Delete Card By Id
        /// </summary>
        /// <param name="id">Идентификатор карты</param>
        /// <returns code="200"></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guidId = Guid.Parse(id);

                if (await cardsService.Delete(guidId))
                {
                    return Ok();
                }

                throw new AppException("Couldn't delete user");
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}