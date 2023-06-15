// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeSolution.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness
// =============================================

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
///   FakeSolution class
/// </summary>
public static class FakeSolution
{
	/// <summary>
	///   Gets a new solution.
	/// </summary>
	/// <param name="keepId">bool whether to keep the generated Id</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>SolutionModel</returns>
	public static SolutionModel GetNewSolution(bool keepId = false, bool useNewSeed = false)
	{
		var solution = GenerateFake(useNewSeed).Generate();

		if (!keepId)
		{
			solution.Id = string.Empty;
		}

		solution.Archived = false;

		return solution;
	}

	/// <summary>
	///   Gets a list of solutions.
	/// </summary>
	/// <param name="numberOfSolutions">The number of solutions.</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A List of SolutionModel</returns>
	public static List<SolutionModel> GetSolutions(int numberOfSolutions, bool useNewSeed = false)
	{
		var solutions = GenerateFake(useNewSeed).Generate(numberOfSolutions);

		foreach (var solution in solutions.Where(x => x.Archived))
		{
			solution.ArchivedBy = new BasicUserModel(FakeUser.GetNewUser());
		}

		return solutions;
	}

	/// <summary>
	///   Gets a list of basic solutions.
	/// </summary>
	/// <param name="numberOfSolutions">The number of solutions.</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A List of BasicSolutionModel</returns>
	public static List<BasicSolutionModel> GetBasicSolutions(int numberOfSolutions, bool useNewSeed = false)
	{
		var solutions = GenerateFake(useNewSeed).Generate(numberOfSolutions);

		return solutions.Select(c => new BasicSolutionModel(c)).ToList();
	}

	/// <summary>
	/// GenerateFake method
	/// </summary>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A Faker SolutionModel</returns>
	private static Faker<SolutionModel> GenerateFake(bool useNewSeed = false)
	{
		var seed = 0;
		if (useNewSeed)
		{
			seed = Random.Shared.Next(10, int.MaxValue);
		}

		return new Faker<SolutionModel>()
			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(f => f.Title, f => f.Lorem.Sentence())
			.RuleFor(f => f.Description, f => f.Lorem.Paragraph())
			.RuleFor(f => f.DateCreated, f => f.Date.Past())
			.RuleFor(f => f.Issue, FakeIssue.GetBasicIssues(1).First())
			.RuleFor(f => f.Author, FakeUser.GetBasicUser(1).First())
			.RuleFor(f => f.Archived, f => f.Random.Bool())
			.UseSeed(seed);
	}
}