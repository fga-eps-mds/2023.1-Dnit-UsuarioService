using api;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;

namespace app.Entidades
{
    public class AppDbContext : DbContext
    {
        public DbSet<Municipio> Municipio { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<RedefinicaoSenha> RedefinicaoSenha { get; set; }
        public DbSet<Empresa> Empresa { get; set; }

        public DbSet<Perfil> Perfis { get; set; }
        public DbSet<PerfilPermissao> PerfilPermissoes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
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

        public void Popula()
        {
            PopulaMunicipiosPorArquivo(null, Path.Join(".", "Migrations", "Data", "municipios.csv"));
        }

        public List<Municipio>? PopulaMunicipiosPorArquivo(int? limit, string caminho)
        {
            var hasMunicipio = Municipio.Any();
            var municipios = new List<Municipio>();

            if (hasMunicipio)
            {
                return null;
            }

            using (var fs = File.OpenRead(caminho))
            using (var parser = new TextFieldParser(fs))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                var columns = new Dictionary<string, int> { { "id", 0 }, { "name", 1 }, { "uf", 2 } };

                while (!parser.EndOfData)
                {
                    var row = parser.ReadFields()!;
                    var municipio = new Municipio
                    {
                        Id = int.Parse(row[columns["id"]]),
                        Nome = row[columns["name"]],
                        Uf = (UF)int.Parse(row[columns["uf"]]),
                    };

                    municipios.Add(municipio);
                    if (limit.HasValue && municipios.Count >= limit.Value)
                    {
                        break;
                    }
                }
            }
            AddRange(municipios);
            SaveChanges();
            return municipios;
        }
    }
}