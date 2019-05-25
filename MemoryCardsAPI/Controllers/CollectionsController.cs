using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CardsCollection;
using Models.CardsCollection.Services;
using Models.Errors;
using System.Linq;

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
        private string defaultCollectionName = "Default";

        public CollectionsController(ICollectionService collectionService)
        {
            this.collectionService = collectionService;
        }

        /// <summary>
        /// Create Collection With Default Name
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns> 
        [HttpPost]
        [Route("default")]
        public async Task<ActionResult<CardsCollection>> CreateDefaultAsync(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                if (await collectionService.IsNameExistAsync(defaultCollectionName, uId))
                {
                    return BadRequest(new {message = "Default collection already exists"});
                }

                var cardCollection = collectionService.CreateCollection(uId, defaultCollectionName);
                if (await collectionService.AddCollectionAsync(cardCollection))
                    return Ok(cardCollection);
                throw new AppException("Couldn't create collection");
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Create Collection By Name
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="name">Имя коллекции</param>
        /// <returns code="200"></returns> 
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<CardsCollection>> CreateAsync(CancellationToken cancellationToken,
            [FromQuery] string name)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                if (await collectionService.IsNameExistAsync(name, uId))
                {
                    return BadRequest(new {message = $"Collection {name} already exists"});
                }
                
                var cardCollection = collectionService.CreateCollection(uId, name);
                if (await collectionService.AddCollectionAsync(cardCollection))
                    return Ok(cardCollection);

                throw new AppException("Couldn't add card to a collection");
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Add Card To A Collection By Name
        /// </summary>
        /// <param name="collectionName">Имя коллекции</param>
        /// <param name="cardId">Идентификатор карты для добавления в коллекцию</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{collectionName}/{cardId}")]
        public async Task<ActionResult> AddCardToCollection(string collectionName, string cardId,
            CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
                Guid.TryParse(cardId, out var cardGuid);
                if (!await collectionService.IsNameExistAsync(collectionName, userId))
                {
                    return BadRequest(new {message = "No collection with name \"" + collectionName + "\""});
                }

                if (await collectionService.AddCardToCollectionAsync(collectionName, cardGuid, userId))
                    return Ok();
                
                throw new AppException("Couldn't add card to a collection");
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }


        /// <summary>
        /// Get Collection By Name
        /// </summary>
        /// <param name="collectionName">Название коллекции</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{collectionName}")]
        public async Task<ActionResult> ShowCollection(string collectionName, CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                if (!await collectionService.IsNameExistAsync(collectionName, uId))
                {
                    return BadRequest(new {message = "No collection with name \"" + collectionName + "\""});
                }

                var cardCollection = collectionService.FindCollectionByNameAsync(collectionName, uId);
                return Ok(cardCollection);
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Shows all existing collections For User
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public ActionResult ListCollections(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                var collections = collectionService.GetAllCollectionsAsync(uId);
                return Ok(collections);
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Shows all existing collections For User
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("learned")]
        public async Task<IActionResult> LearnedCollectionsAsync(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                var collections = await collectionService.GetLearnedCollectionsAsync(uId);
                return Ok(collections.Count());
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}