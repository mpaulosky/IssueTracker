//-----------------------------------------------------------------------
// <copyright file="FakeStatus.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Helpers.BogusFakes;

public static class FakeStatus
{


	public static StatusModel GetNewStatus()
	{

		var statusGenerator = new Faker<StatusModel>()
		.RuleFor(x => x.StatusName, f => f.PickRandom<Status>().ToString())
		.RuleFor(x => x.StatusDescription, f => f.Lorem.Sentence());

		return statusGenerator.Generate();

	}


	public static IEnumerable<StatusModel> GetStatuses(int numberOfStatuses)
	{

		var statusGenerator = new Faker<StatusModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString)
		.RuleFor(x => x.StatusName, f => f.PickRandom<Status>().ToString())
		.RuleFor(x => x.StatusDescription, f => f.Lorem.Sentence());

		return statusGenerator.Generate(numberOfStatuses);

	}

	public static IEnumerable<BasicStatusModel> GetBasicStatuses(int numberOfStatuses)
	{

		var statusGenerator = new Faker<BasicStatusModel>()
		.RuleFor(x => x.StatusName, f => f.PickRandom<Status>().ToString())
		.RuleFor(x => x.StatusDescription, f => f.Lorem.Sentence());

		var basicStatuses = statusGenerator.Generate(numberOfStatuses);

		return basicStatuses;

	}
}

internal enum Status
{

	Answered,
	Watching,
	Dismissed,
	InWork

}