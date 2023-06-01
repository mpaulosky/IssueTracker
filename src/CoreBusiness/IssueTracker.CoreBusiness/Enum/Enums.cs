namespace IssueTracker.CoreBusiness.Enum;

public class Enums
{
	/// <summary>
	///   Category enum
	/// </summary>
	internal enum Category
	{
		Design,
		Documentation,
		Implementation,
		Clarification,
		Miscellaneous
	}

	/// <summary>
	///   Status enum
	/// </summary>
	internal enum Status
	{
		Answered,
		Watching,
		Dismissed,
		InWork
	}

	/// <summary>
	///   SourceType enum
	/// </summary>
	internal enum SourceType
	{
		Comment,
		Issue,
		Solution
	}
}
