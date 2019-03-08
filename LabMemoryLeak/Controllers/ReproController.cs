using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LabMemoryLeak.Controllers
{
    public class ReproController : Controller
    {
        static volatile int id = 0;
        static HttpClient client = new HttpClient();
        public async Task<ActionResult> Index()
        {
            string schema = Request.IsSecureConnection ? "https": "http";
            string serverName = Request.ServerVariables["server_name"];
            string serverPort = Request.ServerVariables["server_port"];
            string path = Request.ApplicationPath;
            string urlString = $"{schema}://{serverName}:{serverPort}{path}Repro/Cached/{id++}";
            string response = await client.GetStringAsync(urlString);

            ViewData["URL"] = urlString;
            ViewData["Response"] = response;
            return View();
        }

        [OutputCache(Duration = 3*24*60*60, VaryByParam = "id")]
        public ActionResult Cached(int id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}