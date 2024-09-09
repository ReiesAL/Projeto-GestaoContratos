using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

// Definição do modelo da tabela "Contratos"
namespace Projeto_GestaoContratos.Models
{
    [Table("Contratos")]
    public class Contratos
    {
        [Column("Id")]
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Column("Cpf")]
        [Display(Name = "CPF")]
        [StringLength(14)] 
        public required string Cpf { get; set; } 

        [Column("Contrato")]
        [Display(Name = "Contrato")]
        public required int Contrato { get; set; }

        [Column("Nome")]
        [Display(Name = "Nome")]
        public required string Nome { get; set; }

        [Column("Produto")]
        [Display(Name = "Produto")]
        public required string Produto { get; set; }

        [Precision(18, 2)]
        [Column("Valor")]
        [Display(Name = "Valor")]
        public required decimal Valor { get; set; }

        [Column("Vencimento")]
        [Display(Name = "Vencimento")]
        public DateOnly Vencimento { get; set; }

        [Column("DataInclusao")]
        [Display(Name = "Data Inclusão")]
        public DateTime DataInclusao { get; set; } = DateTime.Today; // Define a data de inclusão com hora 

        [Column("UsuarioEmail")]
        [Display(Name = "Usuário Email")]
        [StringLength(180)]  
        public string UsuarioEmail { get; set; }

    }
}
