using System;
using System.Threading;
using System.Threading.Tasks;
using Converters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CardItem;
using Models.CardItem.Services;
using Models.Errors;
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

        public CardsController(ICardService cardsService)
        {
            this.cardsService = cardsService;
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
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                var result = cardsService.GetAllUserCards(userId, cancellationToken);
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
        /// Update Card
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
    }
}