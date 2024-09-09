using CsvHelper.Configuration;

namespace Projeto_GestaoContratos.Models.Mapping
{
    // Classe de mapeamento dos dados do arquivo csv herdando a classe principal
    public class ContratosMap : ClassMap<Contratos>
    {
        // Momento que ocorre o mapeamento de onde vem as colunas para onde quero
        public ContratosMap()
        {
            Map(m => m.Id).Ignore();
            Map(m => m.Cpf).Name("CPF");
            Map(m => m.Contrato).Name("Contrato");
            Map(m => m.Nome).Name("Nome");
            Map(m => m.Produto).Name("Produto");
            Map(m => m.Valor).Name("Valor");
            Map(m => m.Vencimento).Name("Vencimento").TypeConverterOption.Format("dd/MM/yyyy"); // Se precisar do formato específico
            Map(m => m.DataInclusao).Default(DateTime.Now); // Atribui a data atual com horário
            Map(m => m.UsuarioEmail).Default("default@example.com"); // Atribui um email padrão
        }
    }
}
