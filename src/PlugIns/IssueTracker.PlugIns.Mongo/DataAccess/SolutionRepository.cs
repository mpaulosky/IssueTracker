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
	/// ArchiveAsync method
	/// </summary>
	/// <param name="solution">SolutionModel</param>
	public async Task ArchiveAsync(SolutionModel solution)
	{

		var objectId = new ObjectId(solution.Id);

		FilterDefinition<SolutionModel> filter =
			Builders<SolutionModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter, solution);

	}
	
	/// <summary>
	/// Creates the solution asynchronous.
	/// </summary>
	/// <param name="solution">The solution.</param>
	public async Task CreateAsync(SolutionModel solution)
	{

		await _collection.InsertOneAsync(solution);

	}

	/// <summary>
	/// GetAsync method
	/// </summary>
	/// <param name="solutionId"></param>
	/// <returns>SolutionModel</returns>
	public async Task<SolutionModel?> GetAsync(string solutionId)
	{

		return (await _collection
				.FindAsync(s=> s.Id == solutionId && s.Archived == false))
			.FirstOrDefault();

	}

	/// <summary>
	/// GetSolutionByIssueIdAsync method
	/// </summary>
	/// <param name="issueId"></param>
	/// <returns>Task of SolutionModel</returns>
	public async Task<IEnumerable<SolutionModel>?> GetByIssueAsync(string issueId)
	{
		
		return (await _collection
			.FindAsync(s=> s.Issue.Id == issueId && s.Archived == false))
			.ToList();
		
	}

	/// <summary>
	/// GetAllAsync method
	/// </summary>
	/// <returns>Task of IEnumerable SolutionModel</returns>
	public async Task<IEnumerable<SolutionModel>?> GetAllAsync(bool includeArchived = false)
	{

		if (includeArchived)
		{
			
			var filter = Builders<SolutionModel>.Filter.Empty;
			return (await _collection
					.FindAsync(filter))
				.ToList();

		}
		else
		{

			return (await _collection
					.FindAsync(x => x.Archived == includeArchived))
				.ToList();
			
		}
		
	}

	/// <summary>
	/// GetSolutionsByUserIdAsync method
	/// </summary>
	/// <param name="userId">string user Id</param>
	/// <returns>Task of IEnumerable SolutionModel</returns>
	public async Task<IEnumerable<SolutionModel>?> GetByUserAsync(string userId)
	{

		return (await _collection
			.FindAsync(s=> s.Author.Id == userId && s.Archived == false))
			.ToList();

	}

	/// <summary>
	/// UpdateSolutionAsync method
	/// </summary>
	/// <param name="solution">SolutionModel</param>
	public async Task UpdateAsync(SolutionModel solution)
	{

		var objectId = new ObjectId(solution.Id);

		FilterDefinition<SolutionModel> filter =
			Builders<SolutionModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter, solution);

	}

}
