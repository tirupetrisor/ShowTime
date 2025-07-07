using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Abstractions;

public interface IUserService
{
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<IList<User>> GetAllUsersAsync();
    Task UpdateUserRoleAsync(int userId, Role newRole);
    Task DeleteUserAsync(int userId);
}
