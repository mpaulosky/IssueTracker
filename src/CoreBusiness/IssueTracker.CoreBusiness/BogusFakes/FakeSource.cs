// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeSource.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness
// =============================================

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
///   FakeSource class
/// </summary>
public static class FakeSource
{
	/// <summary>
	///   Gets a basic source.
	/// </summary>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>BasicCommentSourceModel</returns>
	public static BasicCommentOnSourceModel GetSource(bool useNewSeed = false)
	{
		var source = GenerateFake(useNewSeed).Generate();

		return source;
	}

	/// <summary>
	/// GenerateFake method
	/// </summary>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A Faker BasicCommentOnSourceModel</returns>
	private static Faker<BasicCommentOnSourceModel> GenerateFake(bool useNewSeed = false)
	{
		var seed = 0;
		if (useNewSeed)
		{
			seed = Random.Shared.Next(10, int.MaxValue);
		}

		return new Faker<BasicCommentOnSourceModel>()
			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(x => x.SourceType, f => f.PickRandom<SourceType>().ToString())
			.RuleFor(x => x.Title, f => f.Lorem.Sentence(10, 4))
			.RuleFor(x => x.Description, f => f.Lorem.Paragraph())
			.RuleFor(f => f.Author, FakeUser.GetBasicUser(1).First())
			.UseSeed(seed);
	}
}