using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CardsCollection;
using MemoryCardsAPI.Services;

namespace MemoryCardsAPI.Controllers
{
    /// <summary>
    /// Cards
    /// </summary>

    [Authorize]
    [Route("v1/api/[controller]")]
    public class CollectionsController : Controller
    {
        //private PostgreContext db = new PostgreContext();

        private ICollectionService collectionService;

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
        public async Task<ActionResult<CardsCollection>> 
            CreateDefaultAsync(CancellationToken cancellationToken)
        {
            Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
            if (collectionService.NameExists("Global", uId))
            {
                return BadRequest(new { message = "Default collection already exists" });                        
            }
           
            var cardCollection = new CardsCollection
            {
                Id = Guid.NewGuid(),
                UserId = uId,
                CreationDate = DateTime.UtcNow,
                Name = "Global",
                CardItems = new List<Guid>()                
            };

            //await db.CardItems.AddAsync(card);
            //await db.SaveChangesAsync();
            await collectionService.AddAsync(cardCollection);
            await collectionService.SaveChangesAsync();
            //    _cardService.Create(card);

            Console.WriteLine("{0} records saved to database");
            return Ok(cardCollection);
        }

        /// <summary>
        /// Create collection with name from query
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns> 

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<CardsCollection>> CreateAsync
            (CancellationToken cancellationToken, [FromQuery]string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(new { message = "null or empty name" });
            }
            Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
            var cardCollection = new CardsCollection
            {
                Id = Guid.NewGuid(),
                UserId = uId,
                CreationDate = DateTime.UtcNow,
                Name = name,
                CardItems = new List<Guid>()
            };

            //await db.CardItems.AddAsync(card);
            //await db.SaveChangesAsync();
            await collectionService.AddAsync(cardCollection);
            await collectionService.SaveChangesAsync();
            //    _cardService.Create(card);

            Console.WriteLine("{0} records saved to database");
            return Ok(cardCollection);
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
        public async Task<ActionResult>
            AddCardToCollection(string collectionName, [FromQuery]Guid cardId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(collectionName))
            {
                return BadRequest(new { message = "null or empty collectionName" });
            }

            Guid.TryParse(HttpContext.User.Identity.Name, out var uId);

            if (!collectionService.NameExists(collectionName, uId))
            {
                return BadRequest(new { message = "No collection with such name" });
            }

            if (collectionService.AddCard(collectionName, cardId, uId))
            {
                Console.WriteLine("Card added to collection in database");
                return Ok();
            }
            else
            {
                return BadRequest(new { message = "Could not add card (it might already exist there)" });
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
        public async Task<ActionResult>
            ShowCollection(string collectionName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(collectionName))
            {
                return BadRequest(new { message = "null or empty collectionName" });
            }

            Guid.TryParse(HttpContext.User.Identity.Name, out var uId);

            if (!collectionService.NameExists(collectionName, uId))
            {
                return BadRequest(new { message = "No collection with such name" });
            }

            var cardCollection = collectionService.FindByName(collectionName, uId);

            return Ok(cardCollection);
        }

        /// <summary>
        /// Shows all existing collections
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<CardsCollection>>>
    ListCollections(CancellationToken cancellationToken)
        {
            Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
            return Ok(collectionService.GetCollections(uId));
        }


    }
}
