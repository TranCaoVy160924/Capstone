using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Data.Model;

public class UserModel 
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string? UserAva { get; set; }
    public string FcmToken { get; set; }
    public int CurrenNoticeCount { get; set; }
    public List<UserRoleModel>? userRoles { get; set; }
}

public class UserViewModel
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string FcmToken { get; set; }

    [JsonProperty("userAva")]
    public string? UserAva { get; set; }
}

public class UserPostModel
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string? UserAva { get; set; }
}

public class UserRoleModel
{
    public RoleModel role { get; set; }
}
public class UserCreateModel
{
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string FirstName { get; set; }

    public string? LastName { get; set; }
    [Required]
    public string Address { get; set; }
    
    public string PhoneNumber { get; set; }

    public string Age { get; set; }
    public string IdenficationNumber { get; set; }

}
public class UserUpdateModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Age { get; set; }
}
public class UserUpdateModelAdm
{
    public Guid Id { get; set; }
    public string RoleName { get; set; }
    public string IdenficationNumber { get; set; }
    public bool IdenficationConfirm { get; set; }
    public bool isBlock { get; set; }
    public bool isDelete { get; set; }


}
public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class AssignRoleModel
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public bool isAssign { get; set; }
}

public class UserSearchModel
{
    public string? SearchValue { get; set; } = "";
}