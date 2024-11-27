using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CredentialRequest
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
