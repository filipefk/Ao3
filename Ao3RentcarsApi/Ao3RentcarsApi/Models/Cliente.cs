using Ao3RentcarsApi.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ao3RentcarsApi.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DataInclusao { get; set; }

        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DataAlteracao { get; set; }

        [Required]
        public string Nome { get; set; }

        private string _cpf;

        [Required]
        public string Cpf
        {
            get { return _cpf; }
            set { _cpf = Validador.SoNumeros(value); }
        }

        public ICollection<Locacao> Locacoes { get; set; }
    }
}
