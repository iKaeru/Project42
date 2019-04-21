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
            /// Переводит подьзователя из серверной модели в клиентскую
            /// </summary>
            /// <param name="modelUser">Пользователь в серверной модели</param>
            /// <returns>Пользователь в клиентской модели</returns>
            public static View.User Convert(Model.User modelUser)
            {
                if (modelUser == null)
                {
                    throw new ArgumentNullException(nameof(modelUser));
                }

                var clientUser = new View.User
                {
                    Id = modelUser.Id.ToString(),
                    Login = modelUser.Login,
                    RegisteredAt = modelUser.RegistrationDate.ToString()
                };

                return clientUser;
            }
            
            /// <summary>
            /// Переводит подьзователя из клиентской модели в серверную
            /// </summary>
            /// <param name="clientUser">Пользователь в клиентской модели</param>
            /// <returns>Пользователь в серверной модели</returns>
            public static Model.User Convert(View.User clientUser)
            {
                if (clientUser == null)
                {
                    throw new ArgumentNullException(nameof(clientUser));
                }

                var modelUser = new Model.User();

                return modelUser;
            }
        }
}