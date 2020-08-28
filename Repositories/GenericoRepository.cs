using System.Threading.Tasks;
using ProjetoForest.Interfaces;
using ProjetoForest.Models;

namespace ProjetoForest.Repositories
{
    public class GenericoRepository<TEntity> : IGenerico<TEntity> where TEntity : class
    {
        private readonly Contexto _contexto;

        public GenericoRepository(Contexto contexto)
        {
            _contexto = contexto;
        }

        public void Add(TEntity entity)
        {
            _contexto.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _contexto.Update(entity);
        }

        public void Delete(TEntity entity)
        {
           _contexto.Remove(entity);
        }

        public void DeleteRanger<T>(T[] entityArray) where T : class
        {
            _contexto.RemoveRange(entityArray);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _contexto.SaveChangesAsync()) > 0;
        }
    }
}