using System.ComponentModel.DataAnnotations;

namespace IssueTracker.UI.Models;

public class CreateIssueModel
{
	public CreateIssueModel(string issue, string description)
	{
		Issue = issue ?? throw new ArgumentNullException(nameof(issue));
		Description = description ?? throw new ArgumentNullException(nameof(description));
	}

	[Required]
	[MaxLength(75)]
	public string Issue { get; }

	[Required]
	[MaxLength(500)]
	public string Description { get; }
}