using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ao3RentcarsApi.Models
{
	[Table("Locacao")]
	public class Locacao
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
		public DateTime DataInicio { get; set; }

		public DateTime DataFim { get; set; }

		[ForeignKey("Usuario")]
		public int IdUsuario { get; set; }
		public Usuario Usuario { get; set; }

		[ForeignKey("Veiculo")]
		public int IdVeiculo { get; set; }
		public Veiculo Veiculo { get; set; }
	}
}
