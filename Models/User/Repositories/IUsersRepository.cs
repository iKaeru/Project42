using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.CardItem;

namespace Models.User.Repositories
{
    public interface IUsersRepository
    {
        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        /// <param name="userToCreate">Информация для создания пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронное создание пользователя.
        /// Результат выполнения операции - информация о созданном пользователе</returns>
        Task<UserInfo> CreateAsync(User userToCreate, CancellationToken cancellationToken);

        /// <summary>
        /// Найти пользователя по логину
        /// </summary>
        /// <param name="username">Логин для поиска пользователя</param>
        /// <returns>Найденный пользователь или null.
        /// Результат выполнения операции - информация о найденном пользователе</returns>
        Task<User> SearchUserAsync(string username);

        /// <summary>
        /// Найти пользователя по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор для поиска пользователя</param>
        /// <returns>Найденный пользователь или null.
        /// Результат выполнения операции - информация о найденном пользователе</returns>
        Task<User> GetUserAsync(Guid id);
        
        /// <summary>
        /// Удалить пользователя по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор для удаления пользователя</param>
        /// <returns>True (если пользователь удалён) или false(ошибка).
        /// Результат выполнения операции - true/false</returns>
        Task<bool> DeleteUserAsync(Guid id);
        
        /// <summary>
        /// Найти карты
        /// </summary>
        /// <param name="query">Поисковый запрос</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный поиск пользователя.
        /// Результат выполнения - список найденных пользователей</returns>
        Task<IReadOnlyList<CardItemInfo>> SearchAsync(UserSearchInfo query, CancellationToken cancelltionToken);

        /// <summary>
        /// Запросить карту
        /// </summary>
        /// <param name="userId">Идентификатор карты</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представлящая асинхронный запрос карты.
        /// Результат выполнения - карта</returns>
        Task<UserInfo> GetAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Изменить карту
        /// </summary>
        /// <param name="updateInfo">Описание изменений карты</param>
        /// <returns>Задача, представляющая асинхронный запрос на изменение карты.
        /// Результат выполнения - актуальное состояние карты</returns>
        Task<User> UpdateAsync(User updateInfo);

        /// <summary>
        /// Удалить карту
        /// </summary>
        /// <param name="userId">Идентификатор карту</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный запрос на удаление карты</returns>
        Task RemoveAsync(Guid userId, CancellationToken cancelltionToken);

        Task<bool> FindLoginAsync(string login);
        Task<bool> FindMailAsync(string mailAddress);
    }
}