using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi103.Helpers;
using WebApi103.Services.Interfaces;
namespace WebApi103.Services.Services
{

    //Tdodas las funcionalidades para la base de datos
    public class UsuarioService : IUsuarioService

    {
        private readonly ApplicationDbContext _context;
        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;

        }

        //Lista de usuarios 
        public async Task<Response<List<Usuario>>> ObtenerUsuario()
        {
            try
            {
                List<Usuario> response = await _context.Usuarios.ToListAsync();

                return new Response<List<Usuario>>(response);

            }
            catch (Exception ex)
            {
                throw new Exception("Sucedio un error" + ex.Message);

            }

        }

        public async Task<Response<Usuario>> ById(int id)
        {
            try
            {
                Usuario result = await _context.Usuarios.FirstOrDefaultAsync(x => x.PkUsuario == id);

                return new Response<Usuario>(result);
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public async Task<Response<Usuario>> Create(Usuario request)
        {
            try
            {
                Usuario newUsers = new Usuario()
                {
                    Nombre = request.Nombre,
                    UserName = request.UserName,
                    Password = request.Password,
                    FkRol = request.FkRol
                };

                var result = await _context.Usuarios.AddAsync(newUsers);
                _context.SaveChanges();

                return new Response<Usuario>(newUsers, "Se creo correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception("Sucedió un error al crear el usuario: " + ex.Message);
            }

        }

        public async Task<Response<Usuario>> Delete(int id)
        {
            try
            {
                Usuario usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.PkUsuario == id);

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return new Response<Usuario>(usuario, "Se eliminó correctamente :D");
            }
            catch (Exception ex)
            {
                throw new Exception("Sucedió un error al eliminar el usuario: " + ex.Message);
            }
        }

        public async Task<Response<Usuario>> Update(int id, Usuario request)
        {
            try
            {
                var usuarioExistente = await _context.Usuarios
                    .Include(x => x.Roles)
                    .FirstOrDefaultAsync(x => x.PkUsuario == id);

                if (usuarioExistente == null)
                {
                    return new Response<Usuario>(null, "El usuario no existe.");
                }

                usuarioExistente.Nombre = request.Nombre;
                usuarioExistente.UserName = request.UserName;
                usuarioExistente.Password = request.Password;
                usuarioExistente.FkRol = request.FkRol;

                await _context.SaveChangesAsync();

                return new Response<Usuario>(usuarioExistente, "Usuario actualizado correctamente.");
            }
            catch (Exception ex)
            {
                throw new Exception("Sucedió un error al actualizar: " + ex.Message);
            }
        }
    }
}