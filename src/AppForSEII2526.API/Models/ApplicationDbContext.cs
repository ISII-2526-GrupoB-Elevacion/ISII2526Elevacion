using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Models
{
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){
    
        }
        protected override void OnModelCreating(ModelBuilder builder){
           base.OnModelCreating(builder);
        }
    }
}


