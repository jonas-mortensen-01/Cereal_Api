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

        public DbSet<ProductDTO> ProductTable { get; set; }
        public DbSet<ProductImageDTO> ProductImageTable { get; set; }
    }
}