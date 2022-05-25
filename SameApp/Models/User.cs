using System.ComponentModel.DataAnnotations;

namespace SameApp.Models;

public class User
{
    [Required (ErrorMessage = "Please enter a user name")]
    [Key]
    public string UserName { get; set; }

    [Required (ErrorMessage = "Please enter a password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public ICollection<Contact> Contacts { get; set; }
    
}
