//-----------------------------------------------------------------------
// <copyright file="StatusRepository.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
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
	public StatusRepository(IMongoDbContext context)
	{
		Guard.Against.Null(context, nameof(context));

		string collectionName;
		collectionName = Guard.Against.NullOrWhiteSpace(GetCollectionName(nameof(StatusModel)), nameof(collectionName));
		
		_collection = context.GetCollection<StatusModel>(collectionName);
	}

	/// <summary>
	///   GetStatus method
	/// </summary>
	/// <param name="statusId">string</param>
	/// <returns>Task of StatusModel</returns>
	public async Task<StatusModel> GetStatus(string statusId)
	{
		var objectId = new ObjectId(statusId);

		var filter = Builders<StatusModel>.Filter.Eq("_id", objectId);

		var result = await _collection.FindAsync(filter).ConfigureAwait(true);

		return result.FirstOrDefault();
	}

	/// <summary>
	///   GetStatuses method
	/// </summary>
	/// <returns>Task of IEnumerable StatusModel</returns>
	public async Task<IEnumerable<StatusModel>> GetStatuses()
	{
		var all = await _collection.FindAsync(Builders<StatusModel>.Filter.Empty).ConfigureAwait(true);

		return await all.ToListAsync().ConfigureAwait(true);
	}

	/// <summary>
	///   CreateStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	public async Task CreateStatus(StatusModel status)
	{
		await _collection.InsertOneAsync(status).ConfigureAwait(true);
	}

	/// <summary>
	///   UpdateStatus method
	/// </summary>
	/// <param name="id">string</param>
	/// <param name="status">StatusModel</param>
	public async Task UpdateStatus(string id, StatusModel status)
	{
		await _collection.ReplaceOneAsync(Builders<StatusModel>.Filter.Eq("_id", id), status).ConfigureAwait(true);
	}
}