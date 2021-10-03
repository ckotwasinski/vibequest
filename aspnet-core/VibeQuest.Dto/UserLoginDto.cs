﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class UserLoginDto
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public DateTime? ExpireAt { get; set; }
    }
}
