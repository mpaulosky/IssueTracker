// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     StatusService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

namespace IssueTracker.Services.Status;

/// <summary>
///   StatusService class
/// </summary>
public class StatusService(IStatusRepository repository, IMemoryCache cache) : IStatusService
{
	private const string CacheName = "StatusData";

	/// <summary>
	///   ArchiveStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task ArchiveStatus(StatusModel status)
	{
		ArgumentNullException.ThrowIfNull(status);

		cache.Remove(CacheName);

		return repository.ArchiveAsync(status);
	}

	/// <summary>
	///   CreateStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task CreateStatus(StatusModel status)
	{
		ArgumentNullException.ThrowIfNull(status);

		await repository.CreateAsync(status);

		cache.Remove(CacheName);
	}

	/// <summary>
	///   GetStatus method
	/// </summary>
	/// <param name="statusId">string</param>
	/// <returns>Task StatusModel</returns>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="ArgumentException"></exception>
	public async Task<StatusModel> GetStatus(string statusId)
	{
		ArgumentException.ThrowIfNullOrEmpty(statusId);

		StatusModel result = await repository.GetAsync(statusId);

		return result;
	}

	/// <summary>
	///   GetStatuses method
	/// </summary>
	/// <returns>Task of List StatusModels</returns>
	public async Task<List<StatusModel>> GetStatuses()
	{
		List<StatusModel>? output = cache.Get<List<StatusModel>>(CacheName);

		if (output is not null)
		{
			return output;
		}

		IEnumerable<StatusModel> results = await repository.GetAllAsync();

		output = results.ToList();

		cache.Set(CacheName, output, TimeSpan.FromDays(1));

		return output;
	}

	/// <summary>
	///   UpdateStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task UpdateStatus(StatusModel status)
	{
		ArgumentNullException.ThrowIfNull(status);

		await repository.UpdateAsync(status.Id, status);

		cache.Remove(CacheName);
	}

	/// <summary>
	///   DeleteStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task DeleteStatus(StatusModel status)
	{
		ArgumentNullException.ThrowIfNull(status);

		await repository.ArchiveAsync(status);

		cache.Remove(CacheName);
	}
}