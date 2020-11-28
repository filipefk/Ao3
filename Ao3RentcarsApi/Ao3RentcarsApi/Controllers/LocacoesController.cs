using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ao3RentcarsApi.Models;
using Ao3RentcarsApi.Models.Dto;
using Microsoft.Data.Sqlite;

namespace Ao3RentcarsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocacoesController : ControllerBase
    {
        private readonly RentcarsContext _context;

        public LocacoesController(RentcarsContext context)
        {
            _context = context;
        }

        // GET: api/Locacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocacaoDto>>> GetLocacoes()
        {
            try
            {
                return LocacaoDto.ToDtoList(await _context.Locacoes.ToListAsync());
            }
            catch (SqliteException sqlLex)
            {
                string msg = sqlLex.Message;
                if (sqlLex.InnerException != null)
                {
                    msg += " - " + sqlLex.InnerException.Message;
                }
                throw new ArgumentException("Problema de acesso ao banco de dados - " + msg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += " - " + ex.InnerException.Message;
                }
                throw new ArgumentException("Erro ao tentar buscar a lista de locações - " + msg);
            }
        }

        // GET: api/Locacoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocacaoDto>> GetLocacao(int id)
        {
            try
            {
                Locacao locacao = await LocacaoById(id);

                if (locacao == null)
                {
                    return NotFound();
                }

                LocacaoDto locacaoDto = LocacaoDto.ToDto(locacao);

                return locacaoDto;
            }
            catch (SqliteException sqlLex)
            {
                string msg = sqlLex.Message;
                if (sqlLex.InnerException != null)
                {
                    msg += " - " + sqlLex.InnerException.Message;
                }
                throw new ArgumentException("Problema de acesso ao banco de dados - " + msg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += " - " + ex.InnerException.Message;
                }
                throw new ArgumentException("Erro ao tentar buscar a locação de id = " + id + " - " + msg);
            }
        }

        // PUT: api/Locacoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocacao(int id, LocacaoDto locacaoDto)
        {
            try
            {
                Locacao locacao = await LocacaoById(id);

                if (locacao == null)
                {
                    return NotFound();
                }

                locacao = LocacaoDto.PutEntity(locacao, locacaoDto);
                locacao.DataAlteracao = DateTime.Now;
                ValidaLocacao(locacao);

                _context.Entry(locacao).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException dbUex)
            {
                string msg = dbUex.Message;
                if (dbUex.InnerException != null)
                {
                    msg += " - " + dbUex.InnerException.Message;
                }
                throw new ArgumentException("Erro ao tentar salvar no banco - " + msg);
            }
            catch (SqliteException sqlLex)
            {
                string msg = sqlLex.Message;
                if (sqlLex.InnerException != null)
                {
                    msg += " - " + sqlLex.InnerException.Message;
                }
                throw new ArgumentException("Problema de acesso ao banco de dados - " + msg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += " - " + ex.InnerException.Message;
                }
                throw new ArgumentException("Erro ao tentar alterar a locação de id = " + id + " - " + msg);
            }
        }

        // POST: api/Locacoes
        [HttpPost]
        public async Task<ActionResult<LocacaoDto>> PostLocacao(LocacaoDto locacaoDto)
        {
            try
            {
                Locacao locacao = LocacaoDto.ToEntity(locacaoDto);
                locacao.Id = 0;
                locacao.DataInclusao = DateTime.Now;
                locacao.DataAlteracao = DateTime.Now;
                ValidaLocacao(locacao);
                _context.Locacoes.Add(locacao);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLocacao), new { id = locacao.Id }, locacao);
            }
            catch (DbUpdateException dbUex)
            {
                string msg = dbUex.Message;
                if (dbUex.InnerException != null)
                {
                    msg += " - " + dbUex.InnerException.Message;
                }
                throw new ArgumentException("Erro ao tentar salvar no banco - " + msg);
            }
            catch (SqliteException sqlLex)
            {
                string msg = sqlLex.Message;
                if (sqlLex.InnerException != null)
                {
                    msg += " - " + sqlLex.InnerException.Message;
                }
                throw new ArgumentException("Problema de acesso ao banco de dados - " + msg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += " - " + ex.InnerException.Message;
                }
                throw new ArgumentException("Erro ao tentar criar uma nova locação - " + msg);
            }
        }

        // DELETE: api/Locacoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocacao(int id)
        {
            try
            {
                Locacao locacao = await LocacaoById(id);
                if (locacao == null)
                {
                    return NotFound();
                }

                _context.Locacoes.Remove(locacao);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (SqliteException sqlLex)
            {
                string msg = sqlLex.Message;
                if (sqlLex.InnerException != null)
                {
                    msg += " - " + sqlLex.InnerException.Message;
                }
                throw new ArgumentException("Problema de acesso ao banco de dados - " + msg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += " - " + ex.InnerException.Message;
                }
                throw new ArgumentException("Erro ao tentar excluir a locação de id = " + id + " - " + msg);
            }
        }

        private async Task<Locacao> LocacaoById(int id)
        {
            return await _context.Locacoes.FindAsync(id);
        }

        private void ValidaLocacao(Locacao locacao)
        {
            if (locacao.IdUsuario == 0 || locacao.IdVeiculo == 0)
            {
                throw new ArgumentException("Impossível salvar uma locação sem informar o IdUsuario e o IdVeiculo");
            }
            if (locacao.DataInicio <= DateTime.MinValue)
            {
                throw new ArgumentException("Data de início da locação inválida");
            }
            if (locacao.DataFim != null && locacao.DataFim <= DateTime.MinValue)
            {
                throw new ArgumentException("Data de fim da locação inválida");
            }
            if (locacao.Id == 0)
            {
                // É uma locação nova. Verificando se o carro não está locado
                int IdVeiculo = _context.Locacoes.Where(l => l.IdVeiculo == locacao.IdVeiculo && l.DataFim == null).Select(l => l.IdVeiculo).FirstOrDefault();
                if (IdVeiculo > 0)
                {
                    string placa = _context.Veiculos.Where(v => v.Id == IdVeiculo).Select(v => v.Placa).FirstOrDefault();
                    throw new ArgumentException("O veículo de placa " + placa + " já está está locado");
                }
            }
            else
            {
                DateTime? DataFimDb = _context.Locacoes.Where(l => l.Id == locacao.Id).Select(l => l.DataFim).FirstOrDefault();
                if (DataFimDb != null && locacao.DataFim == null)
                {
                    // Está tentando voltar a locação para "DataFim == null". Verificando se o carro não foi locado
                    int IdVeiculo = _context.Locacoes.Where(l => l.IdVeiculo == locacao.IdVeiculo && l.DataFim == null).Select(l => l.IdVeiculo).FirstOrDefault();
                    if (IdVeiculo > 0)
                    {
                        string placa = _context.Veiculos.Where(v => v.Id == IdVeiculo).Select(v => v.Placa).FirstOrDefault();
                        throw new ArgumentException("Impossível reverter a locação para ativa pois o veículo de placa " + placa + " já foi locado novamente");
                    }
                }
            }
            
        }

        //private bool LocacaoExists(int id)
        //{
        //    return _context.Locacoes.Any(e => e.Id == id);
        //}
    }
}
