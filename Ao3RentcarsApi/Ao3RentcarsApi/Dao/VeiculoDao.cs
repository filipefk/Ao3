﻿using Ao3RentcarsApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ao3RentcarsApi.Models.Dto;
using System;

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

        public async Task<List<Veiculo>> ListaDisponiveis()
        {
            return await _context
                .Veiculos
                .FromSqlRaw("SELECT * FROM Veiculo WHERE id NOT IN (SELECT IdVeiculo FROM Locacao WHERE DataFim IS NULL)")
                .ToListAsync();
        }

        public List<VeiculoDto> ListaLocados()
        {
            var query = _context.Veiculos
                    .Join(
                        _context.Locacoes,
                        veiculos => veiculos.Id,
                        locacoes => locacoes.Veiculo.Id,
                        (veiculos, locacoes) => new
                        {
                            veiculos.Id,
                            veiculos.DataInclusao,
                            veiculos.DataAlteracao,
                            veiculos.Modelo,
                            veiculos.Marca,
                            veiculos.Placa,
                            veiculos.AnoModelo,
                            veiculos.AnoFabricacao,
                            IdLocacao = locacoes.Id,
                            locacoes.DataFim
                        })
                    .Where(v => v.DataFim == null)
                    .ToList();

            IEnumerable<VeiculoDto> veiculosDto = from v in query
                                                  select new VeiculoDto()
                                                  {
                                                      Id = v.Id,
                                                      DataInclusao = v.DataInclusao,
                                                      DataAlteracao = v.DataAlteracao,
                                                      Modelo = v.Modelo,
                                                      Marca = v.Marca,
                                                      Placa = v.Placa,
                                                      AnoModelo = v.AnoModelo,
                                                      AnoFabricacao = v.AnoFabricacao,
                                                      IdLocacao = v.IdLocacao
                                                  };

            return veiculosDto.ToList();
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

        public async Task<Locacao> BuscaLocacao(int idVeiculo)
        {
            return await _context
                .Locacoes
                .Where(l => l.IdVeiculo == idVeiculo && l.DataFim == null)
                .FirstOrDefaultAsync();
        }

        public async Task<int> AlteraLocacao(Locacao locacao)
        {
            _context.Entry(locacao).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        #endregion ========== OPERAÇÕES ESPECÍFICAS DA ENTIDADE ==========

    }
}
