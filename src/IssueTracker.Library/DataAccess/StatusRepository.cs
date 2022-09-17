//-----------------------------------------------------------------------
// <copyright file="StatusRepository.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.DataAccess;

/// <summary>
///   StatusRepository class
/// </summary>
public class StatusRepository : IStatusRepository
{
	private readonly IMongoCollection<StatusModel> _collection;

	/// <summary>
	///   StatusRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public StatusRepository(IMongoDbContextFactory context)
	{
		Guard.Against.Null(context, nameof(context));
		
		string collectionName = GetCollectionName(nameof(StatusModel));

		_collection = context.GetCollection<StatusModel>(collectionName);
	}

	/// <summary>
	///   GetStatus method
	/// </summary>
	/// <param name="statusId">string</param>
	/// <returns>Task of StatusModel</returns>
	public async Task<StatusModel> GetStatus(string statusId)
	{
		var filter = Builders<StatusModel>.Filter.Eq("_id", statusId);

		var result = (await _collection.FindAsync(filter)).FirstOrDefault();

		return result;
	}

	/// <summary>
	///   GetStatuses method
	/// </summary>
	/// <returns>Task of IEnumerable StatusModel</returns>
	public async Task<IEnumerable<StatusModel>> GetStatuses()
	{
		var filter = Builders<StatusModel>.Filter.Empty;
		
		var result = (await _collection.FindAsync(filter)).ToList();

		return result;
	}

	/// <summary>
	///   CreateStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	public async Task CreateStatus(StatusModel status)
	{
		await _collection.InsertOneAsync(status);
	}

	/// <summary>
	///   UpdateStatus method
	/// </summary>
	/// <param name="id">string</param>
	/// <param name="status">StatusModel</param>
	public async Task UpdateStatus(string id, StatusModel status)
	{
		var filter = Builders<StatusModel>.Filter.Eq("_id", id);

		await _collection.ReplaceOneAsync(filter, status);
	}
}