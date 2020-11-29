using Ao3RentcarsApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ao3RentcarsApi.Models.Dto;
using System;
using Ao3RentcarsApi.Helpers;

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

        internal Locacao ValidaLocacaoCliente(LocacaoClienteDto locacaoClienteDto)
        {
            Locacao locacao = null;
            locacaoClienteDto.Cpf = Validador.SoNumeros(locacaoClienteDto.Cpf);
            if (!string.IsNullOrEmpty(locacaoClienteDto.Nome) && Validador.CpfValido(locacaoClienteDto.Cpf))
            {
                ClienteDao clienteDao = new ClienteDao(_context);
                Cliente cliente = clienteDao.BuscaPorCpf(locacaoClienteDto.Cpf);
                if (cliente == null)
                {
                    cliente = new Cliente();
                    cliente.Id = 0;
                    cliente.DataInclusao = DateTime.Now;
                    cliente.DataAlteracao = DateTime.Now;
                    cliente.Nome = locacaoClienteDto.Nome;
                    cliente.Cpf = locacaoClienteDto.Cpf;
                    clienteDao.Insere(cliente);
                }
                locacao = new Locacao()
                {
                    IdCliente = cliente.Id,
                    IdUsuario = locacaoClienteDto.IdUsuario > 0 ? locacaoClienteDto.IdUsuario : 1,
                    DataInicio = (DateTime)locacaoClienteDto.DataInicio,
                    DataFimPrevisto = (DateTime)locacaoClienteDto.DataFimPrevisto,
                    IdVeiculo = locacaoClienteDto.IdVeiculo
                };
                
            }

            return locacao;
        }

        #endregion ========== OPERAÇÕES ESPECÍFICAS DA ENTIDADE ==========
    }
}
