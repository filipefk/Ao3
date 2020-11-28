﻿using System;
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
    public class VeiculosController : ControllerBase
    {
        private readonly RentcarsContext _context;

        public VeiculosController(RentcarsContext context)
        {
            _context = context;
        }

        // GET: api/Veiculos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VeiculoDto>>> GetTodoItems()
        {
            try
            {
                return VeiculoDto.ToDtoList(await _context.Veiculos.ToListAsync());
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
                throw new ArgumentException("Erro ao tentar buscar a lista de veículos - " + msg);
            }
        }

        // GET: api/Veiculos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VeiculoDto>> GetVeiculo(int id)
        {
            try
            {
                Veiculo veiculo = await VeiculoById(id);

                if (veiculo == null)
                {
                    return NotFound();
                }

                VeiculoDto veiculoDto = VeiculoDto.ToDto(veiculo);

                return veiculoDto;
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
                throw new ArgumentException("Erro ao tentar buscar o veículo de id = " + id + " - " + msg);
            }

        }

        // PUT: api/Veiculos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeiculo(int id, VeiculoDto veiculoDto)
        {
            try
            {
                Veiculo veiculo = await VeiculoById(id);

                if (veiculo == null)
                {
                    return NotFound();
                }

                veiculo = VeiculoDto.PutEntity(veiculo, veiculoDto);
                veiculo.DataAlteracao = DateTime.Now;
                ValidaVeiculo(veiculo);

                _context.Entry(veiculo).State = EntityState.Modified;

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
                throw new ArgumentException("Erro ao tentar alterar o veículo de id = " + id + " - " + msg);
            }
        }

        // POST: api/Veiculos
        [HttpPost]
        public async Task<ActionResult<VeiculoDto>> PostVeiculo(VeiculoDto veiculoDto)
        {
            try
            {
                Veiculo veiculo = VeiculoDto.ToEntity(veiculoDto);
                veiculo.Id = 0;
                veiculo.DataInclusao = DateTime.Now;
                veiculo.DataAlteracao = DateTime.Now;
                ValidaVeiculo(veiculo);
                _context.Veiculos.Add(veiculo);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetVeiculo), new { id = veiculo.Id }, veiculo);
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
                throw new ArgumentException("Erro ao tentar incluir um novo veículo - " + msg);
            }
        }

        // DELETE: api/Veiculos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            try
            {
                Veiculo veiculo = await VeiculoById(id);
                if (veiculo == null)
                {
                    return NotFound();
                }

                _context.Veiculos.Remove(veiculo);
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
                throw new ArgumentException("Erro ao tentar excluir o veículo de id = " + id + " - " + msg);
            }
        }

        private async Task<Veiculo> VeiculoById(int id)
        {
            return await _context.Veiculos.FindAsync(id);
        }

        private void ValidaVeiculo(Veiculo veiculo)
        {
            int MinimoAnoVeiculo = int.Parse(AppData.Configuration["ConsistenciaDados:MinimoAnoVeiculo"]);
            int MaximoAnoVeiculo = DateTime.Now.Year + 1;
            if (veiculo.AnoFabricacao < MinimoAnoVeiculo)
            {
                throw new ArgumentException("O ano de fabricação do veículo deve ser no mínimo " + MinimoAnoVeiculo);
            }
            if (veiculo.AnoFabricacao > MaximoAnoVeiculo)
            {
                throw new ArgumentException("O ano de fabricação do veículo deve ser no máximo " + MaximoAnoVeiculo);
            }
            if (veiculo.AnoModelo < MinimoAnoVeiculo)
            {
                throw new ArgumentException("O ano de modelo do veículo deve ser no mínimo " + MinimoAnoVeiculo);
            }
            if (veiculo.AnoModelo > MaximoAnoVeiculo)
            {
                throw new ArgumentException("O ano de modelo do veículo deve ser no máximo " + MaximoAnoVeiculo);
            }
            if (string.IsNullOrEmpty(veiculo.Marca))
            {
                throw new ArgumentException("A marca do veículo é obrigatória");
            }
            if (string.IsNullOrEmpty(veiculo.Modelo))
            {
                throw new ArgumentException("O modelo do veículo é obrigatório");
            }
            if (string.IsNullOrEmpty(veiculo.Placa))
            {
                throw new ArgumentException("A placa do veículo é obrigatória");
            }
            string placa = _context.Veiculos.Where(v => v.Id != veiculo.Id && v.Placa == veiculo.Placa).Select(v => v.Placa).FirstOrDefault();
            if (!string.IsNullOrEmpty(placa))
            {
                throw new ArgumentException("A placa " + placa + " já está cadastrada para outro veículo");
            }
        }

        //private bool VeiculoExists(int id)
        //{
        //    return _context.Veiculos.Any(e => e.Id == id);
        //}
    }
}
