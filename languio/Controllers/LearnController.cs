using Languio.Data;
using Languio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Languio.Controllers
{
    public class LearnController : Controller
    {
        private readonly AppDbContext _context;
        public LearnController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index(string lang)
        {
            var groups = _context.Groups.Include(g => g.Lessons).ToList();
            return View(groups);
        }
    }
}
