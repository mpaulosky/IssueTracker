﻿//-----------------------------------------------------------------------
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
				.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
				.RuleFor(x => x.StatusName, f => f.PickRandom<Status>().ToString())
				.RuleFor(x => x.StatusDescription, f => f.Lorem.Sentence())
				.RuleFor(f => f.Archived, f => f.Random.Bool());

	}

	/// <summary>
	/// Gets a new status.
	/// </summary>
	/// <param name="keepId">bool whether to keep the generated Id</param>
	/// <returns>StatusModel</returns>
	public static StatusModel GetNewStatus(bool keepId = false)
	{

		SetupGenerator();

		var status = _statusGenerator!.Generate();

		if (!keepId) status.Id = string.Empty;

		status.Archived = false;

		return status;

	}

	/// <summary>
	/// Gets a list of statuses that exist.
	/// </summary>
	/// <returns>IEnumerable List of StatusModels</returns>
	public static IEnumerable<StatusModel> GetStatuses()
	{

		var statuses = new List<StatusModel>
		{
			new()
			{
				Id = Guid.NewGuid().ToString(),
				StatusName = "Answered",
				StatusDescription = "The suggestion was accepted and the corresponding item was created.",
				Archived = false
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				StatusName = "Watching",
				StatusDescription =
					"The suggestion is interesting. We are watching to see how much interest there is in it.",
				Archived = false
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				StatusName = "In Work",
				StatusDescription = "The suggestion was accepted and it will be released soon.",
				Archived = false
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				StatusName = "Dismissed",
				StatusDescription = "The suggestion was not something that we are going to undertake.",
				Archived = false
			}
		};

		return statuses;

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
