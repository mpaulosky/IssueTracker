//-----------------------------------------------------------------------
// <copyright file="StatusRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.Mongo.DataAccess;

/// <summary>
///		StatusRepository class
/// </summary>
public class StatusRepository : IStatusRepository
{
	private readonly IMongoCollection<StatusModel> _collection;

	/// <summary>
	///		StatusRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public StatusRepository(IMongoDbContextFactory context)
	{

		Guard.Against.Null(context, nameof(context));

		var collectionName = GetCollectionName(nameof(StatusModel));

		_collection = context.GetCollection<StatusModel>(collectionName);

	}

	/// <summary>
	///		CreateStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	public async Task CreateStatusAsync(StatusModel status)
	{

		await _collection.InsertOneAsync(status);

	}

	/// <summary>
	///		GetStatus method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <returns>Task of StatusModel</returns>
	public async Task<StatusModel> GetStatusByIdAsync(string itemId)
	{

		var objectId = new ObjectId(itemId);

		FilterDefinition<StatusModel> filter = Builders<StatusModel>.Filter.Eq("_id", objectId);

		StatusModel result = (await _collection.FindAsync(filter)).FirstOrDefault();

		return result;

	}

	/// <summary>
	///		GetStatuses method
	/// </summary>
	/// <returns>Task of IEnumerable StatusModel</returns>
	public async Task<IEnumerable<StatusModel>> GetStatusesAsync()
	{

		FilterDefinition<StatusModel> filter = Builders<StatusModel>.Filter.Empty;

		var result = (await _collection.FindAsync(filter)).ToList();

		return result;

	}

	///  <summary>
	/// 		UpdateStatus method
	///  </summary>
	///  <param name="status">StatusModel</param>
	public async Task UpdateStatusAsync(StatusModel status)
	{

		var objectId = new ObjectId(status.Id);

		FilterDefinition<StatusModel> filter = Builders<StatusModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter, status);

	}

	/// <summary>
	///   DeleteStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	public async Task DeleteStatusAsync(StatusModel status)
	{

		var objectId = new ObjectId(status.Id);

		FilterDefinition<StatusModel> filter = Builders<StatusModel>.Filter.Eq("_id", objectId);

		await _collection.DeleteOneAsync(filter);

	}
}