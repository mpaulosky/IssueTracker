//-----------------------------------------------------------------------
// <copyright>
//	File:		SolutionRepository.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.Mongo.DataAccess;

/// <summary>
/// SolutionRepository class
/// </summary>
public class SolutionRepository : ISolutionRepository
{
	private const bool FalseValue = false;
	private readonly IMongoCollection<SolutionModel> _collection;

	/// <summary>
	/// SolutionRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContextFactory</param>
	/// <exception cref="ArgumentNullException"></exception>
	public SolutionRepository(IMongoDbContextFactory context)
	{

		Guard.Against.Null(context, nameof(context));

		var collectionName = GetCollectionName(nameof(SolutionModel));

		_collection = context.GetCollection<SolutionModel>(collectionName);

	}

	/// <summary>
	/// Creates the solution asynchronous.
	/// </summary>
	/// <param name="solution">The solution.</param>
	public async Task CreateSolutionAsync(SolutionModel solution)
	{

		await _collection.InsertOneAsync(solution);

	}

	public Task<SolutionModel?> GetSolution(string solutionId)
	{

		var queryableCollection = _collection.AsQueryable();

		var result = queryableCollection
			.Where(s => s.Id == solutionId && s.Archived == FalseValue);

		return Task.FromResult(result.FirstOrDefault());

	}

	/// <summary>
	/// GetSolutionByIssueIdAsync method
	/// </summary>
	/// <param name="issueId"></param>
	/// <returns>Task of SolutionModel</returns>
	public async Task<IEnumerable<SolutionModel>> GetSolutionsByIssueIdAsync(string issueId)
	{

		var queryableCollection = _collection.AsQueryable();

		var results = queryableCollection
			.Where(s => s.Issue.Id == issueId && s.Archived == FalseValue)
			.OrderByDescending(o => o.DateCreated.Date)
			.ToList();

		return (IEnumerable<SolutionModel>)results;

	}

	/// <summary>
	/// GetSolutionsAsync method
	/// </summary>
	/// <returns>Task of IEnumerable SolutionModel</returns>
	public async Task<IEnumerable<SolutionModel>> GetSolutionsAsync()
	{

		var queryableCollection = _collection.AsQueryable();

		var results = queryableCollection
			.Where(x => x.Archived == FalseValue)
			.OrderBy(o => o.DateCreated.Date)
			.ToList();

		return (IEnumerable<SolutionModel>)results;

	}

	/// <summary>
	/// GetSolutionsByUserIdAsync method
	/// </summary>
	/// <param name="userId">string user Id</param>
	/// <returns>Task of IEnumerable SolutionModel</returns>
	public async Task<IEnumerable<SolutionModel>> GetSolutionsByUserIdAsync(string userId)
	{

		var queryableCollection = _collection.AsQueryable();

		var results = queryableCollection
			.Where(s => s.Author.Id == userId && s.Archived == FalseValue)
			.OrderByDescending(o => o.DateCreated.Date)
			.ToList();

		return (IEnumerable<SolutionModel>)results;

	}

	/// <summary>
	/// UpdateSolutionAsync method
	/// </summary>
	/// <param name="solution">SolutionModel</param>
	public async Task UpdateSolutionAsync(SolutionModel solution)
	{

		var objectId = new ObjectId(solution.Id);

		FilterDefinition<SolutionModel> filter =
			Builders<SolutionModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter, solution);

	}

}
