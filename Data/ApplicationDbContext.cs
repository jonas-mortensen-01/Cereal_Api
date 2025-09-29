using Cereal_Api.Models;
using Cereal_Api.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Cereal_Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CerealDTO> CerealTable { get; set; }
    }
}