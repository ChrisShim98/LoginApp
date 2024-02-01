using api.DTOs;
using api.Entity;
using api.Helpers;
using api.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public UserRepository(DataContext context, IMapper mapper,    
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;       
        }

        public async void DeleteUser(int id)
        {
            AppUser user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            await _userManager.DeleteAsync(user);
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<PagedList<AppUserDTO>> GetUsersAsync(PaginationParams paginationParams)
        {
            var query = _context.Users.AsQueryable();

            if (paginationParams.SearchQuery != "") {
                query = query.Where(x => x.UserName.Contains(paginationParams.SearchQuery));
            }

            query = query.OrderBy(u => u.Id);          

            return await PagedList<AppUserDTO>.CreateAsync(
                query.AsNoTracking().ProjectTo<AppUserDTO>(_mapper.ConfigurationProvider),
                paginationParams.PageNumber, 
                paginationParams.PageSize);
        }
    }
}