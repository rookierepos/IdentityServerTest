using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ServerCenter.Models
{
    public class User : IdentityUser<int>
    {

    }

    public class Role : IdentityRole<int>
    {

    }
}
