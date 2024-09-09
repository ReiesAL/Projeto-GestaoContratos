using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_GestaoContratos.Data;
using Projeto_GestaoContratos.Models;
using CsvHelper.Configuration;
using CsvReader = CsvHelper.CsvReader;
using Projeto_GestaoContratos.Models.Mapping;

namespace Projeto_GestaoContratos.Controllers
{
    // Exigindo autenticação para acessar o controller
    [Authorize] 
    public class ContratosController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Construtor do controller
        public ContratosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Busca - Contratos não deletados
        public async Task<IActionResult> Index()
        {
            // Obtém todos os contratos onde Remocao é falso ou nulo
            var contratos = await _context.Contratos
                .Where(c => !c.Remocao) 
                .ToListAsync();

            return View(contratos);
        }

        // GET: Detalhes - Busca das informações do contrato selecionado
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Obtém detalhes do contrato pelo ID
            var contratos = await _context.Contratos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contratos == null)
            {
                return NotFound();
            }

            // Adiciona log de acesso aos detalhes
            _context.LogUsuarios.Add(
                new LogUsuarios
                {
                    EmailUsuario = User.Identity.Name,
                    Detalhes = $"Entrou na tela de detalhes do contrato: {contratos.Id} - {contratos.Nome} em {DateTime.Now.ToLongDateString()}"
                });

            _context.SaveChanges();

            return View(contratos);
        }

        // GET: Adicionar - Busca dos contratos
        public IActionResult Create()
        {
            return View();
        }

        // POST: Adicionar - Criação de novo contrato manualmente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cpf,Contrato,Nome,Produto,Valor,Vencimento,DataInclusao,UsuarioInclusao")] Contratos contratos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contratos);

                // Adiciona log de criação do contrato
                _context.LogUsuarios.Add(
                new LogUsuarios
                {
                    EmailUsuario = User.Identity.Name,
                    Detalhes = $"Adicionou um novo contrato: {contratos.Contrato} - {contratos.Nome} em {DateTime.Now.ToLongDateString()}"
                });

                _context.SaveChanges();

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(contratos);
        }

        // GET: Edição - Buscar informações do contrato para adição
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Obtém o contrato a ser editado pelo ID
            var contratos = await _context.Contratos.FindAsync(id);
            if (contratos == null)
            {
                return NotFound();
            }

            return View(contratos);
        }

        // POST: Edição - Aplicar edições realizadas e gravar no banco
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cpf,Contrato,Nome,Produto,Valor,Vencimento,UsuarioInclusao")] Contratos contratos)
        {
            if (id != contratos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.Name; // Captura o nome do usuário autenticado

                    _context.Update(contratos);
                    await _context.SaveChangesAsync();

                    // Adiciona log de atualização do contrato
                    _context.LogUsuarios.Add(
                        new LogUsuarios
                        {
                            EmailUsuario = User.Identity.Name,
                            Detalhes = $"Atualizou o contrato: {contratos.Contrato} - {contratos.Nome} em {DateTime.Now.ToLongDateString()}"
                        });
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContratosExists(contratos.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(contratos);
        }

        // GET: Função para obter id para tela de confirmação de remoção do contrato
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Obtém o contrato a ser deletado pelo ID
            var contratos = await _context.Contratos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contratos == null)
            {
                return NotFound();
            }

            return View(contratos);
        }

        // POST: Função de remoção de contrato e aplica alterações no banco
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Obtém o contrato a ser removido pelo ID
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato != null)
            {
                contrato.Remocao = true; // Marca o contrato como removido
                contrato.DataRemocao = DateTime.Now; // Adiciona a data de remoção
                contrato.UsuarioRemocao = User.Identity.Name;

                _context.Update(contrato);

                // Adiciona log de remoção do contrato
                _context.LogUsuarios.Add(new LogUsuarios
                {
                    EmailUsuario = User.Identity.Name,
                    Detalhes = $"Removeu o contrato: {contrato.Contrato} - {contrato.Nome} em {DateTime.Now.ToLongDateString()}"
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Verifica se o contrato existe
        private bool ContratosExists(int id)
        {
            return _context.Contratos.Any(e => e.Id == id);
        }

        // POST: Importa dados do CSV e salva no banco
        [HttpPost]
        public IActionResult ImportCsv(IFormFile file)
        {
            try
            {
                // Configuração do CsvHelper para ler o CSV
                var config = new CsvConfiguration(new CultureInfo("pt-BR"))
                {
                    Delimiter = ";",
                    HasHeaderRecord = true,
                    HeaderValidated = null,
                    MissingFieldFound = null
                };

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, config))
                {
                    // Captura o nome do usuário autenticado
                    var userName = User.Identity.Name; 

                    // Registra a classe e passa o nome do usuário para o mapeamento
                    csv.Context.RegisterClassMap(new ContratosMap(userName));

                    // Lê e converte registros do CSV para a lista de Contratos
                    var records = csv.GetRecords<Contratos>().ToList();

                    // Adiciona registros ao contexto do banco de dados
                    _context.Contratos.AddRange(records);

                    // Adiciona log de importação de contratos
                    _context.LogUsuarios.Add(
                    new LogUsuarios
                    {
                        EmailUsuario = User.Identity.Name,
                        Detalhes = $"Adicionou uma carga de contratos em {DateTime.Now.ToLongDateString()}"
                    });

                    _context.SaveChanges();
                }

                return RedirectToAction("Index"); // Redireciona para a página de listagem
            }
            catch
            {
                return View("Index");
            }
        }
    }
}
