using Capstone.Contract.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Business.Services.Auth;
public interface IAuthService
{
    String Authenticate(AuthRequest request);
}
