using System.ComponentModel.DataAnnotations;
namespace API.Dtos;
public class RegisterAdmiDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}
