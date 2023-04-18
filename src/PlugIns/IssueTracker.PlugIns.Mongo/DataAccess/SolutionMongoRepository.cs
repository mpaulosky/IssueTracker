//-----------------------------------------------------------------------
// <copyright>
//	File:		SolutionMongoRepository.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.Mongo.DataAccess;

/// <summary>
/// SolutionRepository class
/// </summary>
public class SolutionMongoRepository : ISolutionRepository
{

	private readonly IMongoCollection<SolutionModel> _collection;

	/// <summary>
	/// SolutionRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContextFactory</param>
	/// <exception cref="ArgumentNullException"></exception>
	public SolutionMongoRepository(IMongoDbContextFactory context)
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

	/// <summary>
	/// GetSolutionByIssueIdAsync method
	/// </summary>
	/// <param name="issueId"></param>
	/// <returns>Task of SolutionModel</returns>
	public async Task<SolutionModel> GetSolutionByIssueIdAsync(string issueId)
	{

		var objectId = new ObjectId(issueId);

		FilterDefinition<SolutionModel> filter = Builders<SolutionModel>.Filter.Eq("_id", objectId);

		SolutionModel result = (await _collection.FindAsync(filter)).FirstOrDefault();

		return result;

	}

	/// <summary>
	/// GetSolutionsAsync method
	/// </summary>
	/// <returns>Task of IEnumerable SolutionModel</returns>
	public async Task<IEnumerable<SolutionModel>> GetSolutionsAsync()
	{

		FilterDefinition<SolutionModel> filter = Builders<SolutionModel>.Filter.Empty;

		var result = (await _collection.FindAsync(filter)).ToList();

		return result;

	}

	/// <summary>
	/// GetSolutionsByUserIdAsync method
	/// </summary>
	/// <param name="userId">string user Id</param>
	/// <returns>Task of IEnumerable SolutionModel</returns>
	public async Task<IEnumerable<SolutionModel>> GetSolutionsByUserIdAsync(string userId)
	{

		var results = (await _collection.FindAsync(s => s.Author.Id == userId)).ToList();

		return results;

	}

	/// <summary>
	/// UpdateSolutionAsync method
	/// </summary>
	/// <param name="solution">SolutionModel</param>
	public async Task UpdateSolutionAsync(SolutionModel solution)
	{

		var objectId = new ObjectId(solution.Id);

		FilterDefinition<SolutionModel> filter = Builders<SolutionModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter, solution);

	}

}
