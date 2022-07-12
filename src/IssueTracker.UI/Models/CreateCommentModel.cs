using System.ComponentModel.DataAnnotations;

namespace IssueTracker.UI.Models;

public class CreateCommentModel
{
	public CreateCommentModel(string comment, string description)
	{
		Comment = comment ?? throw new ArgumentNullException(nameof(comment));
		Description = description ?? throw new ArgumentNullException(nameof(description));
	}

	[Required] [MaxLength(100)] public string Comment { get; }

	[Required] [MaxLength(500)] public string Description { get; init; }
}