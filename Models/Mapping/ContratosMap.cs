using CsvHelper.Configuration;

namespace Projeto_GestaoContratos.Models.Mapping
{
    // Classe de mapeamento dos dados do arquivo csv herdando a classe principal
    public class ContratosMap : ClassMap<Contratos>
    {
        // Momento que ocorre o mapeamento de onde vem as colunas para onde quero
        public ContratosMap() {
            Map(m => m.Id).Ignore(); // Ignora o campo Id se não for fornecido no CSV
            Map(m => m.Cpf).Name("CPF"); // Mapeia o campo CPF do CSV para a propriedade Cpf
            Map(m => m.Contrato).Name("Contrato"); // Mapeia o campo Contrato do CSV para a propriedade Contrato
            Map(m => m.Nome).Name("Nome").ConvertUsing(row => CleanString(row.GetField("Nome"))); // Limpa e atribui a string do campo Nome
            Map(m => m.Produto).Name("Produto").ConvertUsing(row => CleanString(row.GetField("Produto"))); // Limpa e atribui a string do campo Produto
            Map(m => m.Valor).Name("Valor"); // Mapeia o campo Valor do CSV para a propriedade Valor
            Map(m => m.Vencimento).Name("Vencimento") // Mapeia o campo Vencimento do CSV para a propriedade Vencimento
                .TypeConverterOption.Format("dd/MM/yyyy"); // Formata a data conforme o formato no CSV
            Map(m => m.DataInclusao).Convert(row => DateTime.Now); // Atribui a data e hora atuais
            Map(m => m.UsuarioEmail).Default("default@example.com"); // Atribui um email padrão
        }
    }
}
