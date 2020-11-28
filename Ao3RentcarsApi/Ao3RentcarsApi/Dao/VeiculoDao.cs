using Ao3RentcarsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ao3RentcarsApi.Dao
{
    /// <summary>
    /// Implementa o acesso a dados da entidade Veiculo
    /// </summary>
    public class VeiculoDao
    {
        private readonly RentcarsContext _context;

        public VeiculoDao(RentcarsContext context)
        {
            _context = context;
        }

        #region ========== OPERAÇÕES BÁSICAS ==========

        public bool Existe(int id)
        {
            return _context.Veiculos.Any(e => e.Id == id);
        }

        public async Task<Veiculo> Busca(int id)
        {
            return await _context.Veiculos.FindAsync(id);
        }

        public async Task<List<Veiculo>> ListaTodos()
        {
            return await _context.Veiculos.ToListAsync();
        }

        public async Task<int> Altera(Veiculo veiculo)
        {
            _context.Entry(veiculo).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Insere(Veiculo veiculo)
        {
            _context.Veiculos.Add(veiculo);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Exclui(Veiculo veiculo)
        {
            _context.Veiculos.Remove(veiculo);
            return await _context.SaveChangesAsync();
        }

        #endregion ========== OPERAÇÕES BÁSICAS ==========

        #region ========== OPERAÇÕES ESPECÍFICAS DA ENTIDADE ==========

        public bool PlacaJaCadastrada(Veiculo veiculo)
        {
            return !string.IsNullOrEmpty(
                _context.Veiculos
                .Where(v => v.Id != veiculo.Id && v.Placa == veiculo.Placa)
                .Select(v => v.Placa)
                .FirstOrDefault());
        }

        public Veiculo BuscaPorPlaca(string placa)
        {
            return _context.Veiculos
                    .Where(v => v.Placa == placa)
                    .FirstOrDefault();
        }

        public bool EstaLocado(Veiculo veiculo)
        {
            return EstaLocado(veiculo.Id);
        }

        public bool EstaLocado(string placa)
        {

            int id = _context.Veiculos
                    .Where(v => v.Placa == placa)
                    .Select(v => v.Id)
                    .FirstOrDefault();

            if (id > 0)
            {
                return EstaLocado(id);
            }

            return false;
            
        }

        public bool EstaLocado(int id)
        {
            return (_context.Locacoes
                    .Where(l => l.IdVeiculo == id && l.DataFim == null)
                    .Select(l => l.Id)
                    .FirstOrDefault() > 0);
        }

        #endregion ========== OPERAÇÕES ESPECÍFICAS DA ENTIDADE ==========

    }
}
