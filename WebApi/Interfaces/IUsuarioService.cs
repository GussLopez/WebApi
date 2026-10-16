using Domain.Entities;
using WebApi103.Helpers;


namespace WebApi103.Services.Interfaces
{
    public interface IUsuarioService
    {

        public Task<Response<List<Usuario>>> ObtenerUsuario();

        public Task<Response<Usuario>> ById(int id);

        public Task<Response<Usuario>> Create(Usuario request);

        public Task<Response<Usuario>> Delete(int id);

        public Task<Response<Usuario>> Update(int id, Usuario request);

    }
}