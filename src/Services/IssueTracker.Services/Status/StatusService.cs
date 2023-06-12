// Copyright (c) 2023. All rights reserved.
// File Name :     StatusService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services

namespace IssueTracker.Services.Status;

/// <summary>
///   StatusService class
/// </summary>
public class StatusService : IStatusService
{
	private const string CacheName = "StatusData";
	private readonly IMemoryCache _cache;
	private readonly IStatusRepository _repository;


	/// <summary>
	///   StatusService constructor
	/// </summary>
	/// <param name="repository">IStatusRepository</param>
	/// <param name="cache">IMemoryCache</param>
	/// <exception cref="ArgumentNullException"></exception>
	public StatusService(IStatusRepository repository, IMemoryCache cache)
	{
		ArgumentNullException.ThrowIfNull(repository);
		ArgumentNullException.ThrowIfNull(cache);

		_repository = repository;
		_cache = cache;
	}


	/// <summary>
	///   CreateStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task CreateStatus(StatusModel status)
	{
		ArgumentNullException.ThrowIfNull(status);

		return _repository.CreateAsync(status);
	}


	/// <summary>
	///   DeleteStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task DeleteStatus(StatusModel status)
	{
		ArgumentNullException.ThrowIfNull(status);

		return _repository.ArchiveAsync(status);
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

		StatusModel result = await _repository.GetAsync(statusId);

		return result;
	}


	/// <summary>
	///   GetStatuses method
	/// </summary>
	/// <returns>Task of List StatusModels</returns>
	public async Task<List<StatusModel>> GetStatuses()
	{
		List<StatusModel>? output = _cache.Get<List<StatusModel>>(CacheName);

		if (output is not null)
		{
			return output;
		}

		IEnumerable<StatusModel> results = await _repository.GetAllAsync();

		output = results.ToList();

		_cache.Set(CacheName, output, TimeSpan.FromDays(1));

		return output;
	}


	/// <summary>
	///   UpdateStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task UpdateStatus(StatusModel status)
	{
		ArgumentNullException.ThrowIfNull(status);

		return _repository.UpdateAsync(status.Id, status);
	}
}