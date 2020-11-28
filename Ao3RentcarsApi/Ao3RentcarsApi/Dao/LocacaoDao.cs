using Ao3RentcarsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Ao3RentcarsApi.Dao
{
    public class LocacaoDao
    {
        private readonly RentcarsContext _context;

        public LocacaoDao(RentcarsContext context)
        {
            _context = context;
        }

        #region ========== OPERAÇÕES BÁSICAS ==========

        public bool Existe(int id)
        {
            return _context.Locacoes.Any(e => e.Id == id);
        }

        public async Task<Locacao> Busca(int id)
        {
            return await _context.Locacoes.FindAsync(id);
        }

        public async Task<List<Locacao>> ListaTodos()
        {
            return await _context.Locacoes.ToListAsync();
        }

        public async Task<int> Altera(Locacao locacao)
        {
            _context.Entry(locacao).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Insere(Locacao locacao)
        {
            _context.Locacoes.Add(locacao);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Exclui(Locacao locacao)
        {
            _context.Locacoes.Remove(locacao);
            return await _context.SaveChangesAsync();
        }

        #endregion ========== OPERAÇÕES BÁSICAS ==========


        #region ========== OPERAÇÕES ESPECÍFICAS DA ENTIDADE ==========

        public bool VeiculoLocado(int idVeiculo)
        {
            return (_context.Locacoes
                .Where(l => l.IdVeiculo == idVeiculo && l.DataFim == null)
                .Select(l => l.IdVeiculo)
                .FirstOrDefault() > 0);
            
        }

        public bool Encerrada(Locacao locacao)
        {
            return Encerrada(locacao.Id);
        }

        public bool Encerrada(int id)
        {
            return (_context.Locacoes.Where(l => l.Id == id).Select(l => l.DataFim).FirstOrDefault() != null);
        }

        #endregion ========== OPERAÇÕES ESPECÍFICAS DA ENTIDADE ==========
    }
}
