using SimulationThreeWebApp.Models.Common;

namespace SimulationThreeWebApp.Models;

public class Position : BaseEntity
{
    public string Name { get; set; }
    public List<Member> Members { get; set; }
}
