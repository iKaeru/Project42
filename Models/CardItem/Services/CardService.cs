using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.CardItem.Repositories;

namespace Models.CardItem.Services
{
    public class CardService : ICardService
    {
        private readonly ICardsRepository repository;
        private const int MinimumTextLength = 4;
        private const int MaximumTextLength = 500;

        public CardService(ICardsRepository repository)
        {
            this.repository = repository;
        }

        public CardItem CreateCard(CardCreationInfo cardToCreate, Guid uId)
        {
            if (!ValidateCard(cardToCreate))
                return null;

            var card = new CardItem
            {
                Question = cardToCreate.Question,
                Answer = cardToCreate.Answer,
                CreatedAt = DateTime.Now,
                UserId = uId
            };

            return card;
        }

        public async Task<bool> AddCardAsync(CardItem cardToAddToRepo, CancellationToken cancellationToken)
        {
            if (!ValidateCard(cardToAddToRepo))
                return false;

            try
            {
                await repository.CreateAsync(cardToAddToRepo, cancellationToken);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<CardItem>> GetAllUserCards(Guid uId, CancellationToken cancellationToken)
        {
            if (uId == Guid.Empty)
            {
                return new List<CardItem>();
            }

            return await repository.GetAllUserCards(uId, cancellationToken);
        }

        #region private helper methods

        private bool ValidateCard(CardCreationInfo cardToValidate)
        {
            if (!FieldsAreFilled(cardToValidate.Question))
                return false;
            if (!FieldsAreFilled(cardToValidate.Answer))
                return false;
            if (!LengthIsCorrect(cardToValidate.Question.Text))
                return false;
            if (!LengthIsCorrect(cardToValidate.Question.Code))
                return false;
            if (!LengthIsCorrect(cardToValidate.Answer.Text))
                return false;
            if (!LengthIsCorrect(cardToValidate.Answer.Code))
                return false;

            return true;
        }

        private bool ValidateCard(CardItem cardToValidate)
        {
            if (cardToValidate.UserId == Guid.Empty)
            {
                return false;
            }

            var card = new CardCreationInfo
            {
                Question = cardToValidate.Question,
                Answer = cardToValidate.Answer
            };

            return ValidateCard(card);
        }

        private bool FieldsAreFilled(CardContent cardToCheck)
        {
            var type = cardToCheck.GetType();
            var counter = 0;
            foreach (var property in type.GetProperties())
            {
                if (property.GetValue(cardToCheck) == null)
                {
                    counter++;
                }
            }

            if (counter == type.GetProperties().Length)
            {
                return false;
            }

            return true;
        }

        private bool LengthIsCorrect(string stringToCheck)
        {
            if (stringToCheck == null)
            {
                return true;
            }

            if (stringToCheck.Length < MinimumTextLength || stringToCheck.Length > MaximumTextLength)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}