using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Errors;
using Models.User.Repositories;

namespace Models.User.Services
{
    public class UserService : IUserService
    {
        private const int MinimumLoginLength = 6;
        private const int MaximumLoginLength = 32;
        private const int MinimumPassLength = 8;
        private const int MaximumPassLength = 120;
        private const int MinimumEmailLength = 6;
        private const int MaximumEmailLength = 250;
        private const int MinimumFirstNameLength = 1;
        private const int MaximumFirstNameLength = 300;
        private const int MinimumLastNameLength = 1;
        private const int MaximumLastNameLength = 300;

        private readonly IUsersRepository repository;

        public UserService(IUsersRepository repository)
        {
            this.repository = repository;
        }

        public async Task ValidateUserAsync(User userToValidate)
        {
            var login = userToValidate.Login;
            if (IsFilled(login) && await repository.FindLoginAsync(login))
                throw new AppException("Логин \"" + login + "\" уже занят");

            var email = userToValidate.EmailAdress;
            if (IsFilled(email) && await repository.FindMailAsync(email))
                throw new AppException("Почтовый адрес \"" + email + "\" уже занят");
        }

        public User CreateUser(UserRegistrationInfo userToCreate)
        {
            ValidateUser(userToCreate);

            var user = new User
            {
                EmailAdress = userToCreate.EmailAdress,
                Login = userToCreate.Login,
                RegistrationDate = DateTime.Now
            };

            return user;
        }

        public async Task<bool> AddUserAsync(User userToAdd, string password, CancellationToken cancellationToken)
        {
            IsFilled(password);

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            userToAdd.PasswordHash = passwordHash;
            userToAdd.PasswordSalt = passwordSalt;

            try
            {
                await repository.CreateAsync(userToAdd, cancellationToken);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            IsFilled(username);
            IsFilled(password);

            var user = await repository.SearchUserAsync(username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<User> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                Console.WriteLine(Guid.Empty);
                Console.WriteLine(11);
                return null;
            }

            return await repository.GetUserAsync(id);
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Incorrect value", nameof(id));
            }

            return await repository.DeleteUserAsync(id);
        }

        public async Task UpdateAsync(UserPatchInfo userToUpdate, string password = null)
        {
            ValidateUser(userToUpdate);
            var userFromRepository = await repository.GetUserAsync(userToUpdate.Id);

            if (userFromRepository == null)
                throw new AppException("Пользователь не найден");

            CheckInfoNotExist(userToUpdate, userFromRepository);

            UpdateUserInfo(userToUpdate, userFromRepository);

            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                userFromRepository.PasswordHash = passwordHash;
                userFromRepository.PasswordSalt = passwordSalt;
            }

            await repository.UpdateAsync(userFromRepository);
        }

        #region private helper methods

        private void ValidateUser(UserRegistrationInfo userToValidate)
        {
            if (!FieldsAreFilled(userToValidate))
                throw new AppException("Недостаточно информации про пользователя");
            if (!LengthIsCorrect(userToValidate.Login, MinimumLoginLength, MaximumLoginLength))
                throw new AppException($"Длина логина не правильная, должна быть от {MinimumLoginLength}" +
                                       $"и до {MaximumLoginLength}");
            if (!LengthIsCorrect(userToValidate.Password, MinimumPassLength, MaximumPassLength))
                throw new AppException($"Длина пароля не правильная, должна быть от {MinimumPassLength}" +
                                       $"и до {MaximumPassLength}");
            if (!LengthIsCorrect(userToValidate.EmailAdress, MinimumEmailLength, MaximumEmailLength))
                throw new AppException($"Длина почты не правильная, должна быть от {MinimumEmailLength}" +
                                       $"и до {MaximumEmailLength}");
        }

        private void ValidateUser(UserPatchInfo userToValidate)
        {
            if (!FieldsAreFilled(userToValidate))
                throw new AppException("Недостаточно информации про пользователя");
            if (!LengthIsCorrect(userToValidate.Login, MinimumLoginLength, MaximumLoginLength))
                throw new AppException($"Длина логина не правильная, должна быть от {MinimumLoginLength}" +
                                       $"и до {MaximumLoginLength}");
            if (!LengthIsCorrect(userToValidate.Password, MinimumPassLength, MaximumPassLength))
                throw new AppException($"Длина пароля не правильная, должна быть от {MinimumPassLength}" +
                                       $"и до {MaximumPassLength}");
            if (!LengthIsCorrect(userToValidate.EmailAdress, MinimumEmailLength, MaximumEmailLength))
                throw new AppException($"Длина почты не правильная, должна быть от {MinimumEmailLength}" +
                                       $"и до {MaximumEmailLength}");
            if (!LengthIsCorrect(userToValidate.FirstName, MinimumFirstNameLength, MaximumFirstNameLength))
                throw new AppException($"Длина имени не правильная, должна быть от {MinimumFirstNameLength}" +
                                       $"и до {MaximumFirstNameLength}");
            if (!LengthIsCorrect(userToValidate.LastName, MinimumLastNameLength, MaximumLastNameLength))
                throw new AppException($"Длина имени не правильная, должна быть от {MinimumLastNameLength}" +
                                       $"и до {MaximumLastNameLength}");
        }

        private bool FieldsAreFilled(UserPatchInfo userToCheck)
        {
            var type = userToCheck.GetType();
            foreach (var property in type.GetProperties())
            {
                if (property.GetValue(userToCheck) == null)
                {
                    return false;
                }
            }

            return true;
        }

        private bool FieldsAreFilled(UserRegistrationInfo userToCheck)
        {
            var type = userToCheck.GetType();
            foreach (var property in type.GetProperties())
            {
                if (property.GetValue(userToCheck) == null)
                {
                    return false;
                }
            }

            return true;
        }

        private bool LengthIsCorrect(string stringToCheck, int minValue, int maxValue)
        {
            if (stringToCheck.Length < minValue || stringToCheck.Length > maxValue)
            {
                return false;
            }

            return true;
        }

        private bool IsFilled(string userInfo)
        {
            if (string.IsNullOrWhiteSpace(userInfo) || string.IsNullOrEmpty(userInfo))
                throw new AppException($"Поля {nameof(userInfo)} не указано");
            return true;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.",
                    nameof(password));
            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).",
                    nameof(storedHash));
            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).",
                    nameof(storedSalt));

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        private async void CheckInfoNotExist(UserPatchInfo newInfo, User repositoryInfo)
        {
            if (newInfo.Login != repositoryInfo.Login)
            {
                if (await repository.FindLoginAsync(newInfo.Login))
                    throw new AppException("Пользователь " + newInfo.Login + " уже занят");
            }

            if (newInfo.EmailAdress != repositoryInfo.EmailAdress)
            {
                if (await repository.FindMailAsync(newInfo.EmailAdress))
                    throw new AppException("Почтовый адрес " + newInfo.EmailAdress + " уже занят");
            }
        }

        private void UpdateUserInfo(UserPatchInfo userToUpdate, User userFromRepository)
        {
            userFromRepository.FirstName = userToUpdate.FirstName;
            userFromRepository.LastName = userToUpdate.LastName;
            userFromRepository.Login = userToUpdate.Login;
            userFromRepository.EmailAdress = userToUpdate.EmailAdress;
        }

        #endregion
    }
}