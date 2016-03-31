﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Models.Networking.RequestObjects
{
    [Serializable]
    public class UserInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
