using System.ComponentModel.DataAnnotations;

namespace joro.too.Web.Models;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email {get; set;}
    
    [Required]
    [DataType(DataType.Password)]
    public string Password {get; set;}
    
    [Display(Name = "Remember me?")] 
    public bool RememberMe {get; set;}
}

public class RegisterViewModel
{
    [Required] 
    [EmailAddress] 
    public string Email {get; set;}
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The passwords do not match.")]
    public string ConfirmPassword { get; set; }
}
