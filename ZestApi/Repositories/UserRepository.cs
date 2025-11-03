namespace ZestApi.Repositories;
using Microsoft.EntityFrameworkCore;

using ZestApi.Data;

public interface IUserRepository
{
    Task<UserInfo> GetUserByEmailAsync(string email);
    Task<string?> GetUserImageByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task AddUserAsync(UserInfo user);
    Task UpdateUserAsync(UserInfo user);
    
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    
    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserInfo> GetUserByEmailAsync(string email)
    {
        return await _db.userinfo.FirstOrDefaultAsync(u => u.Email == email);
    }
        
    

    public async Task<string?> GetUserImageByEmailAsync(string email)
    {
        return await _db.userinfo
            .Where(u => u.Email == email)
            .Select(u => u.Image)
            .FirstOrDefaultAsync(); 
    }
    
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _db.userinfo.AnyAsync(u => u.Email == email);
    }

    public async Task AddUserAsync(UserInfo user)
    {
        await _db.userinfo.AddAsync(user);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine(ex.InnerException?.Message);
            throw;
        }
    }
    
    public async Task UpdateUserAsync(UserInfo user)
    {
        user.Last_Login = user.Last_Login.ToUniversalTime();
        user.CreatedAt = user.CreatedAt.ToUniversalTime();
        _db.userinfo.Update(user);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine(ex.InnerException?.Message);
        }
    }
    
    
}