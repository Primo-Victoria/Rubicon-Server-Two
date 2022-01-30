using System;
using System.Collections.Generic;
using System.Text;

namespace Rubicon_Server_Two.Core.Models
{
    public class UserIdentityDto
    {
        public string? NumberPrefix { get; set; }
        public string? Number { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string? Login { get; set; }
    }
}
