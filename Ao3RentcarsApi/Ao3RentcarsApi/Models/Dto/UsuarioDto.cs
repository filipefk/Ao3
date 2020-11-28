using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ao3RentcarsApi.Models.Dto
{
    public class UsuarioDto
    {
        public int Id { get; set; }

        public DateTime DataInclusao { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string Nome { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }

        /// <summary>
        /// Copia os dados de um Usuario para um novo UsuarioDto
        /// Obs.: Por questão de segurança, a senha não é copiada para o UsuarioDto
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>UsuarioDto</returns>
        public static UsuarioDto ToDto(Usuario usuario)
        {
            UsuarioDto usuarioDto = new UsuarioDto()
            {
                Id = usuario.Id,
                DataInclusao = usuario.DataInclusao,
                DataAlteracao = usuario.DataAlteracao,
                Nome = usuario.Nome,
                Login = usuario.Login
            };
            return usuarioDto;
        }

        /// <summary>
        /// Copia os dados de um UsuarioDto para um novo Usuario
        /// </summary>
        /// <param name="usuarioDto"></param>
        /// <returns>Usuario</returns>
        public static Usuario ToEntity(UsuarioDto usuarioDto)
        {
            Usuario usuario = new Usuario()
            {
                Id = usuarioDto.Id,
                Tipo = 1, // Todos vão ser do tipo "1 - Funcionario" inicialmente
                DataInclusao = usuarioDto.DataInclusao,
                DataAlteracao = usuarioDto.DataAlteracao,
                Nome = usuarioDto.Nome,
                Login = usuarioDto.Login,
                Senha = usuarioDto.Senha
            };
            return usuario;
        }

        /// <summary>
        /// Copia só os dados para Update (Put) do usuarioDto para o usuario 
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="usuarioDto"></param>
        /// <returns>Usuario</returns>
        public static Usuario PutEntity(Usuario usuario, UsuarioDto usuarioDto)
        {
            if (!string.IsNullOrEmpty(usuarioDto.Login))
            {
                usuario.Login = usuarioDto.Login;
            }
            if (!string.IsNullOrEmpty(usuarioDto.Nome))
            {
                usuario.Nome = usuarioDto.Nome;
            }
            if (!string.IsNullOrEmpty(usuarioDto.Senha))
            {
                usuario.Senha = usuarioDto.Senha;
            }
            usuario.DataAlteracao = DateTime.Now;
            return usuario;
        }

        /// <summary>
        /// Converte uma List de Usuario para uma List de UsuarioDto
        /// Obs.: Por questão de segurança, a senha não é copiada para o UsuarioDto
        /// </summary>
        /// <param name="usuarios"></param>
        /// <returns>List de UsuarioDto</returns>
        public static List<UsuarioDto> ToDtoList(List<Usuario> usuarios)
        {
            IEnumerable<UsuarioDto> usuariosDto = from u in usuarios
                                                  select new UsuarioDto()
                                                  {
                                                      Id = u.Id,
                                                      DataInclusao = u.DataInclusao,
                                                      DataAlteracao = u.DataAlteracao,
                                                      Nome = u.Nome,
                                                      Login = u.Login
                                                  };

            return usuariosDto.ToList();
        }

    }
}
