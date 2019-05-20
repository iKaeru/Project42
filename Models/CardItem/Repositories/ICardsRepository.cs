using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.CardItem.Repositories
{
    /// <summary>
    /// Интерфейс хранилища карт
    /// </summary>
    public interface ICardsRepository
    {
        /// <summary>
        /// Создать новую карту
        /// </summary>
        /// <param name="cardToCreate">Информация для создания карты</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронное создание карты.
        /// Результат выполнения операции - информация о созданной карте</returns>
        Task<CardItemInfo> CreateAsync(CardItem cardToCreate, CancellationToken cancellationToken);

        /// <summary>
        /// Выдать все карты, соответствующие пользователю
        /// </summary>
        /// <param name="uId">Информация об Id пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронное создание карты.
        /// Результат выполнения операции - информация о созданной карте</returns>
        Task<IEnumerable<CardItem>> GetAllUserCards(Guid uId, CancellationToken cancellationToken);
        
        /// <summary>
        /// Найти карты
        /// </summary>
        /// <param name="query">Поисковый запрос</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный поиск карты.
        /// Результат выполнения - список найденных карт</returns>
        Task<IReadOnlyList<CardItemInfo>> SearchAsync(CardSearchInfo query, CancellationToken cancelltionToken);

        /// <summary>
        /// Запросить карту
        /// </summary>
        /// <param name="cardId">Идентификатор карты</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представлящая асинхронный запрос карты.
        /// Результат выполнения - карта</returns>
        Task<CardItem> GetAsync(Guid cardId, CancellationToken cancellationToken);

        /// <summary>
        /// Изменить карту
        /// </summary>
        /// <param name="patchInfo">Описание изменений карты</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный запрос на изменение карты.
        /// Результат выполнения - актуальное состояние карты</returns>
        Task<CardItem> PatchAsync(CardPatchInfo patchInfo, CancellationToken cancelltionToken);

        /// <summary>
        /// Удалить карту
        /// </summary>
        /// <param name="cardId">Идентификатор карту</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный запрос на удаление карты</returns>
        Task RemoveAsync(Guid cardId, CancellationToken cancelltionToken);
    }
}