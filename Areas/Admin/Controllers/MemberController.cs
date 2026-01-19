using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulationThreeWebApp.Areas.Admin.ViewModels;
using SimulationThreeWebApp.DAL.Context;
using SimulationThreeWebApp.Models;
using SimulationThreeWebApp.Utilities.Enums;
using SimulationThreeWebApp.Utilities.Extensions;


namespace SimulationThreeWebApp.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]

public class MemberController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public MemberController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        List<Member> members = await _context.Members.Include(w => w.Position).ToListAsync();

        return View(members);
    }


    public async Task<IActionResult> Create()
    {
        List<Position> positions = await _context.Positions.ToListAsync();

        CreateMemberVM createMemberVM = new()
        {
            Positions = positions
        };

        return View(createMemberVM);
    }

    [HttpPost]

    public async Task<IActionResult> Create(CreateMemberVM createMemberVM)
    {
        if (!ModelState.IsValid)
        {
            createMemberVM.Positions = await _context.Positions.ToListAsync();
            return View(createMemberVM);
        }


        if (!createMemberVM.ImageFile.ValidateType("image"))
        {
            ModelState.AddModelError("ImageFile", "Please select a valid image file.");
            createMemberVM.Positions = await _context.Positions.ToListAsync();
            return View(createMemberVM);
        }

        if (!createMemberVM.ImageFile.ValidateSize(FileSize.MB, 2))
        {
            ModelState.AddModelError("ImageFile", "Image size must be less than 2 MB.");
            createMemberVM.Positions = await _context.Positions.ToListAsync();
            return View(createMemberVM);
        }


        Member member = new()
        {
            Name = createMemberVM.Name,
            PositionId = createMemberVM.PositionId,
            ImagePath = await createMemberVM.ImageFile.CreateFileAsync(_env.WebRootPath, "assets", "img")
        };

        await _context.Members.AddAsync(member);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Edit(int id)
    {
        Member? member = await _context.Members.FirstOrDefaultAsync(w => w.Id == id);
        if (member == null)
        {
            return NotFound();
        }

        UpdateMemberVM updateMemberVM = new()
        {
            Id = member.Id,
            Name = member.Name,
            PositionId = member.PositionId,
            Positions = await _context.Positions.ToListAsync()
        };

        return View(updateMemberVM);
    }

    [HttpPost]

    public async Task<IActionResult> Edit(int id, UpdateMemberVM updateMemberVM)
    {
        if (!ModelState.IsValid)
        {
            updateMemberVM.Positions = await _context.Positions.ToListAsync();
            return View(updateMemberVM);
        }


        Member? member = await _context.Members.FirstOrDefaultAsync(w => w.Id == id);
        if (member == null)
        {
            return NotFound();
        }

        if (await _context.Members.AnyAsync(w => w.Name == updateMemberVM.Name && w.Id != id))
        {
            ModelState.AddModelError("Name", "This member fullname is already taken by another member.");
            updateMemberVM.Positions = await _context.Positions.ToListAsync();
            return View(updateMemberVM);
        }

        member.Name = updateMemberVM.Name;
        member.PositionId = updateMemberVM.PositionId;

        if (updateMemberVM.ImageFile != null)
        {
            if (!updateMemberVM.ImageFile.ValidateType("image"))
            {
                ModelState.AddModelError("ImageFile", "Please select a valid image file.");
                updateMemberVM.Positions = await _context.Positions.ToListAsync();
                return View(updateMemberVM);
            }

            if (!updateMemberVM.ImageFile.ValidateSize(FileSize.MB, 2))
            {
                ModelState.AddModelError("ImageFile", "Image size must be less than 2 MB.");
                updateMemberVM.Positions = await _context.Positions.ToListAsync();
                return View(updateMemberVM);
            }

            member.ImagePath = await updateMemberVM.ImageFile.CreateFileAsync(_env.WebRootPath, "assets", "img");

        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        Member? member = await _context.Members.FirstOrDefaultAsync(w => w.Id == id);
        if (member == null)
        {
            return NotFound();
        }

        _context.Members.Remove(member);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}