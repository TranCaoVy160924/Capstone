﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Contract.ViewModels.Authentication;
public class AuthRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}