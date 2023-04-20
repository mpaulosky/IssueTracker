//-----------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace IssueTracker.PlugIns.DataAccess;

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

	/// <summary>
	///		GetUser method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <returns>Task of UserModel</returns>
	public async Task<UserModel> GetUserAsync(string itemId)
	{

		var objectId = new ObjectId(itemId);

		FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Eq("_id", objectId);

		UserModel result = (await _collection!.FindAsync(filter)).FirstOrDefault();

		return result;

	}

	/// <summary>
	///		GetUsers method
	/// </summary>
	/// <returns>Task of IEnumerable UserModel</returns>
	public async Task<IEnumerable<UserModel>> GetUsersAsync()
	{

		FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Empty;

		var result = (await _collection!.FindAsync(filter)).ToList();

		return result;

	}

	/// <summary>
	///		CreateUser method
	/// </summary>
	/// <param name="user">UserModel</param>
	public async Task CreateUserAsync(UserModel user)
	{

		await _collection!.InsertOneAsync(user);

	}

	/// <summary>
	///		UpdateUser method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <param name="user">UserModel</param>
	public async Task UpdateUserAsync(string itemId, UserModel user)
	{
		var objectId = new ObjectId(itemId);

		FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Eq("_id", objectId);

		await _collection!.ReplaceOneAsync(filter!, user);

	}

	/// <summary>
	///		GetUserFromAuthentication method
	/// </summary>
	/// <param name="userObjectIdentifierId">string</param>
	/// <returns>Task of UserModel</returns>
	public async Task<UserModel> GetUserFromAuthenticationAsync(string userObjectIdentifierId)
	{

		FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Eq("object_identifier", userObjectIdentifierId);

		UserModel result = (await _collection!.FindAsync(filter)).FirstOrDefault();

		return result;

	}

	public async Task ArchiveUserAsync(UserModel user)
	{

		var objectId = new ObjectId(user.Id);

		// Archive the user
		user.Archived = true;

		FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Eq("_id", objectId);

		await _collection!.ReplaceOneAsync(filter!, user);

	}
}
