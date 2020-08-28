using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjetoForest.Interfaces;
using ProjetoForest.Models;

namespace ProjetoForest.Repositories
{
    public class UsuarioRepository : GenericoRepository<User>, IUsuario
    {
        private readonly Contexto _contexto;

        public UsuarioRepository(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public async Task<User[]> GetAllUsers()
        {
            IQueryable<User> query = _contexto.Users;
            return await query.ToArrayAsync();
        }

        public Task<User> GetUserById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<User[]> GetUserByName(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}