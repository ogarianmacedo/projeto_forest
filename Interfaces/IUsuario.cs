using System.Threading.Tasks;
using ProjetoForest.Models;

namespace ProjetoForest.Interfaces
{
    public interface IUsuario : IGenerico<User>
    {
        Task<User[]> GetAllUsers();

        Task<User> GetUserById(int id);
        
        Task<User[]> GetUserByName(string name);
    }
}