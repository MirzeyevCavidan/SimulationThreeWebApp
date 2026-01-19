using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulationThreeWebApp.DAL.Context;
using SimulationThreeWebApp.Models;

namespace SimulationThreeWebApp.Controllers
{
    public class HomeController : Controller
    {


        private readonly AppDbContext _context;


        public HomeController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            List<Member> members = await _context.Members.Include(m => m.Position).ToListAsync();
            return View(members);
        }


    }
}
