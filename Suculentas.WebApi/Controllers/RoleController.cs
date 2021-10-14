using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Suculentas.Domain.Identity;
using Suculentas.WebApi.Dtos;

namespace Suculentas.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {        
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(IMapper mapper, RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet("getRole")]
        public async Task<IActionResult> GetRoles() 
        {
            try
            {                
                var roles = await _roleManager.Roles.ToListAsync();

                return Ok(roles);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getRoleAndUsers")]
        public async Task<IActionResult> GetRoleAndUsers()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                List<UserByRoleDTO> roleByUser = new List<UserByRoleDTO>();

                foreach (var role in roles)
                {
                    var users = await _userManager.GetUsersInRoleAsync(role.Name);
                    var usersMapper = _mapper.Map<List<UserDTO>>(users);

                    UserByRoleDTO valueLocal = new UserByRoleDTO();
                    valueLocal.RoleName = role.Name;
                    valueLocal.Users = usersMapper;

                    roleByUser.Add(valueLocal);
                }

                return Ok(roleByUser);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost("createRole")]
        public async Task<IActionResult> CreateRole(RoleDTO roleDto) 
        {
            try
            {
                var returnValue = await _roleManager.CreateAsync(new Role { Name = roleDto.Name });

                return Ok(returnValue);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("updateUserRole")]
        public async Task<IActionResult> UpdateUserRole(UpdateUserRoleDTO model) 
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var result = new IdentityResult();

                if(user != null) 
                {

                    if(model.Delete)
                        result = await _userManager.RemoveFromRoleAsync(user, model.Role);

                    else
                        result = await _userManager.AddToRoleAsync(user, model.Role);
                }
                else 
                {
                    return this.StatusCode(StatusCodes.Status404NotFound, "Usuario não encontrado!");
                }

                return Ok(result);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}