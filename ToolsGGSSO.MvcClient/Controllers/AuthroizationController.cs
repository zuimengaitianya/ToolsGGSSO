using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ToolsGGSSO.MvcClient.Controllers
{
    public class AuthroizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}