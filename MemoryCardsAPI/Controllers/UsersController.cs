using System;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using MemoryCardsAPI.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Converters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using Models.Errors;
using Models.User;
using Models.User.Services;
using View = Client.Models.User;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using Models.Token.Services;
using Models.Token;

namespace MemoryCardsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/api")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly AppSettings appSettings;

        public UsersController(IUserService userService, ITokenService tokenService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.appSettings = appSettings.Value;
        }

        /// <summary>
        /// Authorizatе User
        /// </summary>
        /// <param name="userDto">Информация о пользователе для авторизации</param>
        /// <returns code="200"></returns>
        [AllowAnonymous]
        [SwaggerResponse(200, Type=typeof(View.UserAuthorizedInfo))]
        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginInfo userDto)
        {
            try
            {
                var user = await userService.AuthenticateAsync(userDto.Login, userDto.Password);
                if (user == null)
                    return BadRequest(new {message = "Пользователь или пароль указан не верно"});
                var token = TokenHmacSha256Generator(user.Id.ToString());

                CookieOptions option = new CookieOptions();
                option.HttpOnly = true;
                option.SameSite = SameSiteMode.None;
                if (userDto.RememberMe == true)
                {
                    option.Expires = DateTimeOffset.Now.AddDays(5);
                }

                HttpContext.Response.Cookies.Append("token", token, option);


                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);

                var resultUser = new View.UserAuthorizedInfo
                {
                    Id = user.Id,
                    Username = user.Login,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };



                return Ok(resultUser);
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="userDto">Информация о пользователе для регистрации</param>
        /// <param name="cancellationToken"></param>
        /// <returns code="200"></returns>
        [AllowAnonymous]
        [SwaggerResponse(200, Type=typeof(View.UserRegistredInfo))]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationInfo userDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var user = userService.CreateUser(userDto);
                await userService.ValidateUserAsync(user);

                await userService.AddUserAsync(user, userDto.Password, cancellationToken);
                
                var resultUser = new View.UserRegistredInfo
                {
                    Id = user.Id,
                    Username = user.Login
                };

                MailingService emailService = new MailingService();
                await emailService.SendEmailAsync(userDto.EmailAdress,
                    "Успешная регистрация", "Поздравляем, " + user.Login + ", вы зарегистрировались и можете зайти в профиль на https://pr42.ru/login ! \n P.S. Подтверждения почты пока нет, но скоро будет! \n С уважением, администрация pr42.ru");
                Console.WriteLine("Email to {0} was sent", userDto.EmailAdress);
                return Ok(resultUser);
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Get User Info
        /// </summary>
        /// <returns code="200"></returns>
        [HttpGet("info")]
        [SwaggerResponse(200, Type=typeof(View.UserRegistredInfo))]
        public async Task<IActionResult> GetById()
        {
            try
            {
                Guid.TryParse(HttpContext.User.Identity.Name, out var uId);
                var user = await userService.GetByIdAsync(uId);

                if (user == null)
                    return BadRequest(new {message = "Идентификатор пользователя указан не верно"});

                var userDto = mapper.Map<View.User>(user);
                var resultUser = new View.UserRegistredInfo
                {
                    Id = user.Id,
                    Username = user.Login
                };
                return Ok(resultUser);
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Update User By Id
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="userToUpdate">Информация о пользователе для редактирования</param>
        /// <returns code="200"></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] View.UserPatchInfo userToUpdate)
        {
            var user = UserConverter.ConvertPatchInfo(userToUpdate);
            var guidId = Guid.Parse(id);
            Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
            
            if (userId != guidId)
            {
                return BadRequest(new {message = "Запрещено для этого пользователя"});
            }
            
            user.Id = guidId;
            
            try
            {
                await userService.UpdateAsync(user, userToUpdate.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Delete User By Id
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns code="200"></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guidId = Guid.Parse(id);

                Guid.TryParse(HttpContext.User.Identity.Name, out var userId);
            
                if (userId != guidId)
                {
                    return BadRequest(new {message = "Запрещено для этого пользователя"});
                }
                
                if (await userService.Delete(guidId))
                {
                    return Ok();
                }

                throw new AppException("Не получилось удалить пользователя");
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [AllowAnonymous]
        [HttpPost("ask-pass-reset")]
        public async Task<IActionResult> AskPasswordReset([FromBody]string email)
        {
            try
            {
                var user = await userService.GetUserByEmail(email);

                if (user == null) return Ok("Письмо выслано на " + email); //мера безопасности

                var token = new PasswordResetToken(user.Id);
                await tokenService.AddPasswordResetTokenAsync(token);
                await new MailingService().SendEmailAsync(email,
                    "Восстановление пароля",
                    "Для восстановления пароля перейдите по ссылке: https://pr42.ru/re-regist?" + token.token + " (ссылка действительна 3 часа)");
                return Ok("Письмо выслано на " + email);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("re-regist")]
        public async Task<IActionResult> ResetPassword([FromQuery]string tokenValue, [FromBody]string newPassword)
        {
            try
            {
                var token = await tokenService.GetPasswordResetTokenAsync(tokenValue);
                var userToUpdate = await userService.GetByIdAsync(token.userId);
                var userPatchInfo = new UserPatchInfo
                {
                    EmailAdress = userToUpdate.EmailAdress,
                    FirstName = userToUpdate.FirstName==null ? "none" : userToUpdate.FirstName,
                    Id = userToUpdate.Id,
                    LastName = userToUpdate.LastName==null ? "none" : userToUpdate.LastName,
                    Login = userToUpdate.Login,
                    Password = newPassword
                };
                await userService.UpdateAsync(userPatchInfo, newPassword);

                await new MailingService().SendEmailAsync(userToUpdate.EmailAdress,
                    "Изменение пароля",
                    "Пароль для сайта https://pr42.ru был успешно изменён! Логин : " + userToUpdate.Login);

                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        private string TokenHmacSha256Generator(string id)
        {
            IdentityModelEventSource.ShowPII = true;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}