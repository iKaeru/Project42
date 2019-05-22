using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CardsCollection;
using Models.CardsCollection.Services;
using Models.Errors;

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
        private string defaultCollectionName = "Global";

        public CollectionsController(ICollectionService collectionService)
        {
            this.collectionService = collectionService;
        }

        /// <summary>
        /// Create Global Collection
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns> 
        [HttpGet]
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
                throw new AppException("Couldn't add card to a collection");
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Create collection with name from query
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="name"></param>
        /// <returns code="200"></returns> 
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<CardsCollection>> CreateAsync(CancellationToken cancellationToken,
            [FromQuery] string name)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
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
        /// Adds card to a target collection
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="cardId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{collectionName}/AddCard")]
        public async Task<ActionResult> AddCardToCollection(string collectionName, [FromQuery] Guid cardId,
            CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                if (!await collectionService.IsNameExistAsync(collectionName, uId))
                {
                    return BadRequest(new {message = "No collection with name \"" + collectionName + "\""});
                }

                if (await collectionService.AddCardToCollectionAsync(collectionName, cardId, uId))
                    return Ok();
                
                throw new AppException("Couldn't add card to a collection");
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }


        /// <summary>
        /// Shows target collection
        /// </summary>
        /// <param name="collectionName"></param>
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

                var cardCollection = collectionService.FindCollectionByName(collectionName, uId);
                return Ok(cardCollection);
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Shows all existing collections
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public ActionResult<List<CardsCollection>> ListCollections(CancellationToken cancellationToken)
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                return Ok(collectionService.GetCollections(uId));
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}