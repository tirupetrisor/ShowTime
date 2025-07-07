using ShowTime.BusinessLogic.Abstractions;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace ShowTime.BusinessLogic.Services;

public class UserService(IRepository<User> userRepository) : IUserService
{
    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        try
        {
            Console.WriteLine($"=== LOGIN ATTEMPT ===");
            Console.WriteLine($"Email: {loginDto.Email}");
            Console.WriteLine($"Password length: {loginDto.Password?.Length ?? 0}");
            
            var users = await userRepository.GetAllAsync();
            Console.WriteLine($"Total users in database: {users.Count()}");
            
            // Debug: afișează toți utilizatorii
            foreach (var u in users)
            {
                Console.WriteLine($"User in DB: Email='{u.Email}', Role={u.Role}, PasswordHash='{u.Password.Substring(0, Math.Min(10, u.Password.Length))}...'");
            }
            
            var user = users.FirstOrDefault(u => u.Email.Equals(loginDto.Email, StringComparison.OrdinalIgnoreCase));
            
            if (user == null)
            {
                Console.WriteLine($"❌ No user found with email: {loginDto.Email}");
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            Console.WriteLine($"✅ User found: {user.Email}, Role: {user.Role}");
            
            // Debug hash-urile
            var inputHash = HashPassword(loginDto.Password);
            Console.WriteLine($"Input password hash: {inputHash}");
            Console.WriteLine($"Stored password hash: {user.Password}");
            Console.WriteLine($"Hashes match: {inputHash == user.Password}");
            
            // Verifică parola hash-uită
            if (!VerifyPassword(loginDto.Password, user.Password))
            {
                Console.WriteLine("❌ Password verification failed");
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            Console.WriteLine("✅ Login successful!");
            return new LoginResponseDto
            {
                Role = user.Role
            };
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("❌ Authentication failed");
            throw; // Re-throw authentication errors
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Login error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw new InvalidOperationException("Database connection error. Please try again later.", ex);
        }
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            Console.WriteLine($"=== REGISTER ATTEMPT ===");
            Console.WriteLine($"Email: {registerDto.Email}");
            Console.WriteLine($"Password length: {registerDto.Password?.Length ?? 0}");
            
            // Verifică dacă email-ul există deja
            var existingUsers = await userRepository.GetAllAsync();
            var existingUser = existingUsers.FirstOrDefault(u => u.Email.Equals(registerDto.Email, StringComparison.OrdinalIgnoreCase));
            
            if (existingUser != null)
            {
                Console.WriteLine($"❌ Email already exists: {registerDto.Email}");
                return new RegisterResponseDto
                {
                    Success = false,
                    Message = "An account with this email already exists"
                };
            }

            // Creează noul utilizator cu parola hash-uită
            var hashedPassword = HashPassword(registerDto.Password);
            Console.WriteLine($"Generated password hash: {hashedPassword}");
            
            var newUser = new User
            {
                Email = registerDto.Email,
                Password = hashedPassword,
                Role = (int)Role.User // Utilizatorii noi sunt User by default
            };

            Console.WriteLine($"Creating new user: {newUser.Email}");
            
            await userRepository.AddAsync(newUser);
            
            Console.WriteLine($"✅ User created successfully with ID: {newUser.Id}");
            
            return new RegisterResponseDto
            {
                Success = true,
                Message = "Account created successfully",
                UserId = newUser.Id
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Registration error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return new RegisterResponseDto
            {
                Success = false,
                Message = "An error occurred while creating the account"
            };
        }
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        var hashedInput = HashPassword(password);
        return hashedInput == hashedPassword;
    }

    public async Task<IList<User>> GetAllUsersAsync()
    {
        try
        {
            var users = await userRepository.GetAllAsync();
            return users.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting all users: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateUserRoleAsync(int userId, Role newRole)
    {
        try
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Role = (int)newRole;
            await userRepository.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user role: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteUserAsync(int userId)
    {
        try
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Don't allow deleting administrators
            if (user.Role == (int)Role.Administrator)
            {
                throw new Exception("Cannot delete administrator users");
            }

            await userRepository.DeleteAsync(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting user: {ex.Message}");
            throw;
        }
    }
}
