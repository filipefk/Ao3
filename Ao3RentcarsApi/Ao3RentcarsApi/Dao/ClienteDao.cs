using Ao3RentcarsApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ao3RentcarsApi.Helpers;

namespace Ao3RentcarsApi.Dao
{
    public class ClienteDao
    {
        private readonly RentcarsContext _context;

        public ClienteDao(RentcarsContext context)
        {
            _context = context;
        }

        #region ========== OPERAÇÕES BÁSICAS ==========

        public bool Existe(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }

        public async Task<Cliente> Busca(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<List<Cliente>> ListaTodos()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<int> Altera(Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Insere(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Exclui(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
            return await _context.SaveChangesAsync();
        }

        #endregion ========== OPERAÇÕES BÁSICAS ==========

        #region ========== OPERAÇÕES ESPECÍFICAS DA ENTIDADE ==========

        public Cliente BuscaPorCpf(string cpf)
        {
            cpf = Validador.SoNumeros(cpf);
            return _context.Clientes
                    .Where(c => c.Cpf == cpf)
                    .FirstOrDefault();
        }

        public bool CpfJaCadastrado(Cliente cliente)
        {
            return (_context.Clientes
                    .Where(c => c.Cpf == cliente.Cpf)
                    .Select(c => c.Id)
                    .FirstOrDefault() > 0);
        }

        public bool CpfJaCadastrado(string cpf)
        {
            return (_context.Clientes
                    .Where(c => c.Cpf == cpf)
                    .Select(c => c.Id)
                    .FirstOrDefault() > 0);
        }

        public string NomeDoCpf(string cpf)
        {
            return _context.Clientes
                    .Where(c => c.Cpf == cpf)
                    .Select(c => c.Nome)
                    .FirstOrDefault();
        }

        #endregion ========== OPERAÇÕES ESPECÍFICAS DA ENTIDADE ==========
    }
}
