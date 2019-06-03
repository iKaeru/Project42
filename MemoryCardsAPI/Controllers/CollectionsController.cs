using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Converters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CardsCollection;
using Models.CardsCollection.Services;
using Models.Errors;
using View = Client.Models.CardsCollection;
using System.Linq;
using Models.CardItem;
using Models.CardItem.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MemoryCardsAPI.Controllers
{
    /// <summary>
    /// Collections
    /// </summary>
    [Authorize]
    [Route("v1/api/[controller]")]
    public class CollectionsController : Controller
    {
        private readonly ICollectionService collectionService;
        private readonly ICardService cardService;
        private string defaultCollectionName = "Default";

        public CollectionsController(ICollectionService collectionService, ICardService cardService)
        {
            this.collectionService = collectionService;
            this.cardService = cardService;
        }

        /// <summary>
        /// Create Collection With Default Name
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns> 
        [HttpPost]
        [SwaggerResponse(200, Type = typeof(CardsCollection))]
        [Route("default")]
        public async Task<ActionResult<CardsCollection>> CreateDefaultAsync(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                if (await collectionService.IsNameExistAsync(defaultCollectionName, uId))
                {
                    return BadRequest(new { message = "Default collection already exists" });
                }

                var cardCollection = collectionService.CreateCollection(uId, defaultCollectionName);
                if (await collectionService.AddCollectionAsync(cardCollection))
                    return Ok(cardCollection);
                throw new AppException("Couldn't create collection");
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create Collection By Name
        /// </summary>
        /// <param name="name">Имя коллекции</param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns> 
        [HttpPost]
        [SwaggerResponse(200, Type = typeof(CardsCollection))]
        [Route("")]
        public async Task<ActionResult<CardsCollection>> CreateAsync([FromBody] string name,
            CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                if (await collectionService.IsNameExistAsync(name, uId))
                {
                    return BadRequest(new { message = $"Collection {name} already exists" });
                }

                var cardCollection = collectionService.CreateCollection(uId, name);
                if (await collectionService.AddCollectionAsync(cardCollection))
                    return Ok(cardCollection);

                throw new AppException("Couldn't add card to a collection");
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Add Card To A Collection By Id
        /// </summary>
        /// <param name="id">Идентификатор коллекции</param>
        /// <param name="cardId">Идентификатор карты для добавления в коллекцию</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/{cardId}")]
        public async Task<ActionResult> AddCardToCollection(string id, string cardId,
            CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                Guid.TryParse(cardId, out var cardGuid);
                Guid.TryParse(id, out var collectionId);

                if (!await collectionService.IsIdExistAsync(collectionId, userId))
                {
                    return BadRequest(new { message = "No collection with such id" });
                }

                if (await collectionService.AddCardToCollectionAsync(collectionId, cardGuid, userId))
                    return Ok();

                throw new AppException("Couldn't add card to a collection");
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Collection By Id
        /// </summary>
        /// <param name="id">Идентификатор коллекции</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(CardsCollection))]
        [Route("{id}")]
        public async Task<ActionResult> ShowCollection(string id, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                Guid.TryParse(id, out var collectionId);

                if (!await collectionService.IsIdExistAsync(collectionId, userId))
                {
                    return BadRequest(new { message = "No collection with id \"" + collectionId + "\"" });
                }

                var cardCollection = await collectionService.FindCollectionByIdAsync(collectionId, userId);
                return Ok(cardCollection);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Shows all existing collections For User
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(IEnumerable<CardsCollection>))]
        [Route("")]
        public async Task<ActionResult> ListCollections(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                var collections = await collectionService.GetAllCollectionsAsync(uId);
                return Ok(collections);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Shows all existing collections For User with extra info
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(IEnumerable<CardsCollection>))]
        [Route("showAll")]
        public async Task<ActionResult> ListCollectionsExtra(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                var collections = await collectionService.GetAllCollectionsAsync(uId);

                var resultWithTasks = collections.Select(c => new
                {
                    collectionName = c.Name,
                    collectionID = c.Id,
                    collectionCards = c.CardItems.Select(g => cardService.GetCardByIdAsync(g, cancellationToken))
                });

                foreach (var task in resultWithTasks)
                {
                    await Task.WhenAll(task.collectionCards);
                }

                return Ok(resultWithTasks.Select(r => new
                {
                    r.collectionName,
                    r.collectionID,
                    collectionCards = r.collectionCards.Select(c => new
                    {
                        Id = c.Result.Id,
                        Question = c.Result.Question
                    })
                }));
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Shows all existing collections count For User
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(int))]
        [Route("count")]
        public async Task<ActionResult> AmountOfCollections(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                var collections = await collectionService.GetAllCollectionsAsync(uId);
                return Ok(collections.Count());
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Shows All Learned Cards in Collection For User
        /// </summary>
        /// <param name="id">Идентификатор коллекции</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(IEnumerable<CardItem>))]
        [Route("{id}/learnedCards")]
        public async Task<ActionResult> CollectionLearnedCards(string id, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                Guid.TryParse(id, out var collectionId);

                if (!await collectionService.IsIdExistAsync(collectionId, userId))
                {
                    return BadRequest(new { message = "No collection with id \"" + collectionId + "\"" });
                }

                var collections = await collectionService.GetAllLearnedCardsAsync(userId,
                    collectionId);
                return Ok(collections);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Shows All Unlearned Cards in Collection For User
        /// </summary>
        /// <param name="id">Идентификатор коллекции</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(IEnumerable<CardItem>))]
        [Route("{id}/unlearnedCards")]
        public async Task<ActionResult> CollectionUnlearnedCards(string id, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                Guid.TryParse(id, out var collectionId);

                if (!await collectionService.IsIdExistAsync(collectionId, userId))
                {
                    return BadRequest(new { message = "No collection with id \"" + collectionId + "\"" });
                }

                var collections = await collectionService.GetAllUnlearnedCardsAsync(userId,
                    collectionId);
                return Ok(collections);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Shows number of learned collections For User
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(int))]
        [Route("learnedAmount")]
        public async Task<IActionResult> LearnedCollectionsAsync(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                var collections = await collectionService.GetLearnedCollectionsAsync(userId);
                if (collections == null) return Ok(0);
                return Ok(collections.Count());
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Shows number of learned collections For User
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(int))]
        [Route("notlearnedAmount")]
        public async Task<IActionResult> NotLearnedCollectionsAsync(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                var collections = await collectionService.GetNotLearnedCollectionsAsync(userId);
                return Ok(collections.Count());
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update Collection By Id
        /// </summary>
        /// <param name="id">Идентификатор коллекции</param>
        /// <param name="updateInfo">Информация о коллекции для редактирования</param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCardByIdAsync(string id,
            [FromBody] View.CardsCollectionPatchInfo updateInfo, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                Guid.TryParse(id, out var collectionId);

                if (!await collectionService.IsIdExistAsync(collectionId, userId))
                {
                    return BadRequest(new { message = "No collection with such id" });
                }

                var collection = CardsCollectionConverter.ConvertPatchInfo(updateInfo);
                collectionService.UpdateByIdAsync(collection, collectionId, userId);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete Collection By Id And All Cards In It
        /// </summary>
        /// <param name="id">Идентификатор коллекции</param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                Guid.TryParse(id, out var collectionId);

                var cardsList = (await collectionService.FindCollectionByIdAsync(collectionId, userId)).CardItems;
                if (await cardService.DeleteCardsFromList(cardsList))
                {
                    if (await collectionService.Delete(userId, collectionId))
                    {
                        return Ok();
                    }
                }

                throw new AppException("Couldn't delete collection");
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}