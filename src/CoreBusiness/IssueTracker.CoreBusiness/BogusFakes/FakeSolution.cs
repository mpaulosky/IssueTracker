//-----------------------------------------------------------------------
// <copyright>
//	File:		FakeSolution.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
/// FakeSolution class
/// </summary>
public static class FakeSolution
{

	private static Faker<SolutionModel>? _solutionGenerator;

	private static void SetupGenerator()
	{

		Randomizer.Seed = new Random(123);

		_solutionGenerator = new Faker<SolutionModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString)
			.RuleFor(f => f.Title, f => f.Lorem.Sentence())
			.RuleFor(f => f.Description, f => f.Lorem.Paragraph())
			.RuleFor(f => f.DateCreated, f => f.Date.Past())
			.RuleFor(f => f.Issue, FakeIssue.GetBasicIssues(1).First())
			.RuleFor(f => f.Author, FakeUser.GetBasicUser(1).First())
			.RuleFor(f => f.Archived, f => f.Random.Bool());

	}

	/// <summary>
	/// Gets a new solution.
	/// </summary>
	/// <returns>SolutionModel</returns>
	public static SolutionModel GetNewSolution()
	{

		SetupGenerator();

		var solution = _solutionGenerator!.Generate();

		solution.Id = string.Empty;

		return solution;

	}

	/// <summary>
	/// Gets a list of solutions.
	/// </summary>
	/// <param name="numberOfSolutions">The number of solutions.</param>
	/// <returns>IEnumerable List of SolutionModel</returns>
	public static IEnumerable<SolutionModel> GetSolutions(int numberOfSolutions)
	{

		SetupGenerator();

		var solutions = _solutionGenerator!.Generate(numberOfSolutions);

		return solutions;

	}

	/// <summary>
	/// Gets a list of basic solutions.
	/// </summary>
	/// <param name="numberOfSolutions">The number of solutions.</param>
	/// <returns>IEnumerable List of BasicSolutionModel</returns>
	public static IEnumerable<BasicSolutionModel> GetBasicSolutions(int numberOfSolutions)
	{

		SetupGenerator();

		var solutions = _solutionGenerator!.Generate(numberOfSolutions);

		var basicSolutions = solutions.Select(c => new BasicSolutionModel(c));

		return basicSolutions;

	}

}
