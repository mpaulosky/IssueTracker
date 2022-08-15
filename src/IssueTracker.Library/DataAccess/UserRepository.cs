﻿using static IssueTracker.Library.Helpers.CollectionNames;

namespace IssueTracker.Library.DataAccess;

/// <summary>
/// UserRepository class
/// </summary>
public class UserRepository : IUserRepository
{
	private readonly IMongoCollection<UserModel> _collection;

	/// <summary>
	/// UserRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	public UserRepository(IMongoDbContext context)
	{
		_collection = context.GetCollection<UserModel>(GetCollectionName(nameof(UserModel)));
	}

	/// <summary>
	/// GetUser method
	/// </summary>
	/// <param name="id">string</param>
	/// <returns>Task of UserModel</returns>
	public async Task<UserModel> GetUser(string id)
	{
		var objectId = new ObjectId(id);

		var filter = Builders<UserModel>.Filter.Eq("_id", objectId);

		var result = await _collection.FindAsync(filter);

		return result.FirstOrDefault();
	}

	/// <summary>
	/// GetUsers method
	/// </summary>
	/// <returns>Task of IEnumerable UserModel</returns>
	public async Task<IEnumerable<UserModel>> GetUsers()
	{
		var all = await _collection.FindAsync(Builders<UserModel>.Filter.Empty);

		return await all.ToListAsync();
	}

	/// <summary>
	/// CreateUser method
	/// </summary>
	/// <param name="user">UserModel</param>
	public async Task CreateUser(UserModel user)
	{
		await _collection.InsertOneAsync(user);
	}

	/// <summary>
	/// UpdateUser method
	/// </summary>
	/// <param name="id">string</param>
	/// <param name="user">UserModel</param>
	public async Task UpdateUser(string id, UserModel user)
	{
		await _collection.ReplaceOneAsync(Builders<UserModel>.Filter.Eq("_id", id), user);
	}

	/// <summary>
	/// GetUserFromAuthentication method
	/// </summary>
	/// <param name="objectId">string</param>
	/// <returns>Task of UserModel</returns>
	public async Task<UserModel> GetUserFromAuthentication(string objectId)
	{
		var results = await _collection.FindAsync(u => u.ObjectIdentifier == objectId);

		return results.FirstOrDefault();
	}
}