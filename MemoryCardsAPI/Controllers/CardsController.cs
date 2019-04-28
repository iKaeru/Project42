using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MemoryCardsAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Models.CardItem;

namespace MemoryCardsAPI.Controllers
{
    /// <summary>
    /// Cards
    /// </summary>
    [Route("api/v1/[controller]")]
    public class CardsController : Controller
    {
        private PostgreContext db = new PostgreContext();

        /// <summary>
        /// Get Item
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns> 

        [HttpGet]
        [Route("")]
        public async Task CreateAsync(CancellationToken cancellationToken)
        {
            var card = new CardItem
            {
                Question = "What is meaning of 42 number?",
                Answer = "Answer to universe question",
                CreatedAt = DateTime.Now,
            };
            await db.CardItems.AddAsync(card);
            await db.SaveChangesAsync();
            Console.WriteLine("{0} records saved to database");
        }
    }
}
