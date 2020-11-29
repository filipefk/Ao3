using Ao3RentcarsApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ao3RentcarsApi.Dao
{
    public class UsuarioDao
    {
        private readonly RentcarsContext _context;

        public UsuarioDao(RentcarsContext context)
        {
            _context = context;
        }

        #region ========== OPERAÇÕES BÁSICAS ==========

        public bool Existe(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        public async Task<Usuario> Busca(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<List<Usuario>> ListaTodos()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<int> Altera(Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Insere(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Exclui(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            return await _context.SaveChangesAsync();
        }

        #endregion ========== OPERAÇÕES BÁSICAS ==========

        #region ========== OPERAÇÕES ESPECÍFICAS DA ENTIDADE ==========

        public async Task<Usuario> BuscaPorLogin(string login)
        {
            int id = _context.Usuarios
                .Where(u => u.Login == login).Select(u => u.Id)
                .FirstOrDefault();

            if (id > 0)
            {
                return await _context.Usuarios.FindAsync(id);
            }

            return null;
            
        }

        #endregion  ========== OPERAÇÕES ESPECÍFICAS DA ENTIDADE ==========
    }
}
