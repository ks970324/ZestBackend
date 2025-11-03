using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


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
        [Column("id")]
        public int Id { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("password_hash")]
        public string Password_Hash { get; set; }
        [Column("image")]
        public string Image { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set;} = DateTime.UtcNow;
        [Column("is_active")]
        public Boolean Is_Active { get; set; }
        [Column("last_login")]
        public DateTime Last_Login { get; set; } = DateTime.UtcNow;
        
    }

}