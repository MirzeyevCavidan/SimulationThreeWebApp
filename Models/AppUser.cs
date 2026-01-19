using Microsoft.AspNetCore.Identity;

namespace SimulationThreeWebApp.Models;

public class AppUser : IdentityUser
{
    public string Name { get; set; }
    public string SurName { get; set; }
}