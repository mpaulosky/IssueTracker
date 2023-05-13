//-----------------------------------------------------------------------
// <copyright>
//	File:		StatusMongoRepository.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
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

	///  <summary>
	/// 		ArchiveAsync method
	///  </summary>
	///  <param name="status">StatusModel</param>
	public async Task ArchiveAsync(StatusModel status)
	{

		// Archive the category
		status.Archived = true;

		await UpdateAsync(status);

	}

	/// <summary>
	///		CreateAsync method
	/// </summary>
	/// <param name="status">StatusModel</param>
	public async Task CreateAsync(StatusModel status)
	{

		await _collection.InsertOneAsync(status);

	}

	/// <summary>
	///		GetAsync method
	/// </summary>
	/// <param name="statusId">string</param>
	/// <returns>Task of StatusModel</returns>
	public async Task<StatusModel?> GetAsync(string statusId)
	{

		return (await _collection
				.FindAsync(s => s.Id == statusId && !s.Archived))
			.FirstOrDefault();


	}

	/// <summary>
	///		GetAllAsync method
	/// </summary>
	/// <returns>Task of IEnumerable StatusModel</returns>
	public async Task<IEnumerable<StatusModel>?> GetAllAsync(bool includeArchived = false)
	{

		if (includeArchived)
		{

			var filter = Builders<StatusModel>.Filter.Empty;
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

	///  <summary>
	/// 		UpdateStatus method
	///  </summary>
	///  <param name="status">StatusModel</param>
	public async Task UpdateAsync(StatusModel status)
	{

		var objectId = new ObjectId(status.Id);

		FilterDefinition<StatusModel> filter = Builders<StatusModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter, status);

	}

}
