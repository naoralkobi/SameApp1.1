using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SameApp.Models;

public class User
{
    [Required (ErrorMessage = "Please enter a user name")]
    [Key]
    public string UserName { get; set; }

    [Required (ErrorMessage = "Please enter a password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [JsonIgnore]
    public ICollection<Contact> Contacts { get; set; }
    
}
