//-----------------------------------------------------------------------// <copyright file="StatusRepository.cs" company="mpaulosky">//		Author:  Matthew Paulosky//		Copyright (c) 2022. All rights reserved.// </copyright>//-----------------------------------------------------------------------namespace IssueTracker.PlugIns.DataAccess;/// <summary>
///   StatusRepository class
/// </summary>
public class StatusRepository : IStatusRepository{	private readonly IMongoCollection<StatusModel> _collection;	/// <summary>
	///   StatusRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public StatusRepository(IMongoDbContextFactory context)
	{
		ArgumentNullException.ThrowIfNull(context);

		var collectionName = GetCollectionName(nameof(StatusModel));

		_collection = context.GetCollection<StatusModel>(collectionName);
	}	/// <summary>
	///   ArchiveStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	public async Task ArchiveAsync(StatusModel status)
	{		// Archive the category																status.Archived = true;

		await UpdateAsync(status.Id, status);
	}	/// <summary>
	///   CreateStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	public async Task CreateAsync(StatusModel status)
	{
		await _collection.InsertOneAsync(status);
	}	/// <summary>
	///   GetStatus method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <returns>Task of StatusModel</returns>
	public async Task<StatusModel> GetAsync(string itemId)
	{
		var objectId = new ObjectId(itemId);

		var filter = Builders<StatusModel>.Filter.Eq("_id", objectId);

		var result = (await _collection.FindAsync(filter)).FirstOrDefault();

		return result;
	}	/// <summary>
	///   GetStatuses method
	/// </summary>
	/// <returns>Task of IEnumerable StatusModel</returns>
	public async Task<IEnumerable<StatusModel>> GetAllAsync()
	{
		var filter = Builders<StatusModel>.Filter.Empty;

		var result = (await _collection.FindAsync(filter)).ToList();

		return result;
	}	/// <summary>
	///   UpdateStatus method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <param name="status">StatusModel</param>
	public async Task UpdateAsync(string itemId, StatusModel status)	{		var objectId = new ObjectId(itemId);		var filter = Builders<StatusModel>.Filter.Eq("_id", objectId);		await _collection.ReplaceOneAsync(filter, status);	}}
