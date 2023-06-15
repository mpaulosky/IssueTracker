// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeStatus.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness
// =============================================

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
///   FakeStatus class
/// </summary>
public static class FakeStatus
{
	/// <summary>
	///   Gets a new status.
	/// </summary>
	/// <param name="keepId">bool whether to keep the generated Id</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>StatusModel</returns>
	public static StatusModel GetNewStatus(bool keepId = false, bool useNewSeed = false)
	{
		var status = GenerateFake(useNewSeed).Generate();

		if (!keepId)
		{
			status.Id = string.Empty;
		}

		status.Archived = false;

		return status;
	}

	/// <summary>
	///   Gets a list of statuses that exist.
	/// </summary>
	/// <returns>IEnumerable List of StatusModels</returns>
	public static List<StatusModel> GetStatuses()
	{
		List<StatusModel> statuses = new()
		{
			new StatusModel
			{
				Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString(),
				StatusName = "Answered",
				StatusDescription = "The issue was accepted and the corresponding item was created.",
				Archived = false
			},
			new StatusModel
			{
				Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString(),
				StatusName = "Watching",
				StatusDescription =
					"The issue is interesting. We are watching to see how much interest there is in it.",
				Archived = false
			},
			new StatusModel
			{
				Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString(),
				StatusName = "InWork",
				StatusDescription = "The issue was accepted and it is in work.",
				Archived = false
			},
			new StatusModel
			{
				Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString(),
				StatusName = "Dismissed",
				StatusDescription = "The issue was not something that we are going to undertake.",
				Archived = false
			}
		};

		return statuses;
	}

	/// <summary>
	///   Gets a list of statuses.
	/// </summary>
	/// <param name="numberOfStatuses">The number of statuses.</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A List of StatusModels</returns>
	public static IEnumerable<StatusModel> GetStatuses(int numberOfStatuses, bool useNewSeed = false)
	{
		var statuses = GenerateFake(useNewSeed).Generate(numberOfStatuses);

		foreach (var status in statuses.Where(x => x.Archived))
		{
			status.ArchivedBy = new BasicUserModel(FakeUser.GetNewUser());
		}

		return statuses;
	}

	/// <summary>
	///   Gets a list of basic statuses.
	/// </summary>
	/// <param name="numberOfStatuses">The number of statuses.</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A List of BasicStatusModels</returns>
	public static List<BasicStatusModel> GetBasicStatuses(int numberOfStatuses, bool useNewSeed = false)
	{
		var statuses = GenerateFake(useNewSeed).Generate(numberOfStatuses);

		return statuses.Select(s => new BasicStatusModel(s)).ToList();
	}

	/// <summary>
	/// GenerateFake method
	/// </summary>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A Faker StatusModel</returns>
	private static Faker<StatusModel> GenerateFake(bool useNewSeed = false)
	{
		var seed = 0;
		if (useNewSeed)
		{
			seed = Random.Shared.Next(10, int.MaxValue);
		}

		return new Faker<StatusModel>()
			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(x => x.StatusName, f => f.PickRandom<Status>().ToString())
			.RuleFor(x => x.StatusDescription, f => f.Lorem.Sentence())
			.RuleFor(f => f.Archived, f => f.Random.Bool())
			.UseSeed(seed);
	}
}