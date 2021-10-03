using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class ResetPasswordVerifyDto
    {
        public string Email { get; set; }
        public string SecurityToken { get; set; }
    }
}
