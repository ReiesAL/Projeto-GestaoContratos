using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projeto_GestaoContratos.Models;

namespace Projeto_GestaoContratos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contratos> Contratos { get; set; } = default!;
        public DbSet<LogUsuarios> LogUsuarios { get; set; }

        }
}
