using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Projeto_GestaoContratos.Models
{
    [Table("LogUsuarios")]

    public class LogUsuarios
    { 
        [Column("Id")]
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Column("Detalhes")]
        [Display(Name = "Detalhes")]
        public string Detalhes { get; set; }

        [Column("EmailUsuario")]
        [Display(Name = "Email Usuário")]
        public string EmailUsuario { get; set; }
        
    }

}
