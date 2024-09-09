using CsvHelper.Configuration;

namespace Projeto_GestaoContratos.Models.Mapping
{
    public class ContratosMap : ClassMap<Contratos>
    {
        public ContratosMap(string userName)
        {
            Map(m => m.Id).Ignore();
            Map(m => m.Cpf).Name("CPF");
            Map(m => m.Contrato).Name("Contrato");
            Map(m => m.Nome).Name("Nome");
            Map(m => m.Produto).Name("Produto");
            Map(m => m.Valor).Name("Valor");
            Map(m => m.Vencimento).Name("Vencimento").TypeConverterOption.Format("dd/MM/yyyy");
            Map(m => m.DataInclusao).Default(DateTime.Now); // Atribui a data atual com horário
            Map(m => m.UsuarioInclusao).Default(userName); // Usa o nome do usuário como valor padrão
        }
    }
}