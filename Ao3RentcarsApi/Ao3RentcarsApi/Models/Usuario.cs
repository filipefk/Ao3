using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ao3RentcarsApi.Models
{
    [Table("Usuario")]
    public class Usuario
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

        [Required]
        public string Login { get; set; }

        [Required]
        [Encrypted]
        public string Senha { get; set; }

        public ICollection<Locacao> Locacoes { get; set; }
    }
}
