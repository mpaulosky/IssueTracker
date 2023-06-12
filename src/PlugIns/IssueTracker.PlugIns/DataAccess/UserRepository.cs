// Copyright (c) 2023. All rights reserved.
// File Name :     UserRepository.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns

namespace IssueTracker.PlugIns.DataAccess;

/// <summary>
///   UserRepository class
/// </summary>
public class UserRepository : IUserRepository
{
	private readonly IMongoCollection<UserModel> _collection;


	/// <summary>
	///   UserRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public UserRepository(IMongoDbContextFactory context)
	{
		ArgumentNullException.ThrowIfNull(context);

		string collectionName = GetCollectionName(nameof(UserModel));

		_collection = context.GetCollection<UserModel>(collectionName);
	}


	/// <summary>
	///   Archive User method
	/// </summary>
	/// <param name="user">UserModel</param>
	/// <returns>Task</returns>
	public async Task ArchiveAsync(UserModel user)
	{
		// Archive the category
		user.Archived = true;

		await UpdateAsync(user.Id, user);
	}


	/// <summary>
	///   CreateUser method
	/// </summary>
	/// <param name="user">UserModel</param>
	public async Task CreateAsync(UserModel user)
	{
		await _collection.InsertOneAsync(user);
	}


	/// <summary>
	///   GetUser method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <returns>Task of UserModel</returns>
	public async Task<UserModel> GetAsync(string itemId)
	{
		ObjectId objectId = new ObjectId(itemId);

		FilterDefinition<UserModel>? filter = Builders<UserModel>.Filter.Eq("_id", objectId);

		UserModel? result = (await _collection.FindAsync(filter)).FirstOrDefault();

		return result;
	}


	/// <summary>
	///   GetUsers method
	/// </summary>
	/// <returns>Task of IEnumerable UserModel</returns>
	public async Task<IEnumerable<UserModel>> GetAllAsync()
	{
		FilterDefinition<UserModel>? filter = Builders<UserModel>.Filter.Empty;

		List<UserModel>? result = (await _collection.FindAsync(filter)).ToList();

		return result;
	}


	/// <summary>
	///   UpdateUser method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <param name="user">UserModel</param>
	public async Task UpdateAsync(string itemId, UserModel user)
	{
		ObjectId objectId = new ObjectId(itemId);

		FilterDefinition<UserModel>? filter = Builders<UserModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter!, user);
	}


	/// <summary>
	///   GetUserFromAuthentication method
	/// </summary>
	/// <param name="userObjectIdentifierId">string</param>
	/// <returns>Task of UserModel</returns>
	public async Task<UserModel> GetFromAuthenticationAsync(string userObjectIdentifierId)
	{
		FilterDefinition<UserModel>? filter = Builders<UserModel>.Filter.Eq("object_identifier", userObjectIdentifierId);

		UserModel? result = (await _collection.FindAsync(filter)).FirstOrDefault();

		return result;
	}
}