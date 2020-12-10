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
using Suculentas.Domain.Identity;
using Suculentas.WebApi.Dtos;
using Suculentas.WebApi.Utils;

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
        private readonly EnvioEmail _envio = new EnvioEmail();
        private readonly CorpoEmail _bodyEmail = new CorpoEmail();

        public UserController(IConfiguration config,
                              UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IMapper mapper)
        {
           this. _config = config;
           this._userManager = userManager;
           this._signInManager = signInManager;
           this._mapper = mapper;
        }

        [HttpGet("GetUser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser()
        {
            return Ok(new UserDto());
        }

        [HttpGet("GetUserBD")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserBD()
        {
            try
            {
                User user = await _userManager.FindByEmailAsync("1234@1234");

                var results = _mapper.Map<UserDto>(user);

                return Ok(results);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email);

                if(user == null) 
                {
                    user = new User 
                    {
                        UserName = userDto.Email,
                        FullName = userDto.FullName,
                        CPF = userDto.CPF,
                        DataNascimento = Convert.ToDateTime(userDto.DataNascimento),
                        PhoneNumber = userDto.PhoneNumber,
                        Email = userDto.Email
                    };

                    var result = await _userManager.CreateAsync(user, userDto.Password);

                    if (result.Succeeded) 
                    {
                        var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == user.Email.ToUpper());
                        var token = GenerateJwToken(appUser).Result;

                        var userToReturn = _mapper.Map<UserDto>(appUser);
                        //var confirmationEmail = Url.Action("ConfirmEmailAddress", "Home", 
                        //    new { token = token, email = user.Email }, Request.Scheme);

                        //System.IO.File.WriteAllText("confirmationEmail.txt", confirmationEmail);

                        //EnvioEmail email = new EnvioEmail();

                        //email.EnviarEmail("castrohenrique13899@gmail.com","Teste","testando mensagem");

                        return Ok(new {
                            token = GenerateJwToken(appUser).Result,
                            user = userToReturn
                        });
                    }

                    return this.StatusCode(StatusCodes.Status401Unauthorized, result.Errors); 
                }

                return this.StatusCode(StatusCodes.Status401Unauthorized, "DuplicateUserName"); 
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " + ex.Message);
            }
        }

        [HttpPost("EsqueciSenha")]
        [AllowAnonymous]
        public async Task<IActionResult> EsqueciSenha(EsqueciSenhaDto EsqueciSenhaDto) 
        {
            var user = await _userManager.FindByEmailAsync(EsqueciSenhaDto.Email);

            if(user != null) 
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);

                string tokenWeb = HttpUtility.UrlEncode(token);

                var resetURL = "https://suculentasdaro.com.br/user/reset/" + user.Email + "/" + tokenWeb;

                string corpoEmail = _bodyEmail.BodyEsqueciSenha(resetURL, user.FullName);

                _envio.EnviarEmail(user.Email, "[Suculentas da Rô] Solicitação para redefinir sua senha" , corpoEmail);

                return Ok(new ResetarSenhaDto { Token = token, Email = EsqueciSenhaDto.Email});

            }
            else 
            {
                return this.StatusCode(StatusCodes.Status404NotFound, "Usuario não encontrado!");
            }
        }

        [HttpPost("ResetarSenha")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetarSenha(ResetarSenhaDto ResetarSenhaDto) 
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(ResetarSenhaDto.Email);

                if(user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, ResetarSenhaDto.Token, ResetarSenhaDto.Password);

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
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " + ex.Message);
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user =  await _userManager.FindByEmailAsync(userLogin.Email);

                if(user != null) 
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);

                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.Users
                            .FirstOrDefaultAsync(u => u.NormalizedEmail == userLogin.Email.ToUpper());

                        var userToReturn = _mapper.Map<UserDto>(appUser);

                        return Ok(new {
                            token = GenerateJwToken(appUser).Result,
                            user = userToReturn
                        });
                    }
                }

                return Unauthorized();
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " + ex.Message);
            }
        }

        private async Task<string> GenerateJwToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.SerialNumber, user.CPF),
                new Claim(ClaimTypes.DateOfBirth, user.DataNascimento.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
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