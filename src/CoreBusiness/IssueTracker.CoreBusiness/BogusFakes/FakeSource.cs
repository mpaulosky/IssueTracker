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
	private static Faker<BasicCommentOnSourceModel>? _sourceGenerator;

	private static void SetupGenerator()
	{
		Randomizer.Seed = new Random(123);

		_sourceGenerator = new Faker<BasicCommentOnSourceModel>()
			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(x => x.SourceType, f => f.PickRandom<SourceType>().ToString())
			.RuleFor(x => x.Title, f => f.Lorem.Sentence(10, 4))
			.RuleFor(x => x.Description, f => f.Lorem.Paragraph())
			.RuleFor(f => f.Author, FakeUser.GetBasicUser(1).First());
	}

	/// <summary>
	///   Gets a basic source.
	/// </summary>
	/// <returns>BasicCommentSourceModel</returns>
	public static BasicCommentOnSourceModel GetSource()
	{
		SetupGenerator();

		BasicCommentOnSourceModel? source = _sourceGenerator!.Generate();

		return source;
	}
}