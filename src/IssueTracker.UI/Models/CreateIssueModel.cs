using System.ComponentModel.DataAnnotations;

namespace IssueTracker.UI.Models;

public class CreateIssueModel
{
	[Required] [MaxLength(75)] public string Issue { get; set; }
	
	[Required]
	[MinLength(1)]
	[Display(Name = "Category")]
	public string CategoryId { get; set; }

	[Required] [MaxLength(500)] public string Description { get; set; }
}