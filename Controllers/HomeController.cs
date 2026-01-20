using Fitness.Data;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.Controllers
{
    public class HomeController : Controller
    {
        private FitnessDbContext _context { get; }
        public HomeController(FitnessDbContext context)
        {
            _context=context;
        }
        public IActionResult Index()
        {
            return View(_context.trainers.Where(c=>!c.IsDeleted));
        }
    }
}
