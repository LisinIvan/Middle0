using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middle0.Domain.Common.DTO
{
    public class LoginResponseDto
    {
		public string Token { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
	}
}
