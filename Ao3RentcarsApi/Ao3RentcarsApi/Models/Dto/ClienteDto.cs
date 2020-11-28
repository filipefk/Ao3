using Ao3RentcarsApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ao3RentcarsApi.Models.Dto
{
    public class ClienteDto
    {
        public int Id { get; set; }

        public DateTime DataInclusao { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string Nome { get; set; }

        private string _cpf;

        public string Cpf {
            get { return _cpf; } 
            set { _cpf = Validador.SoNumeros(value); } 
        }

        /// <summary>
        /// Copia os dados de um Cliente para um novo ClienteDto
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns>ClienteDto</returns>
        public static ClienteDto ToDto(Cliente cliente)
        {
            ClienteDto clienteDto = new ClienteDto()
            {
                Id = cliente.Id,
                DataInclusao = cliente.DataInclusao,
                DataAlteracao = cliente.DataAlteracao,
                Nome = cliente.Nome,
                Cpf = cliente.Cpf
            };
            return clienteDto;
        }

        /// <summary>
        /// Copia os dados de um ClienteDto para um novo Cliente
        /// </summary>
        /// <param name="clienteDto"></param>
        /// <returns>Cliente</returns>
        public static Cliente ToEntity(ClienteDto clienteDto)
        {
            Cliente cliente = new Cliente()
            {
                Id = clienteDto.Id,
                DataInclusao = clienteDto.DataInclusao,
                DataAlteracao = clienteDto.DataAlteracao,
                Nome = clienteDto.Nome,
                Cpf = clienteDto.Cpf
            };
            return cliente;
        }

        /// <summary>
        /// Copia só os dados para Update (Put) do clienteDto para o cliente 
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="clienteDto"></param>
        /// <returns>Cliente</returns>
        public static Cliente PutEntity(Cliente cliente, ClienteDto clienteDto)
        {
            if (!string.IsNullOrEmpty(clienteDto.Nome))
            {
                cliente.Nome = clienteDto.Nome;
            }
            if (!string.IsNullOrEmpty(clienteDto.Cpf))
            {
                cliente.Cpf = clienteDto.Cpf;
            }
            cliente.DataAlteracao = DateTime.Now;
            return cliente;
        }

        /// <summary>
        /// Converte uma List de Cliente para uma List de ClienteDto
        /// </summary>
        /// <param name="clientes"></param>
        /// <returns>List de ClienteDto</returns>
        public static List<ClienteDto> ToDtoList(List<Cliente> clientes)
        {
            IEnumerable<ClienteDto> clientesDto = from u in clientes
                                                  select new ClienteDto()
                                                  {
                                                      Id = u.Id,
                                                      DataInclusao = u.DataInclusao,
                                                      DataAlteracao = u.DataAlteracao,
                                                      Nome = u.Nome,
                                                      Cpf = u.Cpf
                                                  };

            return clientesDto.ToList();
        }
    }
}
