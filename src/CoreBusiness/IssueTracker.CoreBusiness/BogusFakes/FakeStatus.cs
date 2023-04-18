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

	private static Faker<StatusModel>? _statusGenerator;

	private static void SetupGenerator()
	{

		Randomizer.Seed = new Random(123);

		_statusGenerator = new Faker<StatusModel>()
				.RuleFor(x => x.Id, Guid.NewGuid().ToString)
				.RuleFor(x => x.StatusName, f => f.PickRandom<Status>().ToString())
				.RuleFor(x => x.StatusDescription, f => f.Lorem.Sentence());

	}

	/// <summary>
	/// Gets a new status.
	/// </summary>
	/// <returns>StatusModel</returns>
	public static StatusModel GetNewStatus()
	{

		SetupGenerator();

		var status = _statusGenerator!.Generate();

		status.Id = string.Empty;

		return status;

	}

	/// <summary>
	/// Gets a list of statuses.
	/// </summary>
	/// <param name="numberOfStatuses">The number of statuses.</param>
	/// <returns>IEnumerable List of StatusModels</returns>
	public static IEnumerable<StatusModel> GetStatuses(int numberOfStatuses)
	{

		SetupGenerator();

		var statuses = _statusGenerator!.Generate(numberOfStatuses);

		return statuses;

	}

	/// <summary>
	/// Gets a list of basic statuses.
	/// </summary>
	/// <param name="numberOfStatuses">The number of statuses.</param>
	/// <returns>IEnumerable List of BasicStatusModels</returns>
	public static IEnumerable<BasicStatusModel> GetBasicStatuses(int numberOfStatuses)
	{

		SetupGenerator();

		var statuses = _statusGenerator!.Generate(numberOfStatuses);

		var basicStatuses = statuses.Select(s => new BasicStatusModel(s));

		return basicStatuses;

	}
}
