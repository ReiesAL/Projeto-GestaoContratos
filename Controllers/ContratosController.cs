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
    [Authorize]
    public class ContratosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContratosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contratos não deletados
        public async Task<IActionResult> Index()
        {
            // Obtém todos os contratos onde Remocao é falso ou nulo
            var contratos = await _context.Contratos
                .Where(c => !c.Remocao) // Filtra contratos não removidos
                .ToListAsync();

            return View(contratos);
        }

        // GET: Contratos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contratos = await _context.Contratos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contratos == null)
            {
                return NotFound();
            }

            // Adicionando log com as informações local
            _context.LogUsuarios.Add(
                new LogUsuarios
                {
                    EmailUsuario = User.Identity.Name,
                    Detalhes = $"Entrou na tela de detalhes do contrato: {contratos.Id} - {contratos.Nome} em {DateTime.Now.ToLongDateString()}"
                });

            _context.SaveChanges();

            return View(contratos);
        }

        // GET: Contratos/Create
        public IActionResult Create()
        { 
            return View();
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cpf,Contrato,Nome,Produto,Valor,Vencimento,DataInclusao,UsuarioEmail")] Contratos contratos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contratos);

                // Adicionando log com as informações local
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


        // GET: Contratos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contratos = await _context.Contratos.FindAsync(id);
            if (contratos == null)
            {
                return NotFound();
            }

            return View(contratos);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cpf,Contrato,Nome,Produto,Valor,Vencimento")] Contratos contratos)
        {
            if (id != contratos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contratos);
                    await _context.SaveChangesAsync();

                    // Adicionando log com as informações local
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

        // GET: Contratos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contratos = await _context.Contratos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contratos == null)
            {
                return NotFound();
            }

            return View(contratos);
        }

        // POST: Contratos/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var contratos = await _context.Contratos.FindAsync(id);
        //    if (contratos != null)
        //    {
        //        _context.Contratos.Remove(contratos);

        //        // Adicionando log com as informações local
        //        _context.LogUsuarios.Add(
        //            new LogUsuarios
        //            {
        //                EmailUsuario = User.Identity.Name,
        //                Detalhes = $"Removeu o contrato: {contratos.Contrato} - {contratos.Nome} em {DateTime.Now.ToLongDateString()}"
        //            });

        //        _context.SaveChanges();
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato != null)
            {
                contrato.Remocao = true; // Marca o contrato como removido
                //contrato.DataRemocao = DateTime.Now.ToLongDateString(); // Adicionando data de remoção
                _context.Update(contrato);

                // Adicionando log com as informações local
                _context.LogUsuarios.Add(new LogUsuarios
                {
                    EmailUsuario = User.Identity.Name,
                    Detalhes = $"Removeu o contrato: {contrato.Contrato} - {contrato.Nome} em {DateTime.Now.ToLongDateString()}"
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool ContratosExists(int id)
        {
            return _context.Contratos.Any(e => e.Id == id);
        }

        // Arquivo csv
        // Controlador para importar CSV
        [HttpPost]
        public IActionResult ImportCsv(IFormFile file)
        {

            try
            {
                // Configuração do CsvHelper para ler o CSV com reforço de cultura
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
                    // Registra a classe de mapeamento se necessário
                    csv.Context.RegisterClassMap<ContratosMap>();

                    // Lê os registros do CSV e os converte para a lista de Contratos
                    var records = csv.GetRecords<Contratos>().ToList();

                    // Adiciona os registros ao contexto do banco de dados
                    _context.Contratos.AddRange(records);

                    // Adicionando log com as informações local
                    _context.LogUsuarios.Add(
                    new LogUsuarios
                    {
                        EmailUsuario = User.Identity.Name,
                        Detalhes = $"Adicionou uma carga de contratos em {DateTime.Now.ToLongDateString()}"
                    });

                    _context.SaveChanges();
                }

                return RedirectToAction("Index"); // Redireciona para a página de listagem ou onde desejar
            }
            catch
            {
                return View("Index");
            }
        }
    }
}
