//-----------------------------------------------------------------------
// <copyright>
//	File:		FakeStatus.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
/// FakeStatus class
/// </summary>
public static class FakeStatus
{

	/// <summary>
	/// Gets the new status.
	/// </summary>
	/// <returns>StatusModel</returns>
	public static StatusModel GetNewStatus()
	{

		Faker<StatusModel> statusGenerator = new Faker<StatusModel>()
		.RuleFor(x => x.StatusName, f => f.PickRandom<Status>().ToString())
		.RuleFor(x => x.StatusDescription, f => f.Lorem.Sentence());

		return statusGenerator.Generate();

	}

	/// <summary>
	/// Gets the statuses.
	/// </summary>
	/// <param name="numberOfStatuses">The number of statuses.</param>
	/// <returns>IEnumerable List of StatusModels</returns>
	public static IEnumerable<StatusModel> GetStatuses(int numberOfStatuses)
	{

		Faker<StatusModel> statusGenerator = new Faker<StatusModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString)
		.RuleFor(x => x.StatusName, f => f.PickRandom<Status>().ToString())
		.RuleFor(x => x.StatusDescription, f => f.Lorem.Sentence());

		return statusGenerator.Generate(numberOfStatuses);

	}

	/// <summary>
	/// Gets the basic statuses.
	/// </summary>
	/// <param name="numberOfStatuses">The number of statuses.</param>
	/// <returns>IEnumerable List of BasicStatusModels</returns>
	public static IEnumerable<BasicStatusModel> GetBasicStatuses(int numberOfStatuses)
	{

		Faker<BasicStatusModel> statusGenerator = new Faker<BasicStatusModel>()
		.RuleFor(x => x.StatusName, f => f.PickRandom<Status>().ToString())
		.RuleFor(x => x.StatusDescription, f => f.Lorem.Sentence());

		List<BasicStatusModel> basicStatuses = statusGenerator.Generate(numberOfStatuses);

		return basicStatuses;

	}
}

/// <summary>
/// Status enum
/// </summary>
internal enum Status
{

	Answered,
	Watching,
	Dismissed,
	InWork

}