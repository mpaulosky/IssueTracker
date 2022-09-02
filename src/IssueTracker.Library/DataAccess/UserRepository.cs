//-----------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.DataAccess;

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
	public UserRepository(IMongoDbContext context)
	{
		Guard.Against.Null(context, nameof(context));

		string collectionName;

		collectionName = Guard.Against.NullOrWhiteSpace(GetCollectionName(nameof(UserModel)), nameof(collectionName));

		_collection = context.GetCollection<UserModel>(collectionName);
	}

	/// <summary>
	///   GetUser method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of UserModel</returns>
	public async Task<UserModel> GetUser(string userId)
	{
		Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

		var objectId = new ObjectId(userId);

		var filter = Builders<UserModel>.Filter.Eq("_id", objectId);

		var result = await _collection!.FindAsync(filter).ConfigureAwait(true);

		return result.FirstOrDefault();
	}

	/// <summary>
	///   GetUsers method
	/// </summary>
	/// <returns>Task of IEnumerable UserModel</returns>
	public async Task<IEnumerable<UserModel>> GetUsers()
	{
		var all = await _collection!.FindAsync(Builders<UserModel>.Filter.Empty).ConfigureAwait(true);

		return await all.ToListAsync().ConfigureAwait(true);
	}

	/// <summary>
	///   CreateUser method
	/// </summary>
	/// <param name="user">UserModel</param>
	public async Task CreateUser(UserModel user)
	{
		Guard.Against.Null(user, nameof(user));

		await _collection!.InsertOneAsync(user).ConfigureAwait(true);
	}

	/// <summary>
	///   UpdateUser method
	/// </summary>
	/// <param name="id">string</param>
	/// <param name="user">UserModel</param>
	public async Task UpdateUser(string id, UserModel user)
	{
		Guard.Against.NullOrWhiteSpace(id, nameof(id));
		Guard.Against.Null(user, nameof(user));

		var filter = Guard.Against.Null(Builders<UserModel>.Filter.Eq("_id", id), nameof(FieldDefinition<UserModel>));

		await _collection!.ReplaceOneAsync(filter!, user).ConfigureAwait(true);
	}

	/// <summary>
	///   GetUserFromAuthentication method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of UserModel</returns>
	public async Task<UserModel> GetUserFromAuthentication(string userId)
	{
		Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

		var results = await _collection.FindAsync(u => u != null && u.ObjectIdentifier == userId).ConfigureAwait(true);

		return results.FirstOrDefault();
	}
}