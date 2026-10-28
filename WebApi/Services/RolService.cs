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
    public class RolService : IRolService
    {
        private readonly ApplicationDbContext _context;
        public RolService(ApplicationDbContext context) => _context = context;

        public async Task<Response<List<Rol>>> ObtenerRoles()
        {
            var rows = await _context.Database.GetDbConnection().QueryAsync<Rol>(
                "dbo.spGetRoles", commandType: CommandType.StoredProcedure);
            return new Response<List<Rol>>(rows.ToList());
        }

        public Task<Response<Rol>> ById(int id) =>
            Execute("dbo.usp_Roles_ObtenerPorId", new { Id = id });

        public Task<Response<Rol>> Delete(int id) =>
            Execute("dbo.usp_Roles_Eliminar", new { Id = id }, "Se eliminó correctamente.");

        public Task<Response<Rol>> Create(Rol request)
        {
            
            return Execute("dbo.usp_Roles_Crear",
                new { request.Nombre, request.Estatus }, "Se creó correctamente.");
        }

        public Task<Response<Rol>> Update(int id, Rol request)
        {
            
            return Execute("dbo.usp_Roles_Actualizar",
                new { Id = id, request.Nombre, request.Estatus }, "Se actualizó correctamente.");
        }

        private async Task<Response<Rol>> Execute(string procedure, object parameters, string message = "")
        {
            try
            {
                var row = await _context.Database.GetDbConnection().QuerySingleOrDefaultAsync<Rol>(
                    procedure, parameters, commandType: CommandType.StoredProcedure);
                return row == null
                    ? new Response<Rol>("El rol no existe.")
                    : new Response<Rol>(row, message);
            }
            catch (SqlException ex) when (ex.Number == 50002)
            {
                return new Response<Rol>("No se puede eliminar un rol asignado a usuarios.", "role_in_use");
            }
        }
    }
}
