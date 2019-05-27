using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.CardItem.Repositories;
using Models.Errors;

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

        public async Task AddCardAsync(CardItem cardToAddToRepo, CancellationToken cancellationToken)
        {
            if (!IsCardValid(cardToAddToRepo))
                throw new AppException(nameof(cardToAddToRepo) + " is incorrect");
            
            try
            {
                await repository.CreateAsync(cardToAddToRepo, cancellationToken);
            }
            catch
            {
                throw new AppException("Couldn't add card");
            }
        }

        public async Task<IEnumerable<CardItem>> GetAllUserCards(Guid uId, CancellationToken cancellationToken)
        {
            if (uId == Guid.Empty)
            {
                return new List<CardItem>();
            }

            return await repository.GetAllUserCards(uId, cancellationToken);
        }

        public async Task<CardItem> GetCardByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                throw new NullReferenceException();
            }

            return await repository.GetAsync(id, cancellationToken);
        }

        public async void UpdateCardByIdAsync(CardPatchInfo cardToUpdate, CancellationToken cancellationToken)
        {
            var cardFromRepository = await repository.GetAsync(cardToUpdate.Id, cancellationToken);

            if (cardFromRepository == null)
                throw new AppException("Card not found");
            
            UpdateCardInfo(cardToUpdate, cardFromRepository);

            if (!IsCardValid(cardFromRepository))
                throw new AppException("Bad card info");
            
            await repository.PatchAsync(cardFromRepository, cancellationToken);
        }
        
        public async void CheckOwnership(CardItem card, Guid userId)
        {
            if (card == null)
                throw new AppException("Card not found");

            if (!IsCardValid(card) || userId == Guid.Empty)
                throw new AppException("Bad info");
            var cardInRepo = await repository.GetAsync(card.Id, CancellationToken.None);
            if(cardInRepo.UserId != userId)
                throw new AppException("No access");
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Incorrect value", nameof(id));
            }

            return await repository.DeleteCardAsync(id);
        }

        public async Task<bool> DeleteCardsFromList(ICollection<Guid> cardsList)
        {
            if (cardsList == null)
            {
                throw new ArgumentException("No cards in list", nameof(cardsList));
            }
            
            return await repository.DeleteCardsFromListAsync(cardsList);
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

        private bool IsCardValid(CardItem cardToValidate)
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
        
        private void UpdateCardInfo(CardPatchInfo cardToUpdate, CardItem cardFromRepository)
        {
            cardFromRepository.Answer = cardToUpdate.Answer;
            cardFromRepository.Question = cardToUpdate.Question;
        }

        #endregion
    }
}