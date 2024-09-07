using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_GestaoContratos.Data;
using Projeto_GestaoContratos.Models;

namespace Projeto_GestaoContratos.Controllers
{
    public class LogUsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogUsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LogUsuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.LogUsuarios.ToListAsync());
        }

    }
}
