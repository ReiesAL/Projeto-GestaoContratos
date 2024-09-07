using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        public string Cpf { get; set; } 

        [Column("Contrato")]
        [Display(Name = "Contrato")]
        public int Contrato { get; set; }

        [Column("Nome")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Column("Produto")]
        [Display(Name = "Produto")]
        public string Produto { get; set; }

        [Column("Valor")]
        [Display(Name = "Valor")]
        public decimal Valor { get; set; } 

        [Column("Vencimento")]
        [Display(Name = "Vencimento")]
        public DateTime Vencimento { get; set; } 

        [Column("DataInclusao")]
        [Display(Name = "Data Inclusão")]
        public DateTime DataInclusao { get; set; }

        [Column("UsuarioCpf")]
        [Display(Name = "Usuário CPF")]
        [StringLength(14)] 
        public string UsuarioCpf { get; set; } 

    }
}
