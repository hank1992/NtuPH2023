using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NtuPH2023.Models;
using System.Diagnostics;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using NtuPH2023.Data;
using Microsoft.EntityFrameworkCore;

namespace NtuPH2023.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly GraphServiceClient _graphServiceClient;
        private readonly ILogger<HomeController> _logger;
        private readonly NtuPH2023Context _context;

        public HomeController(ILogger<HomeController> logger, NtuPH2023Context context)
        {
            _logger = logger;
            //_graphServiceClient = graphServiceClient; 
            _context = context;
        }

        public IActionResult Index()
        {
            //var user = await _graphServiceClient.Me.Request().GetAsync();
            //ViewData["GraphApiResult"] = user.DisplayName;
            return View();
        }

        public async Task<IActionResult> Comment()
        {
            var comments = await _context.TblComments.AsNoTracking().OrderByDescending(m => m.CreatedTimestamp).ToListAsync();
            
            return View(comments);
        }

        public async Task<IActionResult> News()
        {
            var realtedNews = await _context.TblNews.AsNoTracking().OrderByDescending(m => m.CreatedTimestamp).ToListAsync();

            return View(realtedNews);
        }

        public async Task<IActionResult> RelatedInfo()
        {
            var realtedInfos = await _context.TblRelatedInfos.AsNoTracking().OrderByDescending(m => m.CreatedTimestamp).ToListAsync();

            return View(realtedInfos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}