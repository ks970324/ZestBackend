using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ZestApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserInfo> userinfo { get; set; }
    }
    
    public class UserInfo
    {
        [Key]
        public string email { get; set; }
        public string password_hash { get; set; }
    }

}