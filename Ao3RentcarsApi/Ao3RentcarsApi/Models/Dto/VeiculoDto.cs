using System;
using System.Collections.Generic;
using System.Linq;

namespace Ao3RentcarsApi.Models.Dto
{
    public class VeiculoDto
    {
        public int Id { get; set; }

        public DateTime? DataInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public string Modelo { get; set; }

        public string Marca { get; set; }

        private string _placa { get; set; }

        public string Placa {
            get { return _placa.ToUpper(); }
            set { _placa = value.ToUpper(); }
        }

        public int AnoModelo { get; set; }

        public int AnoFabricacao { get; set; }

        public int IdLocacao { get; set; }


        /// <summary>
        /// Copia os dados de um Veiculo para um novo VeiculoDto
        /// </summary>
        /// <param name="veiculo"></param>
        /// <returns>VeiculoDto</returns>
        public static VeiculoDto ToDto(Veiculo veiculo)
        {
            VeiculoDto veiculoDto = new VeiculoDto()
            {
                Id = veiculo.Id,
                DataInclusao = veiculo.DataInclusao,
                DataAlteracao = veiculo.DataAlteracao,
                Modelo = veiculo.Modelo,
                Marca = veiculo.Marca,
                Placa = veiculo.Placa,
                AnoModelo = veiculo.AnoModelo,
                AnoFabricacao = veiculo.AnoFabricacao
            };
            return veiculoDto;
        }

        /// <summary>
        /// Copia os dados de um VeiculoDto para um novo Veiculo
        /// </summary>
        /// <param name="veiculoDto"></param>
        /// <returns>Veiculo</returns>
        public static Veiculo ToEntity(VeiculoDto veiculoDto)
        {
            Veiculo veiculo = new Veiculo()
            {
                Id = veiculoDto.Id,
                DataInclusao = veiculoDto.DataInclusao == null ? DateTime.Now : (DateTime)veiculoDto.DataInclusao,
                DataAlteracao = veiculoDto.DataAlteracao == null ? DateTime.Now : (DateTime)veiculoDto.DataAlteracao,
                Modelo = veiculoDto.Modelo,
                Marca = veiculoDto.Marca,
                Placa = veiculoDto.Placa,
                AnoModelo = veiculoDto.AnoModelo,
                AnoFabricacao = veiculoDto.AnoFabricacao
            };
            return veiculo;
        }

        /// <summary>
        /// Copia só os dados para Update (Put) do veiculoDto para o veiculo 
        /// </summary>
        /// <param name="veiculo"></param>
        /// <param name="veiculoDto"></param>
        /// <returns>Veiculo</returns>
        public static Veiculo PutEntity(Veiculo veiculo, VeiculoDto veiculoDto)
        {
            if (veiculoDto.AnoFabricacao > 0)
            {
                veiculo.AnoFabricacao = veiculoDto.AnoFabricacao;
            }
            if (veiculoDto.AnoModelo > 0)
            {
                veiculo.AnoModelo = veiculoDto.AnoModelo;
            }
            veiculo.DataAlteracao = DateTime.Now;
            if (!string.IsNullOrEmpty(veiculoDto.Marca))
            {
                veiculo.Marca = veiculoDto.Marca;
            }
            if (!string.IsNullOrEmpty(veiculoDto.Modelo))
            {
                veiculo.Modelo = veiculoDto.Modelo;
            }
            if (!string.IsNullOrEmpty(veiculoDto.Placa))
            {
                veiculo.Placa = veiculoDto.Placa;
            }
            return veiculo;
        }

        /// <summary>
        /// Converte uma List de Veiculo para uma List de VeiculoDto
        /// </summary>
        /// <param name="veiculos"></param>
        /// <returns>List de VeiculoDto</returns>
        public static List<VeiculoDto> ToDtoList(List<Veiculo> veiculos)
        {
            IEnumerable<VeiculoDto> veiculosDto = from v in veiculos
                                                  select new VeiculoDto()
                                                  {
                                                      Id = v.Id,
                                                      DataInclusao = v.DataInclusao,
                                                      DataAlteracao = v.DataAlteracao,
                                                      Modelo = v.Modelo,
                                                      Marca = v.Marca,
                                                      Placa = v.Placa,
                                                      AnoModelo = v.AnoModelo,
                                                      AnoFabricacao = v.AnoFabricacao
                                                  };

            return veiculosDto.ToList();
        }

    }
}
