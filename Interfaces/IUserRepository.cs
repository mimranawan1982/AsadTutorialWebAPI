using AsadTutorialWebAPI.Models;

namespace AsadTutorialWebAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string username, string password);

        void Register(string username, string password);

        Task<bool> UserAlreadyExist(string username);
    }
}
