using api.DTOs;
using api.Entity;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        public UsersController(IUserRepository userRepository, UserManager<AppUser> userManager,
            ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<AppUserDTO>>> GetUsers([FromQuery]PaginationParams paginationParams)
        {
            var users = await _userRepository.GetUsersAsync(paginationParams);
            
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, 
                users.PageSize, users.TotalCount, users.TotalPages));
                
            return Ok(users);
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteUser([FromQuery]int id)
        {
            if (!await UserExists(id)) {
                return BadRequest("User does not exist");
            }
            
            _userRepository.DeleteUser(id);
            return Ok();
        }
        private async Task<bool> UserExists(int id)
        {
            return await _userManager.Users.AnyAsync(x => x.Id == id);
        }

    }
}