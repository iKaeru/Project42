using System;
using System.Collections.Generic;
using System.Threading;
using MemoryCardsAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Models.CardItem;

namespace MemoryCardsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    public class CardsController : Controller
    {
        private PostgreContext db = new PostgreContext();

        [HttpGet]
        [Route("")]
        public void CreateAsync(CancellationToken cancellationToken)
        {
            var card = new CardItem
            {
                Question = "What is meaning of 42 number?",
                Answer = "Answer to universe question",
                CreatedAt = DateTime.Now,
            };
            db.CardItems.Add(card);
            var count = db.SaveChanges();
            Console.WriteLine("{0} records saved to database", count);
        }
    }
}