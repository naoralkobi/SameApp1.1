using System.ComponentModel.DataAnnotations;

namespace SameApp.Models;

public class Message
{	
	[Key]
	public int Id { get; set; }
	
	public string Content { get; set; }
	//time
	public string Created { get; set; }
	// who sent the message
	public bool Sent { get; set; }
	
	public string UserId { get; set; }
    
	public string ContactId { get; set; }
	
	public Contact Contact { get; set; }
}