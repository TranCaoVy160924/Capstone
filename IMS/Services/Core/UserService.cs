using AutoMapper;
using Data.DataAccess;
using Data.DataAccess.Constant;
using Data.Entities;
using Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Core;

public interface IUserService
{
    Task<ResultModel> Register(UserCreateModel model);
    Task<ResultModel> Login(LoginModel model);
}
public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;

    public UserService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public async Task<ResultModel> Login(LoginModel model)
    {

        var result = new ResultModel();

        var userByEmail = _dbContext.User.Where(s => s.Email == model.Email).FirstOrDefault();

        if (userByEmail != null)
        {
            var user = await _userManager.FindByNameAsync(userByEmail.UserName);
            var check = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (check.Succeeded)
            {
                var userRoles = _dbContext.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
                var roles = new List<string>();
                foreach (var userRole in userRoles)
                {
                    var role = await _dbContext.Role.FindAsync(userRole.RoleId);
                    if (role != null) roles.Add(role.Name);
                }
                var token = await GetAccessToken(user, roles);
                result.Succeed = true;
                result.Data = token;
            }
            else
            {
                result.Succeed = false;
                result.ErrorMessage = "Error password";
            }
        }
        else
        {

            result.ErrorMessage = "Email or Password not correct!";
        }
        return result;
    }

    public async Task<ResultModel> Register(UserCreateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            if (!await _roleManager.RoleExistsAsync("Member"))
            {
                await _roleManager.CreateAsync(new Role { Description = "Role for Member", Name = "Member" });
            }
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Member");
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Age = model.Age,
                IdenficationNumber = model.IdenficationNumber,
                IdenficationConfirm = false,
                isBlock = false,
                IsGoogle = false,
                isDelete = false,
                UserAva = "",
                NormalizedEmail = model.Email,
            };
            var userByPhone = _dbContext.User.Where(s => s.PhoneNumber == user.PhoneNumber).FirstOrDefault();
            var userByMail = _dbContext.User.Where(s => s.Email == user.Email).FirstOrDefault();
            if (userByPhone != null)
            {
                result.Succeed = false;
                result.ErrorMessage = "PHONE_NUMBER " + ErrorMessage.EXISTED;
            }
            else
            {
                if (userByMail != null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "EMAIL " + ErrorMessage.EXISTED; ;
                }
                else
                {
                    if (user.PhoneNumber.Length < 9 || user.PhoneNumber.Length > 10)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "PHONE_NUMBER " + ErrorMessage.INVALID;
                    }
                    if (user.IdenficationNumber.Length != 12)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Idenfication_Number" + ErrorMessage.INVALID;
                    }
                    else
                    {
                        var check = await _userManager.CreateAsync(user, model.Password);

                        if (check.Succeeded == true)
                        {
                            var userRole = new UserRole
                            {
                                RoleId = role.Id,
                                UserId = user.Id
                            };
                            _dbContext.UserRoles.Add(userRole);
                            await _dbContext.SaveChangesAsync();
                            result.Succeed = true;
                            result.Data = user.Id;
                        }
                        else
                        {
                            result.Succeed = false;
                            result.ErrorMessage = "REGISTER_USER_ERROR";
                        }
                    }

                }

            }
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    private async Task<Token> GetAccessToken(User user, List<string> role)
    {
        List<Claim> claims = GetClaims(user, role);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
          _configuration["Jwt:Issuer"],
          claims,
          expires: DateTime.Now.AddHours(int.Parse(_configuration["Jwt:ExpireTimes"])),
          //int.Parse(_configuration["Jwt:ExpireTimes"]) * 3600
          signingCredentials: creds);

        var serializedToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new Token
        {
            Access_token = serializedToken,
            Token_type = "Bearer",
            Expires_in = int.Parse(_configuration["Jwt:ExpireTimes"]) * 3600,
            UserID = user.Id.ToString(),
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            UserAva = user.UserAva,
            CurrenNoticeCount = user.CurrenNoticeCount,
            //Role = user.Role
        };
    }

    private List<Claim> GetClaims(User user, List<string> roles)
    {
        IdentityOptions _options = new IdentityOptions();
        var claims = new List<Claim> {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("FullName", user.FirstName + user.LastName),

                new Claim("UserName", user.UserName)
            };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        if (!string.IsNullOrEmpty(user.PhoneNumber)) claims.Add(new Claim("PhoneNumber", user.PhoneNumber));
        return claims;
    }
}
