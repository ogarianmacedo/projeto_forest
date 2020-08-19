using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ProjetoForest.Models
{
    public class User : IdentityUser<int>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}