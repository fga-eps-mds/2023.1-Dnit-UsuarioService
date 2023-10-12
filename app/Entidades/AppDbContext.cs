using Microsoft.EntityFrameworkCore;

namespace app.Entidades
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<RedefinicaoSenha> RedefinicaoSenha { get; set; }
        public DbSet<Empresa> Empresa { get; set; }

        public AppDbContext (IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSql"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<RedefinicaoSenha>()
                .Property(r => r.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<RedefinicaoSenha>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.RedefinicaoSenha)
                .HasForeignKey(r => r.IdUsuario);

            modelBuilder.Entity<Empresa>()
                .HasMany(e => e.Usuarios)
                .WithMany(u => u.Empresas)
                .UsingEntity(em => 
                {
                    em.Property<int>("UsuariosId").HasColumnName("IdUsuario");
                    em.Property<string>("EmpresasCnpj").HasColumnName("CnpjEmpresa");
                    em.ToTable("UsuarioEmpresa");
                });
        }
    }
}