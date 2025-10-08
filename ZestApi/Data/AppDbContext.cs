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
        public string Email { get; set; }
        public string Password_hash { get; set; }
        public string Image { get; set; }
    }

}