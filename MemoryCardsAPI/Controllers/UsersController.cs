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
using Model = Models.User;

namespace MemoryCardsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/api")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly AppSettings appSettings;

        public UsersController(IUserService userService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginInfo userDto)
        {
            try
            {
                var user = await userService.AuthenticateAsync(userDto.Login, userDto.Password);
                if (user == null)
                    return BadRequest(new {message = "Username or password is incorrect"});
                var token = TokenHmacSha256Generator(user.Id.ToString());
                return Ok(new
                {
                    Id = user.Id,
                    Username = user.Login,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = token
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationInfo userDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var user = userService.CreateUser(userDto);
                userService.ValidateUser(user);

                await userService.AddUserAsync(user, userDto.Password, cancellationToken);
                return Ok(new
                {
                    Id = user.Id,
                    Username = user.Login
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var guidId = Guid.Parse(id);
                var user = await userService.GetById(guidId);

                if (user == null)
                    return BadRequest(new {message = "User id is incorrect"});

                var userDto = mapper.Map<View.User>(user);
                return Ok(userDto);
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] View.UserPatchInfo userToUpdate)
        {
            var user = UserConverter.ConvertPatchInfo(userToUpdate);
            var guidId = Guid.Parse(id);
            user.Id = guidId;
            
            try
            {
                userService.Update(user, userToUpdate.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guidId = Guid.Parse(id);

                if (await userService.Delete(guidId))
                {
                    return Ok();
                }

                throw new AppException("Couldn't delete user");
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message});
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