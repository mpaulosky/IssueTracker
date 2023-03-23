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

	/// <summary>
	/// Gets the new solution.
	/// </summary>
	/// <returns>SolutionModel</returns>
	public static SolutionModel GetNewSolution()
	{
		Faker<SolutionModel> solutionGenerator = new Faker<SolutionModel>()
		.RuleFor(x => x.Title, f => f.Lorem.Sentence())
		.RuleFor(x => x.Description, f => f.Lorem.Sentence());
		SolutionModel solution = solutionGenerator.Generate();
		return solution;
	}

}
