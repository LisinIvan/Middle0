using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middle0.Application.Service.Interfaces
{
    public interface ITokenService
    {
		string GenerateJwtToken(IdentityUser user);
	}
}
