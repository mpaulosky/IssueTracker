﻿// Copyright (c) 2023. All rights reserved.
// File Name :     FakeSolution.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness

namespace IssueTracker.CoreBusiness.BogusFakes;













/// <summary>///   FakeSolution class/// </summary>                                                                                                            public static class FakeSolution{	private static Faker<SolutionModel>? _solutionGenerator;	private static void SetupGenerator()	{		Randomizer.Seed = new Random(123);		_solutionGenerator = new Faker<SolutionModel>()			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())			.RuleFor(f => f.Title, f => f.Lorem.Sentence())			.RuleFor(f => f.Description, f => f.Lorem.Paragraph())			.RuleFor(f => f.DateCreated, f => f.Date.Past())			.RuleFor(f => f.Issue, FakeIssue.GetBasicIssues(1).First())			.RuleFor(f => f.Author, FakeUser.GetBasicUser(1).First())			.RuleFor(f => f.Archived, f => f.Random.Bool());	}











	/// <summary>	///   Gets a new solution.	/// </summary>	/// <param name="keepId">bool whether to keep the generated Id</param>	/// <returns>SolutionModel</returns>                                                                                                                                                                             public static SolutionModel GetNewSolution(bool keepId = false)	{		SetupGenerator();		SolutionModel? solution = _solutionGenerator!.Generate();
		if (!keepId)		{			solution.Id = string.Empty;		}		solution.Archived = false;		return solution;	}











	/// <summary>	///   Gets a list of solutions.	/// </summary>	/// <param name="numberOfSolutions">The number of solutions.</param>	/// <returns>IEnumerable List of SolutionModel</returns>                                                                                                                                                                                                     public static IEnumerable<SolutionModel> GetSolutions(int numberOfSolutions)	{		SetupGenerator();		List<SolutionModel>? solutions = _solutionGenerator!.Generate(numberOfSolutions);
		foreach (SolutionModel? item in solutions.Where(x => x.Archived))
		{			item.ArchivedBy = new BasicUserModel(FakeUser.GetNewUser());		}		return solutions;	}











	/// <summary>	///   Gets a list of basic solutions.	/// </summary>	/// <param name="numberOfSolutions">The number of solutions.</param>	/// <returns>IEnumerable List of BasicSolutionModel</returns>                                                                                                                                                                                                                public static IEnumerable<BasicSolutionModel> GetBasicSolutions(int numberOfSolutions)	{		SetupGenerator();		IEnumerable<SolutionModel> solutions = GetSolutions(numberOfSolutions);
		IEnumerable<BasicSolutionModel> basicSolutions =
			solutions.Select(c => new BasicSolutionModel(c));		return basicSolutions;	}}