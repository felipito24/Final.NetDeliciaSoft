using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliciaSoft.Controllers
{
    public class HomeController : Controller
    {
        [Authorize] // Requiere autenticación para acceder
        public IActionResult Index()
        {
            // Puedes pasar datos adicionales a la vista si es necesario
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}