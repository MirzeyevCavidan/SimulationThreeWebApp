using SimulationThreeWebApp.Models.Common;

namespace SimulationThreeWebApp.Models;

public class Member : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public int PositionId { get; set; }
    public Position Position { get; set; }
}
