using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using MemoryCardsAPI.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Client.Models.User;
using MemoryCardsAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;

namespace MemoryCardsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService userService;
        private IMapper mapper;
        private readonly AppSettings appSettings;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] UserLoginInfo userDto)
        {
            var user = userService.Authenticate(userDto.Login, userDto.Password);

            if (user == null)
                return BadRequest(new {message = "Username or password is incorrect"});
            IdentityModelEventSource.ShowPII = true;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                Username = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegistrationInfo userDto)
        {
            // map dto to entity
            var modelUser = mapper.Map<Models.User.User>(userDto);

            var user = new Models.User.User()
            {
                RegistrationDate = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Login = userDto.Login,
                EmailAdress = userDto.EmailAdress
            };

            try
            {
                // save 
                userService.Create(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = userService.GetAll();
            var userDtos = mapper.Map<IList<UserRegistrationInfo>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var GuidId = Guid.Parse(id);
            var user = userService.GetById(GuidId);
            var userDto = mapper.Map<UserRegistrationInfo>(user);
            return Ok(userDto);
        }

/*        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UserRegistrationInfo userDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<User>(userDto);
            user.Id = id.ToString();

            try 
            {
                // save 
                _userService.Update(user, userDto.Password);
                return Ok();
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }*/

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            userService.Delete(id);
            return Ok();
        }
    }
}