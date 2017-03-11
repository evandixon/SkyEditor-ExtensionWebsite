using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SkyEditor.ExtensionWebsite.Data;

namespace SkyEditor.ExtensionWebsite.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IOptions<ExtensionsConfig> options, ExtensionsDbContext context)
        {
            this.Options = options.Value;
        }

        protected ExtensionsConfig Options { get; set; }

        public IActionResult Index()
        {
            if (!ExtensionsMonitor.HasStarted)
            {
                ExtensionsMonitor.StartExtensionMonitor(Options);
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
