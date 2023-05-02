//-----------------------------------------------------------------------
// <copyright>
//	File:		IssueRepository.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.Mongo.DataAccess;

/// <summary>
///		IssueRepository class
/// </summary>
public class IssueRepository : IIssueRepository
{

	private readonly IMongoCollection<IssueModel> _collection;

	/// <summary>
	///		IssueRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public IssueRepository(IMongoDbContextFactory context)
	{

		Guard.Against.Null(context, nameof(context));

		var issueCollectionName = GetCollectionName(nameof(IssueModel));

		_collection = context.GetCollection<IssueModel>(issueCollectionName);

	}

	///  <summary>
	/// 		ArchiveAsync method
	///  </summary>
	///  <param name="issue">IssueModel</param>
	public async Task ArchiveAsync(IssueModel issue)
	{

		var objectId = new ObjectId(issue.Id);

		FilterDefinition<IssueModel> filter = Builders<IssueModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter, issue);

	}
	
	/// <summary>
	///		CreateIssue method
	/// </summary>
	/// <param name="issue">IssueModel</param>
	public async Task CreateAsync(IssueModel issue)
	{

		await _collection.InsertOneAsync(issue);

	}

	///  <summary>
	/// 	GetIssue method
	///  </summary>
	///  <param name="itemId">string</param>
	///  <returns>Task of IssueModel</returns>
	public async Task<IssueModel?> GetAsync(string itemId)
	{

		return (await _collection
			.FindAsync(s=> s.Id == itemId && s.Archived == false && s.Rejected == false))
			.FirstOrDefault();

	}
	/// <summary>
	///		GetIssues method
	/// </summary>
	/// <param name="includeArchived">bool default is false</param>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>?> GetAllAsync(bool includeArchived = false)
	{

		if (includeArchived)
		{
			
			var filter = Builders<IssueModel>.Filter.Empty;
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

	/// <summary>
	///		GetIssuesWaitingForApproval method
	/// </summary>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>?> GetWaitingForApprovalAsync()
	{

		return (await _collection
			.FindAsync(x => x.ApprovedForRelease == false && x.Archived == false && x.Rejected == false))
			.ToList();

	}

	/// <summary>
	///		GetApprovedIssues method
	/// </summary>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>?> GetApprovedAsync()
	{

		return (await _collection
				.FindAsync(x => x.ApprovedForRelease == true && x.Archived == false && x.Rejected == false))
			.ToList();

	}

	/// <summary>
	///		GetUserIssues method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>?> GetByUserAsync(string userId)
	{

		return (await _collection
			.FindAsync(s => s.Author.Id == userId && s.Archived == false && s.Rejected == false))
			.ToList();

	}

	///  <summary>
	/// 		UpdateAsync method
	///  </summary>
	///  <param name="issue">IssueModel</param>
	public async Task UpdateAsync(IssueModel issue)
	{

		var objectId = new ObjectId(issue.Id);

		FilterDefinition<IssueModel> filter = Builders<IssueModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter, issue);

	}

}
