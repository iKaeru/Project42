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
        private const int MinimumTextLength = 1;
        private const int MaximumTextLength = 500;
        private const int MinimumCodeLength = 1;
        private const int MaximumCodeLength = 500;
        
        public CardService(ICardsRepository repository)
        {
            this.repository = repository;
        }

        public CardItem CreateCard(CardCreationInfo cardToCreate, Guid userId)
        {
            ValidateCard(cardToCreate);

            var card = new CardItem
            {
                Question = cardToCreate.Question,
                Answer = cardToCreate.Answer,
                CreatedAt = DateTime.Now,
                UserId = userId,
            };

            return card;
        }

        public async Task AddCardAsync(CardItem cardToAddToRepo, CancellationToken cancellationToken)
        {
            if (!IsCardValid(cardToAddToRepo))
                throw new AppException(nameof(cardToAddToRepo) + " не указана информация");

            try
            {
                await repository.CreateAsync(cardToAddToRepo, cancellationToken);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                throw new AppException("Невозможно добавить карту" + ex.Message);
            }
        }

        public async Task<IEnumerable<CardItem>> GetAllUserCards(Guid userId, CancellationToken cancellationToken)
        {
            if (userId == Guid.Empty)
            {
                throw new AppException($"Поле {nameof(userId)} не указано");
            }

            return await repository.GetAllUserCards(userId, cancellationToken);
        }

        public async Task<IEnumerable<Guid>> GetAllUserCardsId(Guid userId, CancellationToken cancellationToken)
        {
            if (userId == Guid.Empty)
            {
                throw new AppException($"Поле {nameof(userId)} не указано");
            }

            return await repository.GetAllUserCardsId(userId, cancellationToken);
        }

        public async Task<CardItem> GetCardByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                throw new AppException($"Поле {nameof(id)} не указано");
            }

            return await repository.GetAsync(id, cancellationToken);
        }

        public async void UpdateCardByIdAsync(CardPatchInfo cardToUpdate, CancellationToken cancellationToken)
        {
            var cardFromRepository = await repository.GetAsync(cardToUpdate.Id, cancellationToken);

            if (cardFromRepository == null)
                throw new AppException("Карта не найдена");

            UpdateCardInfo(cardToUpdate, cardFromRepository);

            if (!IsCardValid(cardFromRepository))
                throw new AppException("Не правильно указана информация");

            await repository.PatchAsync(cardFromRepository, cancellationToken);
        }

        public async void CheckOwnership(CardItem card, Guid userId)
        {
            if (card == null)
                throw new AppException("Карта не найдена");

            if (!IsCardValid(card) || userId == Guid.Empty)
                throw new AppException("Не правильно указана информация");
            var cardInRepo = await repository.GetAsync(card.Id, CancellationToken.None);
            if (cardInRepo.UserId != userId)
                throw new AppException("Нет доступа");
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Incorrect value", nameof(id));
            }

            return await repository.DeleteCardAsync(id);
        }

        public async Task<IEnumerable<CardItem>> GetAllCardsFromList(IEnumerable<Guid> cardsList)
        {
            var result = new List<CardItem>();
            foreach (var cardId in cardsList)
            {
                if (cardId == Guid.Empty)
                {
                    throw new NullReferenceException();
                }

                result.Add(await repository.GetAsync(cardId, CancellationToken.None));
            }

            return result;
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
                throw new AppException($"Не заполнены поля в \"Вопросе\" карты");
            if (!FieldsAreFilled(cardToValidate.Answer))
                throw new AppException($"Не заполнены поля в \"Вопросе\" карты");
            if (!LengthIsCorrect(cardToValidate.Question.Text))
                throw new AppException($"Некорректная длина в \"Вопросе\" карты в поле \"Текст\"" +
                                       $", должна быть от {MinimumTextLength} до {MaximumTextLength}");
            if (!LengthIsCorrect(cardToValidate.Question.Code))
                throw new AppException($"Некорректная длина в \"Вопросе\" карты в поле \"Код\"" +
                                       $", должна быть от {MinimumCodeLength} до {MaximumCodeLength}");
            if (!LengthIsCorrect(cardToValidate.Answer.Text))
                throw new AppException($"Некорректная длина в \"Ответе\" карты в поле \"Текст\"" +
                                       $", должна быть от {MinimumTextLength} до {MaximumTextLength}");
            if (!LengthIsCorrect(cardToValidate.Answer.Code))
                throw new AppException($"Некорректная длина в \"Ответе\" карты в поле \"Код\"" +
                                       $", должна быть от {MinimumCodeLength} до {MaximumCodeLength}");
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
                if (string.IsNullOrWhiteSpace((string) property.GetValue(cardToCheck).ToString()) ||
                    string.IsNullOrEmpty((string) property.GetValue(cardToCheck).ToString()))
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
            if (string.IsNullOrWhiteSpace(stringToCheck) || string.IsNullOrEmpty(stringToCheck))
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