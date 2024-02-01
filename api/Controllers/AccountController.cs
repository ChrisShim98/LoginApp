using api.DTOs;
using api.Entity;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("register")] // POST: api/account/register
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username))
            return BadRequest("Username is taken!");
            
            var user = _mapper.Map<AppUser>(registerDTO);

            user.UserName = registerDTO.Username.ToLower();

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDTO
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        public async Task<bool> UserExists(string username)
        {
            return await Task.FromResult(_userManager.Users.Any(x => x.UserName == username.ToLower()));
        }

        [HttpPost("login")] // POST: api/account/login
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await Task.FromResult(_userManager.Users
                .SingleOrDefault(x => x.UserName == loginDTO.Username.ToLower()));

            if (user == null) return Unauthorized("Invalid username");

            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!result) return Unauthorized("Invalid Password");

            return new UserDTO
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }
    }
}