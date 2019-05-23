using System;
using View = Client.Models.User;
using Model = Models.User;

namespace Converters
{
        /// <summary>
        /// Предоставляет методы конвертирования пользователя между серверной и клиентской моделями
        /// </summary>
        public static class UserConverter
        {
            /// <summary>
            /// Переводит информацию для редактирования пользователя из клиентской модели в серверную
            /// </summary>
            /// <param name="viewUser">Пользователь в клиентской модели</param>
            /// <returns>Пользователь в серверной модели</returns>
            public static Model.UserPatchInfo ConvertPatchInfo(View.UserPatchInfo viewUser)
            {
                if (viewUser == null)
                {
                    throw new ArgumentNullException(nameof(viewUser));
                }

                var clientUser = new Model.UserPatchInfo()
                {
                    EmailAdress = viewUser.EmailAdress,
                    Login = viewUser.Login,
                    Password = viewUser.Password,
                    FirstName = viewUser.FirstName,
                    LastName= viewUser.LastName,
                    Id = Guid.Empty
                };

                return clientUser;
            }
        }
}