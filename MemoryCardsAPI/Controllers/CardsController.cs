using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CardItem;
using Project42.Services;

namespace MemoryCardsAPI.Controllers
{
    /// <summary>
    /// Cards
    /// </summary>

    [Authorize]
    [Route("v1/api/[controller]")]
    public class CardsController : Controller
    {
        //private PostgreContext db = new PostgreContext();

        private ICardService _cardService;

        public CardsController(
            ICardService cardService)
        {
            _cardService = cardService;
        }

        /// <summary>
        /// Get Item
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns> 

        [HttpGet]
        [Route("")]
        public async Task CreateAsync(CancellationToken cancellationToken)
        {
            Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
            var card = new CardItem
            {
                Id = Guid.NewGuid(),
                UserId = uId,
                Question = "What is meaning of 42 number?",
                Answer = "Answer to universe question",
                CreatedAt = DateTime.Now,
            };
            //await db.CardItems.AddAsync(card);
            //await db.SaveChangesAsync();
            _cardService.AddAsync(card);
            _cardService.SaveChangesAsync();
        //    _cardService.Create(card);

            Console.WriteLine("{0} records saved to database");
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetCards(CancellationToken cancellationToken)
        {
            var cards = _cardService.GetAll();
            return Ok(cards);
        }

        [HttpGet]
        [Route("userAll")]
        public IActionResult GetCardsForUser(CancellationToken cancellationToken)
        {
            Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
            var cards = _cardService.GetAll().Where(x => x.UserId==uId || x.UserId==default(Guid));
            return Ok(cards);
        }
    }
}
