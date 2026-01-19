using SimulationThreeWebApp.Models;

namespace SimulationThreeWebApp.Areas.Admin.ViewModels;

public class CreateMemberVM
{
    public string Name { get; set; }
    public IFormFile ImageFile { get; set; }
    public int PositionId { get; set; }
    public List<Position> Positions { get; set; } = new List<Position>();
}
