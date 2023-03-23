//-----------------------------------------------------------------------
// <copyright>
//	File:		FakeSource.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
/// FakeSource class
/// </summary>
public static class FakeSource
{

	/// <summary>
	/// Gets the new source.
	/// </summary>
	/// <returns>BasicCommentSourceModel</returns>
	public static BasicCommentSourceModel GetNewSource()
	{

		Faker<BasicCommentSourceModel> sourceGenerator = new Faker<BasicCommentSourceModel>()
			.RuleFor(x => x.Id, f => f.Random.Guid().ToString())
			.RuleFor(x => x.Title, f => f.Lorem.Sentence(10, 4))
			.RuleFor(x => x.Description, f => f.Lorem.Paragraph())
			.RuleFor(f => f.Author, FakeUser.GetBasicUser(1).First());

		BasicCommentSourceModel source = sourceGenerator.Generate();

		return source;

	}

}
