using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ao3RentcarsApi.Models;
using Ao3RentcarsApi.Models.Dto;
using Microsoft.Data.Sqlite;
using Ao3RentcarsApi.Dao;
using Microsoft.AspNetCore.Authorization;

namespace Ao3RentcarsApi.Controllers
{
    /// <summary>
    /// Faz o CRUD das Locações
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LocacoesController : ControllerBase
    {
        private readonly LocacaoDao _dao;

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <remarks>
        /// Recebe o contexto e instancia a classe dao passando o contexto
        /// </remarks>
        /// <param name="context">
        /// RentcarsContext
        /// </param>
        public LocacoesController(RentcarsContext context)
        {
            _dao = new LocacaoDao(context);
        }

        /// <summary>
        /// Rota GET: api/Locacoes
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". o token é obtido na rota api/Login <br/>
        /// Se estiver usando o Swagger tem um botão no topo, a direita escrito "Authorize", clique nele e preencha com a palavra "Bearer", um espaço e depois o token
        /// </remarks>
        /// <returns>
        /// Retorna uma lista de todas as Locações Cadastradas
        /// </returns>
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

        /// <summary>
        /// Rota GET: api/Locacoes/{id}
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". o token é obtido na rota api/Login <br/>
        /// Se estiver usando o Swagger tem um botão no topo, a direita escrito "Authorize", clique nele e preencha com a palavra "Bearer", um espaço e depois o token
        /// </remarks>
        /// <param name="id">
        /// Id da Locação
        /// </param>
        /// <returns>
        /// Retorna a Locação do id informado
        /// </returns>
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

        /// <summary>
        /// Rota PUT: api/Locacoes/{id}
        /// Altera os dados da Locação do id informado
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". o token é obtido na rota api/Login <br/>
        /// Se estiver usando o Swagger tem um botão no topo, a direita escrito "Authorize", clique nele e preencha com a palavra "Bearer", um espaço e depois o token
        /// As propriedades da Locação não informadas serão ignoradas <br/>
        /// O Id e DataInclusao do Json sempre são ignorados <br/>
        /// A DataAlteracao é preenchida automaticamente, mesmo que seja informada <br/>
        /// É feita uma validação se o Veículo já não está locado
        /// </remarks>
        /// <param name="id">
        /// id da Locação a ser alterada
        /// </param>
        /// <param name="locacaoDto">
        /// Dados da Locação que devem ser alterados
        /// </param>
        /// <returns>
        /// Retorna a Locação com os dados alterados
        /// </returns>
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

        /// <summary>
        /// Rota POST: api/Locacoes
        /// Insere uma nova Locação
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". o token é obtido na rota api/Login <br/>
        /// Se estiver usando o Swagger tem um botão no topo, a direita escrito "Authorize", clique nele e preencha com a palavra "Bearer", um espaço e depois o token
        /// O Id, DataInclusao e DataAlteracao são preenchidos automaticamente, mesmo que sejam informadas <br/>
        /// O IdUsuario, IdVeiculo, IdCliente, DataInicio e DataFimPrevisto são obrigatórios <br/>
        /// É feita uma validação se o Veículo já não está locado
        /// </remarks>
        /// <param name="locacaoDto">
        /// Dados da nova Locação
        /// </param>
        /// <returns>
        /// Retorna a Locação criada
        /// </returns>
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

        /// <summary>
        /// Rota DELETE: api/Locacoes/{id}
        /// Exclui a Locação do id informado
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". o token é obtido na rota api/Login <br/>
        /// Se estiver usando o Swagger tem um botão no topo, a direita escrito "Authorize", clique nele e preencha com a palavra "Bearer", um espaço e depois o token
        /// </remarks>
        /// <param name="id">
        /// id da Locação a ser excluída
        /// </param>
        /// <returns>
        /// Retorna NoContent
        /// </returns>
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
            if (locacao.IdUsuario <= 0 || locacao.IdVeiculo <= 0 || locacao.IdCliente <= 0)
            {
                throw new ArgumentException("Impossível salvar uma locação sem informar o IdUsuario, IdVeiculo e o IdCliente");
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
