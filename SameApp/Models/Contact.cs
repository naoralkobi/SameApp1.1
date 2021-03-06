using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace SameApp.Models;

public class Contact
{
    [Key]
    // this is the real User Name of this Contact 
    public string Id { get; set; }
    
    // this is the User that has this contact in his list.
    public string UserNameOwner { get; set; }
    
    // this is the Display Name
    public string Name { get; set; }
    
    public string Server { get; set; }
    
    public string Last { get; set; }
    
    public string LastDate { get; set; }
    [JsonIgnore]
    public ICollection<Message> Messages { get; set; }
    [JsonIgnore]
    public User User { get; set; }
}