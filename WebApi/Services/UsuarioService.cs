using Dapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApi.Context;
using WebApi103.Helpers;
using WebApi103.Services.Interfaces;

namespace WebApi103.Services.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;
        public UsuarioService(ApplicationDbContext context) => _context = context;

        public async Task<Response<List<Usuario>>> ObtenerUsuario()
        {
            var rows = await _context.Database.GetDbConnection().QueryAsync<Usuario>(
                "dbo.spGetUsers", commandType: CommandType.StoredProcedure);
            return new Response<List<Usuario>>(rows.ToList());
        }

        public Task<Response<Usuario>> ById(int id) =>
            Execute("dbo.usp_Usuarios_ObtenerPorId", new { Id = id });

        public Task<Response<Usuario>> Delete(int id) =>
            Execute("dbo.usp_Usuarios_Eliminar", new { Id = id }, "Se eliminó correctamente.");

        public Task<Response<Usuario>> Create(Usuario request)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
                return Task.FromResult(new Response<Usuario>("La contraseña es obligatoria."));
            var hash = new PasswordHasher<Usuario>().HashPassword(request, request.Password);
            return Execute("dbo.usp_Usuarios_Crear",
                new { request.Nombre, request.UserName, PasswordHash = hash, request.FkRol }, "Se creó correctamente.");
        }

        public Task<Response<Usuario>> Update(int id, Usuario request)
        {
            var hash = string.IsNullOrWhiteSpace(request.Password)
                ? null : new PasswordHasher<Usuario>().HashPassword(request, request.Password);
            return Execute("dbo.usp_Usuarios_Actualizar",
                new
                {
                    Id = id,
                    request.Nombre,
                    request.UserName,
                    PasswordHash = hash,
                    request.FkRol
                }, 
                "Se actualizó correctamente.");

        }

        private async Task<Response<Usuario>> Execute(string procedure, object parameters, string message = "")
        {
            try
            {
                var row = await _context.Database.GetDbConnection().QuerySingleOrDefaultAsync<Usuario>(
                    procedure, parameters, commandType: CommandType.StoredProcedure);
                return row == null
                    ? new Response<Usuario>("El usuario no existe.")
                    : new Response<Usuario>(row, message);
            }
            catch (SqlException ex) when (ex.Number == 50001)
            {
                return new Response<Usuario>("El rol indicado no existe. Consulta GET /Roles.", "invalid_role");
            }
        }
    }
}
