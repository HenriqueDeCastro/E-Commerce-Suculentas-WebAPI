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

        [HttpGet("GetByEmail/{Email}")]
        public async Task<IActionResult> GetUser(string email) 
        {
            try
            {                
                var user = await _userManager.FindByEmailAsync(email);

                if(user == null) {
                    return NotFound();
                }

                var userToReturn = _mapper.Map<UserDto>(user);

                return Ok(userToReturn);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " + ex.Message);
            }
        }

        [HttpGet("GetByRole/{RoleName}")]
        public async Task<IActionResult> GetByRole(string RoleName) 
        {
            try
            {                
                var user = await _userManager.GetUsersInRoleAsync(RoleName);

                if(user == null) {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " + ex.Message);
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email);
                string data = null;

                if (user == null) 
                {
                    if(!string.IsNullOrEmpty(userDto.DataNascimento))
                    {
                        data = userDto.DataNascimento;
                    }
                    user = new User 
                    {
                        UserName = userDto.Email,
                        FullName = userDto.FullName,
                        CPF = userDto.CPF,
                        DataNascimento = Convert.ToDateTime(data),
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
            try
            {
                var user = await _userManager.FindByEmailAsync(EsqueciSenhaDto.Email);

                if (user != null)
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    string tokenWeb = HttpUtility.UrlEncode(token);

                    string resetURL = "https://suculentasdaro.com.br/user/reset/" + user.Email + "/" + tokenWeb;
                    string corpo = _email.BodyEsqueciSenha(resetURL, user.FullName);
                    string Assunto = "Solicitação para redefinir sua senha";

                    try
                    {
                        _email.EnviarEmailSuculentas(user.Email, Assunto, corpo);
                    }
                    catch (Exception ex)
                    {

                        LogEmail log = new LogEmail();
                        log.Para = user.Email;
                        log.Assunto = Assunto;
                        log.Corpo = corpo;
                        log.ExceptionMensagem = ex.Message;

                        _repo.Add(log);
                        return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao enviar e-mail");
                    }

                    return Ok(new ResetarSenhaDto { Token = token, Email = EsqueciSenhaDto.Email });

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

        [HttpPut("AtualizarUsuario")]
        public async Task<IActionResult> Atualizar(UserDto user)
        {
            try
            {
                var usuario = await _userManager.FindByEmailAsync(user.Email);

                if(usuario == null) 
                {
                    return this.StatusCode(StatusCodes.Status404NotFound, "Usuario não encontrado!");
                }

                User usuarioAtt = _mapper.Map(user, usuario);
                var result = await _userManager.UpdateAsync(usuarioAtt);

                if (result.Succeeded) 
                {
                    var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == user.Email.ToUpper());
                    var token = GenerateJwToken(appUser).Result;

                    var userToReturn = _mapper.Map<UserDto>(appUser);

                    return Ok(new {
                        token = GenerateJwToken(appUser).Result,
                        user = userToReturn
                    });
                }

                return this.StatusCode(StatusCodes.Status401Unauthorized, result); 
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
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
                        var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == userLogin.Email.ToUpper());
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
                new Claim(type: "id", user.Id.ToString()),
                new Claim(type: "fullName", user.FullName),
                new Claim(type: "cpf", user.CPF),
                new Claim(type: "dataNascimento", user.DataNascimento.ToString()),
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