// Copyright (c) 2023. All rights reserved.
// File Name :     SolutionRepository.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns

namespace IssueTracker.PlugIns.DataAccess;

/// <summary>
///   SolutionRepository class
/// </summary>
public class SolutionRepository : ISolutionRepository
{
	private readonly IMongoCollection<SolutionModel> _solutionCollection;

	/// <summary>
	///   SolutionRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public SolutionRepository(IMongoDbContextFactory context)
	{
		ArgumentNullException.ThrowIfNull(context);

		string solutionCollectionName = GetCollectionName(nameof(SolutionModel));

		_solutionCollection = context.GetCollection<SolutionModel>(solutionCollectionName);
	}

	/// <summary>
	///   ArchiveAsync method
	/// </summary>
	/// <param name="solution">SolutionModel</param>
	/// <returns>Task</returns>
	public async Task ArchiveAsync(SolutionModel solution)
	{
		// Archive the category
		solution.Archived = true;

		await UpdateAsync(solution.Id, solution);
	}

	/// <summary>
	///   CreateAsync method
	/// </summary>
	/// <param name="solution">SolutionModel</param>
	/// <exception cref="Exception"></exception>
	public async Task CreateAsync(SolutionModel solution)
	{
		await _solutionCollection.InsertOneAsync(solution);
	}

	/// <summary>
	///   GetAsync method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <returns>Task of SolutionModel</returns>
	public async Task<SolutionModel> GetAsync(string itemId)
	{
		ObjectId objectId = new(itemId);

		FilterDefinition<SolutionModel>? filter = Builders<SolutionModel>.Filter.Eq("_id", objectId);

		SolutionModel? result = (await _solutionCollection.FindAsync(filter)).FirstOrDefault();

		return result;
	}

	/// <summary>
	///   GetAllAsync method
	/// </summary>
	/// <returns>Task of IEnumerable SolutionModel</returns>
	public async Task<IEnumerable<SolutionModel>> GetAllAsync()
	{
		FilterDefinition<SolutionModel>? filter = Builders<SolutionModel>.Filter.Empty;

		List<SolutionModel>? results = (await _solutionCollection.FindAsync(filter)).ToList();

		return results;
	}

	/// <summary>
	///   GetByUserAsync method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of IEnumerable SolutionModel</returns>
	public async Task<IEnumerable<SolutionModel>> GetByUserAsync(string userId)
	{
		List<SolutionModel>? results = (await _solutionCollection.FindAsync(s => s.Author.Id == userId)).ToList();

		return results;
	}

	/// <summary>
	///   GetByIssueAsync method
	/// </summary>
	/// <param name="issueId">string</param>
	/// <returns>Task of IEnumerable SolutionModel</returns>
	public async Task<IEnumerable<SolutionModel>> GetByIssueAsync(string issueId)
	{
		List<SolutionModel>? results = (await _solutionCollection.FindAsync(s => s.Issue.Id == issueId)).ToList();

		return results;
	}

	/// <summary>
	///   UpdateSolution method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <param name="solution">SolutionModel</param>
	public async Task UpdateAsync(string itemId, SolutionModel solution)
	{
		ObjectId objectId = new(itemId);

		FilterDefinition<SolutionModel>? filter = Builders<SolutionModel>.Filter.Eq("_id", objectId);

		await _solutionCollection.ReplaceOneAsync(filter, solution);
	}
}