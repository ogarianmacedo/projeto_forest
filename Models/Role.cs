using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ProjetoForest.Models
{
    public class Role : IdentityRole<int>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}