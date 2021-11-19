using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Okonomen.Areas.Identity.Code;
using Okonomen.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Okonomen.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleHandler _roleHandler;
        private readonly IServiceProvider _serviceProvider;

        public HomeController(
            ILogger<HomeController> logger, 
            RoleHandler roleHandler,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _roleHandler = roleHandler;
            _serviceProvider = serviceProvider;

        }

        public IActionResult Index()
        {
           

            //sæt rollen ved startup
           // await _roleHandler.CreateRole("Admin", _serviceProvider);
            //await _roleHandler.SetRole("janniks@comxnet.dk", "Admin", _serviceProvider);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Skat()
        {
            return View();
        }
        [Authorize("RequireAuthenticatedUser")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
