using Microsoft.EntityFrameworkCore;

namespace app.Entidades
{
    public class AppDbContext : DbContext
    {

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<RedefinicaoSenha> RedefinicaoSenha { get; set; }
        public DbSet<Empresa> Empresa { get; set; }

        public DbSet<Perfil> Perfis { get; set; }
        public DbSet<PerfilPermissao> PerfilPermissoes { get; set; }

        public AppDbContext (DbContextOptions<AppDbContext> options) : base (options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
            
            modelBuilder.Entity<RedefinicaoSenha>()
                .Property(r => r.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<RedefinicaoSenha>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.RedefinicaoSenha)
                .HasForeignKey(r => r.IdUsuario);

            modelBuilder.Entity<Empresa>()
                .HasIndex(e => e.Cnpj)
                .IsUnique();

            modelBuilder.Entity<Empresa>()
                .HasMany(e => e.Usuarios)
                .WithMany(u => u.Empresas)
                .UsingEntity(em => 
                {
                    em.Property<int>("UsuariosId").HasColumnName("IdUsuario");
                    em.Property<string>("EmpresasCnpj").HasColumnName("CnpjEmpresa");
                    em.ToTable("UsuarioEmpresa");
                });

            modelBuilder.Entity<Perfil>()
                .HasIndex(p => p.Nome)
                .IsUnique();

            modelBuilder.Entity<Perfil>()
                .HasMany(p => p.PerfilPermissoes)
                .WithOne(pp => pp.Perfil)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Perfil)
                .WithMany(p => p.Usuarios)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}