using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using System.Diagnostics;
using WebShell.Connection;
using WebShell.Data;
using WebShell.Models;

namespace WebShell.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;
        private readonly SshService _sshService;

        public HomeController(ILogger<HomeController> logger, SshService sshService, AppDbContext dbContext)
        {
            _logger = logger;
            _sshService = sshService;
            _dbContext = dbContext;
        }

        // Carrega os comandos de forma assíncrona dentro do método Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Carregar os comandos do banco de dados
            List<CommandModel> commands = await _dbContext.Commands
                .OrderByDescending(c => c.Last_Usage)
                .Take(6)
                .ToListAsync();

            return View(commands);
        }

        [HttpPost]
        [Route("cmd")]
        public async Task<IActionResult> ExecCommand(string command)
        {
            try
            {
                // Cria e armazena o comando no banco de dados
                var cmd = new CommandModel { Command = command, Last_Usage = DateTime.Now };

                await _dbContext.Commands.AddAsync(cmd);
                await _dbContext.SaveChangesAsync();

                // Executa o comando via SSH e obtém o resultado
                var output = await _sshService.ExecCommand(command);

                // Passa o resultado para a View
                ViewBag.CommandResult = System.Net.WebUtility.HtmlEncode(output);
            }
            catch (Exception ex)
            {
                ViewBag.CommandResult = "Erro ao executar comando: " + ex.Message;
            }

            // Recarrega a lista de comandos após a execução do comando
            List<CommandModel> commands = await _dbContext.Commands
                .OrderByDescending(c => c.Last_Usage)
                .Take(6)
                .ToListAsync();
            
            return View("Index", commands);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _sshService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
