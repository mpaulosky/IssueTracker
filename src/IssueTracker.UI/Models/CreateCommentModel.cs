using System.ComponentModel.DataAnnotations;

namespace IssueTrackerUI.Models;

public class CreateCommentModel
{
	[Required]
	[MaxLength(100)]
	public string Comment { get; set; }
	
	[Required]
	[MaxLength(500)]
	public string Description { get; set; }
}