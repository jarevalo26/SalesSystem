using SalesSystem.Domain.Entities;
using System.Diagnostics.SymbolStore;

namespace SalesSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<Usuario>> GetAll();
        Task<Usuario> Create(
            Usuario entity, 
            Stream? photo = null, 
            string? photoName = null, 
            string? UrlEmailTemplate = null);
        Task<Usuario> Edit(
            Usuario entity,
            Stream? photo = null,
            string? photoName = null);
        Task<bool> Delete(int UserId);
        Task<Usuario> GetByCredentials(string email, string password);
        Task<Usuario> GetById(int UserId);
        Task<bool> SaveProfile(Usuario entity);
        Task<bool> ChangePassword(int UserId, string Password, string newPassword);
        Task<bool> RestorePassword(string email, string UrlEmailTemplate);
    }
}
