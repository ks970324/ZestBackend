namespace ZestApi.Repositories;
using Microsoft.EntityFrameworkCore;

using ZestApi.Data;

public interface IUserRepository
{
    UserInfo? GetUserByEmailAndPassword(string email, string password);
    string? GetUserImageByEmail(string email);
    Task<bool> EmailExistsAsync(string email);
    Task AddUserAsync(UserInfo user);
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public UserInfo? GetUserByEmailAndPassword(string email, string password)
    {
        return _db.userinfo
            .FirstOrDefault(u => u.Email == email && u.Password_hash == password);
    }

    public string? GetUserImageByEmail(string email)
    {
        return _db.userinfo
            .Where(u => u.Email == email)
            .Select(u => u.Image)
            .FirstOrDefault();
    } 
    
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _db.userinfo.AnyAsync(u => u.Email == email);
    }

    public async Task AddUserAsync(UserInfo user)
    {
        await _db.userinfo.AddAsync(user);
        await _db.SaveChangesAsync();
    }
    
    
}