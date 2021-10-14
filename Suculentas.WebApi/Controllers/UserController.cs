using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Suculentas.Domain;
using Suculentas.Domain.Identity;
using Suculentas.Email;
using Suculentas.Repository;
using Suculentas.WebApi.Dtos;

namespace Suculentas.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly ISuculentasEmail _email;
        private readonly ISuculentasRepository _repo;

        public UserController(IConfiguration config,
                              UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IMapper mapper,
                              ISuculentasEmail email,
                              ISuculentasRepository repo)
        {
           this._config = config;
           this._userManager = userManager;
           this._signInManager = signInManager;
           this._mapper = mapper;
           this._email = email;
           this._repo = repo;
        }

        [HttpGet("getTestServer")]
        public async Task<IActionResult> GetTesteServer()
        {
            try
            {
                return Ok("Ok");
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getByEmail/{email}")]
        public async Task<IActionResult> GetUser(string email) 
        {
            try
            {                
                var user = await _userManager.FindByEmailAsync(email);

                if(user == null) {
                    return NotFound();
                }

                var userToReturn = _mapper.Map<UserDTO>(user);

                return Ok(userToReturn);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getByRole/{roleName}")]
        public async Task<IActionResult> GetByRole(string roleName) 
        {
            try
            {                
                var user = await _userManager.GetUsersInRoleAsync(roleName);

                if(user == null) {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDTO userDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email);
                string data = null;

                if(userDto.AcceptTerms == false) {
                    return this.StatusCode(StatusCodes.Status406NotAcceptable);
                }
                else if (user == null) 
                {
                    if(!string.IsNullOrEmpty(userDto.BirthDate))
                    {
                        data = userDto.BirthDate;
                    }
                    user = new User 
                    {
                        UserName = userDto.Email,
                        FullName = userDto.FullName,
                        CPF = userDto.CPF,
                        BirthDate = Convert.ToDateTime(data),
                        PhoneNumber = userDto.PhoneNumber,
                        Email = userDto.Email,
                        AcceptTerms = userDto.AcceptTerms
                    };

                    var result = await _userManager.CreateAsync(user, userDto.Password);

                    if (result.Succeeded) 
                    {
                        var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == user.Email.ToUpper());
                        var token = GenerateJwToken(appUser).Result;

                        var userToReturn = _mapper.Map<UserDTO>(appUser);

                        return Ok(new {
                            token = GenerateJwToken(appUser).Result,
                            user = userToReturn
                        });
                    }

                    return this.StatusCode(StatusCodes.Status501NotImplemented, result.Errors); 
                }
                else {
                    return this.StatusCode(StatusCodes.Status412PreconditionFailed, "DuplicateUserName"); 
                }
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> EsqueciSenha(ForgotPasswordDTO forgotPassword) 
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(forgotPassword.Email);

                if (user != null)
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    string tokenWeb = HttpUtility.UrlEncode(token);

                    string resetURL = "http://localhost:4200/user/reset-password/" + user.Email + "/" + tokenWeb;
                    //string resetURL = "https://suculentasdaro.com.br/user/reset-password/" + user.Email + "/" + tokenWeb;
                    string body = _email.BodyForgotPassword(resetURL, user.FullName);
                    string topic = "Solicitação para redefinir sua senha";

                    try
                    {
                        _email.SendEmailSuculentas(user.Email, topic, body);
                        return Ok();
                    }
                    catch (Exception e)
                    {

                        LogEmail log = new LogEmail();
                        log.To = user.Email;
                        log.Topic = topic;
                        log.Body = body;
                        log.ExceptionMessage = e.Message;

                        _repo.Add(log);
                        return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao enviar e-mail");
                    }
                }
                else
                {
                    return this.StatusCode(StatusCodes.Status404NotFound, "Usuario não encontrado!");
                }
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetarSenha(ResetPasswordDTO resetPassword) 
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(resetPassword.Email);

                if(user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);

                    if(!result.Succeeded) 
                    {
                        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
                    }

                    return Ok();
                }
                else 
                {
                    return this.StatusCode(StatusCodes.Status404NotFound, "Usuario não encontrado!");
                } 
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> Atualizar(UserDTO model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if(user == null) 
                {
                    return this.StatusCode(StatusCodes.Status404NotFound, "Usuario não encontrado!");
                }

                User usuarioAtt = _mapper.Map(model, user);
                var result = await _userManager.UpdateAsync(usuarioAtt);

                if (result.Succeeded) 
                {
                    var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == model.Email.ToUpper());

                    var token = GenerateJwToken(appUser).Result;

                    var userToReturn = _mapper.Map<UserDTO>(appUser);

                    return Ok(new {
                        token = GenerateJwToken(appUser).Result,
                        user = userToReturn
                    });
                }

                return this.StatusCode(StatusCodes.Status401Unauthorized, result); 
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDTO userLogin)
        {
            try
            {
                var user =  await _userManager.FindByEmailAsync(userLogin.Email);

                if(user != null) 
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);

                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == userLogin.Email.ToUpper());
                        var userToReturn = _mapper.Map<UserDTO>(appUser);

                        return Ok(new {
                            token = GenerateJwToken(appUser).Result,
                            user = userToReturn
                        });
                    }
                }

                return this.StatusCode(StatusCodes.Status403Forbidden);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        private async Task<string> GenerateJwToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(type: "id", user.Id.ToString()),
                new Claim(type: "fullName", user.FullName),
                new Claim(type: "cpf", user.CPF),
                new Claim(type: "birthDate", user.BirthDate.ToString()),
                new Claim(type: "phoneNumber", user.PhoneNumber),
                new Claim(type: "email", user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach(var role in roles) 
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}