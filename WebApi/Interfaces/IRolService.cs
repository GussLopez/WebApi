using Domain.Entities;
using WebApi103.Helpers;

namespace WebApi103.Services.Interfaces
{
    public interface IRolService
    {
        Task<Response<List<Rol>>> ObtenerRoles();

        Task<Response<Rol>> ById(int id);

        Task<Response<Rol>> Create(Rol request);

        Task<Response<Rol>> Delete(int id);

        Task<Response<Rol>> Update(int id, Rol request);
    }
}
