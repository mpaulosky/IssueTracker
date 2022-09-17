//-----------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.DataAccess;

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

		string collectionName = GetCollectionName(nameof(UserModel));

		_collection = context.GetCollection<UserModel>(collectionName);
	}

	/// <summary>
	///		GetUser method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of UserModel</returns>
	public async Task<UserModel> GetUser(string userId)
	{
		ObjectId objectId = new ObjectId(userId);

		FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Eq("_id", objectId);

		UserModel result = (await _collection!.FindAsync(filter)).FirstOrDefault();

		return result;
	}

	/// <summary>
	///		GetUsers method
	/// </summary>
	/// <returns>Task of IEnumerable UserModel</returns>
	public async Task<IEnumerable<UserModel>> GetUsers()
	{
		FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Empty;

		List<UserModel> result = (await _collection!.FindAsync(filter)).ToList();

		return result;
	}

	/// <summary>
	///		CreateUser method
	/// </summary>
	/// <param name="user">UserModel</param>
	public async Task CreateUser(UserModel user)
	{
		await _collection!.InsertOneAsync(user);
	}

	/// <summary>
	///		UpdateUser method
	/// </summary>
	/// <param name="id">string</param>
	/// <param name="user">UserModel</param>
	public async Task UpdateUser(string id, UserModel user)
	{
		FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Eq("_id", id);

		await _collection!.ReplaceOneAsync(filter!, user);
	}

	/// <summary>
	///		GetUserFromAuthentication method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of UserModel</returns>
	public async Task<UserModel> GetUserFromAuthentication(string userId)
	{
		FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Eq("object_identifier", userId);

		UserModel result = (await _collection!.FindAsync(filter)).FirstOrDefault();

		return result;
	}
}