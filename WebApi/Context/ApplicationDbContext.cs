using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        } 

        // Modelos a Mapear

        public DbSet<Usuario> Usuarios { get; set;  }
        public DbSet<Rol> Roles { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Insertar en la tabla de Usuario

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    PkUsuario = 1,
                    Nombre = "Gustavo",
                    UserName = "gus",
                    Password = "Password1.",
                    FkRol = 1
                }
                );

            // Insertar en la tabla de roles

            modelBuilder.Entity<Rol>().HasData(
                new Rol
                {
                    PkRol = 1,
                    Nombre = "sa",
                    Estatus = true
                },
                new Rol
                {
                    PkRol = 2,
                    Nombre = "admin",
                    Estatus = false
                }


                );
        }
    }

}
