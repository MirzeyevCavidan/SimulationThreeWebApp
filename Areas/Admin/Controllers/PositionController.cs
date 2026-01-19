using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulationThreeWebApp.DAL.Context;
using SimulationThreeWebApp.Models;

namespace SimulationThreeWebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]

public class PositionController : Controller
{
    private readonly AppDbContext _context;

    public PositionController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Position> positions = await _context.Positions.ToListAsync();
        return View(positions);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Position position)
    {
        if (!ModelState.IsValid)
        {
            return View(position);
        }

        _context.Positions.Add(position);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Update(int id)
    {

        Position? positions = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);

        if(positions == null)
        {
            return NotFound();
        }

        return View(positions);
    }

    [HttpPost]

    public async Task<IActionResult> Update(Position positions)
    {
        if (!ModelState.IsValid)
        {
            return View(positions);
        }

        Position dbPositions = await _context.Positions.FirstOrDefaultAsync(p => p.Id == positions.Id);
        if(dbPositions == null)
        {
            return NotFound();
        }
        bool isDuplicate = await _context.Positions.AnyAsync(p => p.Id == positions.Id && p.Name != positions.Name);

        if(isDuplicate)
        {
            ModelState.AddModelError("Name", "This designation name is already taken.");
            return View(positions);
        }
        dbPositions.Name = positions.Name;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        Position positions = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
        if(positions == null)
        {
            return NotFound();
        }

        _context.Positions.Remove(positions);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
