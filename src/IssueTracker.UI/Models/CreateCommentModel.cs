using System.ComponentModel.DataAnnotations;

namespace IssueTracker.UI.Models;

public class CreateCommentModel
{

	[Required] [MaxLength(500)] public string Comment { get; set; }
	
}