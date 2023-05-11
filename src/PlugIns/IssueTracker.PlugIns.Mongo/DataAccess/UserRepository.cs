//-----------------------------------------------------------------------
// <copyright>
//	File:		UserMongoRepository.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.Mongo.DataAccess;

/// <summary>
///		UserRepository class
/// </summary>
public class UserRepository : IUserRepository
{

	private readonly IMongoCollection<UserModel> _collection;

	/// <summary>
	///		UserRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public UserRepository(IMongoDbContextFactory context)
	{

		Guard.Against.Null(context, nameof(context));

		var collectionName = GetCollectionName(nameof(UserModel));

		_collection = context.GetCollection<UserModel>(collectionName);

	}

	///  <summary>
	/// 		ArchiveAsync method
	///  </summary>
	///  <param name="user">UserModel</param>
	public async Task ArchiveAsync(UserModel user)
	{
		var objectId = new ObjectId(user.Id);

		FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter!, user);

	}

	/// <summary>
	///		CreateAsync method
	/// </summary>
	/// <param name="user">UserModel</param>
	public async Task CreateAsync(UserModel user)
	{

		await _collection.InsertOneAsync(user);

	}

	/// <summary>
	///		GetAsync method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of UserModel</returns>
	public async Task<UserModel?> GetAsync(string userId)
	{

		return (await _collection
			.FindAsync(x => x.Id == userId && x.Archived == false))
			.FirstOrDefault();

	}

	/// <summary>
	///		GetAllAsync method
	/// </summary>
	/// <returns>Task of IEnumerable UserModel</returns>
	public async Task<IEnumerable<UserModel>?> GetAllAsync(bool includeArchived = false)
	{

		if (includeArchived)
		{

			var filter = Builders<UserModel>.Filter.Empty;
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
	/// 		UpdateAsync method
	///  </summary>
	///  <param name="user">UserModel</param>
	public async Task UpdateAsync(UserModel user)
	{
		var objectId = new ObjectId(user.Id);

		FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter!, user);

	}

	/// <summary>
	///		GetUserFromAuthentication method
	/// </summary>
	/// <param name="userObjectIdentifierId">string</param>
	/// <returns>Task of UserModel</returns>
	public async Task<UserModel?> GetByAuthenticationIdAsync(string userObjectIdentifierId)
	{

		FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Eq("object_identifier", userObjectIdentifierId);

		UserModel result = (await _collection.FindAsync(filter)).FirstOrDefault();

		return result;

	}

}
