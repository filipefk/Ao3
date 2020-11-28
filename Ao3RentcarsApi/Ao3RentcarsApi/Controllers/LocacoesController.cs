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
using Ao3RentcarsApi.Dao;
using Microsoft.AspNetCore.Authorization;

namespace Ao3RentcarsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocacoesController : ControllerBase
    {
        private readonly LocacaoDao _dao;

        public LocacoesController(RentcarsContext context)
        {
            _dao = new LocacaoDao(context);
        }

        // GET: api/Locacoes
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LocacaoDto>>> ListaTodos()
        {
            try
            {
                return LocacaoDto.ToDtoList(await _dao.ListaTodos());
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
        [Authorize]
        public async Task<ActionResult<LocacaoDto>> Busca(int id)
        {
            try
            {
                Locacao locacao = await _dao.Busca(id);

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
        [Authorize]
        public async Task<IActionResult> Altera(int id, LocacaoDto locacaoDto)
        {
            try
            {
                Locacao locacao = await _dao.Busca(id);

                if (locacao == null)
                {
                    return NotFound();
                }

                locacao = LocacaoDto.PutEntity(locacao, locacaoDto);
                locacao.DataAlteracao = DateTime.Now;
                Valida(locacao);

                await _dao.Altera(locacao);

                locacaoDto = LocacaoDto.ToDto(locacao);
                return CreatedAtAction(nameof(Altera), new { id = locacaoDto.Id }, locacaoDto);
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
        [Authorize]
        public async Task<ActionResult<LocacaoDto>> Insere(LocacaoDto locacaoDto)
        {
            try
            {
                Locacao locacao = LocacaoDto.ToEntity(locacaoDto);
                locacao.Id = 0;
                locacao.DataInclusao = DateTime.Now;
                locacao.DataAlteracao = DateTime.Now;
                Valida(locacao);
                await _dao.Insere(locacao);

                locacaoDto = LocacaoDto.ToDto(locacao);
                return CreatedAtAction(nameof(Insere), new { id = locacaoDto.Id }, locacaoDto);
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
        [Authorize]
        public async Task<IActionResult> Exclui(int id)
        {
            try
            {
                Locacao locacao = await _dao.Busca(id);
                if (locacao == null)
                {
                    return NotFound();
                }

                await _dao.Exclui(locacao);

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

        private void Valida(Locacao locacao)
        {
            if (locacao.IdUsuario == 0 || locacao.IdVeiculo == 0)
            {
                throw new ArgumentException("Impossível salvar uma locação sem informar o IdUsuario e o IdVeiculo");
            }
            if (locacao.DataInicio <= DateTime.MinValue)
            {
                throw new ArgumentException("Data de início da locação inválida");
            }
            if (locacao.DataFimPrevisto <= DateTime.MinValue)
            {
                throw new ArgumentException("Data de fim previsto da locação inválida");
            }
            if (locacao.DataFim != null && locacao.DataFim <= DateTime.MinValue)
            {
                throw new ArgumentException("Data de fim da locação inválida");
            }
            if (locacao.Id == 0)
            {
                // É uma locação nova. Verificando se o carro não está locado
                if (_dao.VeiculoLocado(locacao.IdVeiculo))
                {
                    throw new ArgumentException("O veículo selecionado já está está locado");
                }
            }
            else
            {
                if (_dao.Encerrada(locacao.Id) && locacao.DataFim == null)
                {
                    // Está tentando reverter o encerramento da locação (DataFim = null). Verificando se o carro não foi locado
                    if (_dao.VeiculoLocado(locacao.IdVeiculo))
                    {
                        throw new ArgumentException("Impossível reverter a locação para ativa pois o veículo selecionado já foi locado novamente");
                    }
                }
            }
            
        }

    }
}
