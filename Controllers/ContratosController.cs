﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_GestaoContratos.Data;
using Projeto_GestaoContratos.Models;

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

        // GET: Contratos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contratos.ToListAsync());
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
                    Detalhes = $"Entrou na tela de detalhes do contrato {contratos.Id} - {contratos.Nome}"
                });

            await _context.SaveChangesAsync();

            return View(contratos);
        }

        // GET: Contratos/Create
        public IActionResult Create()
        {
            // Adicionando log com as informações local
            _context.LogUsuarios.Add(
                new LogUsuarios
                {
                    EmailUsuario = User.Identity.Name,
                    Detalhes = "Entrou na tela de cadastro de contratos"
                });

            _context.SaveChanges();

            return View();
        }

        // POST: Contratos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cpf,Contrato,Nome,Produto,Valor,Vencimento,DataInclusao,UsuarioEmail")] Contratos contratos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contratos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Adicionando log com as informações local
            _context.LogUsuarios.Add(
                new LogUsuarios
                {
                    EmailUsuario = User.Identity.Name,
                    Detalhes = $"Adicionou o contrato {contratos.Id} - {contratos.Nome}"
                });

            await _context.SaveChangesAsync();

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

            // Adicionando log com as informações local
            _context.LogUsuarios.Add(
                new LogUsuarios
                {
                    EmailUsuario = User.Identity.Name,
                    Detalhes = $"Entrou na tela de edição de contratos {contratos.Id} - {contratos.Nome}"
                });

            await _context.SaveChangesAsync();

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
                            Detalhes = $"Atualizou o contrato: {contratos.Contrato} Data de atualização: {DateTime.Now.ToLongDateString()}"
                        });
                    await _context.SaveChangesAsync();

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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contratos = await _context.Contratos.FindAsync(id);
            if (contratos != null)
            {
                _context.Contratos.Remove(contratos);

                // Adicionando log com as informações local
                _context.LogUsuarios.Add(
                    new LogUsuarios
                    {
                        EmailUsuario = User.Identity.Name,
                        Detalhes = $"Removeu o contrato: {contratos.Contrato} Data de remoção: {DateTime.Now.ToLongDateString()}"
                    });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ContratosExists(int id)
        {
            return _context.Contratos.Any(e => e.Id == id);
        }
    }
}
