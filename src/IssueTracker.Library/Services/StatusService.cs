//-----------------------------------------------------------------------
// <copyright file="StatusService.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Services;

/// <summary>
///		StatusService class
/// </summary>
public class StatusService : IStatusService
{
	private const string _cacheName = "StatusData";
	private readonly IMemoryCache _cache;
	private readonly IStatusRepository _repository;

	/// <summary>
	///		StatusService constructor
	/// </summary>
	/// <param name="repository">IStatusRepository</param>
	/// <param name="cache">IMemoryCache</param>
	/// <exception cref="ArgumentNullException"></exception>
	public StatusService(IStatusRepository repository, IMemoryCache cache)
	{
		_repository = Guard.Against.Null(repository, nameof(repository));
		_cache = Guard.Against.Null(cache, nameof(cache));
	}

	/// <summary>
	///		CreateStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task CreateStatus(StatusModel status)
	{
		Guard.Against.Null(status, nameof(status));

		return _repository.CreateStatus(status);
	}

	/// <summary>
	///		GetStatus method
	/// </summary>
	/// <param name="statusId">string</param>
	/// <returns>Task StatusModel</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<StatusModel> GetStatus(string statusId)
	{
		Guard.Against.NullOrWhiteSpace(statusId, nameof(statusId));

		StatusModel result = await _repository.GetStatus(statusId).ConfigureAwait(true);

		return result;
	}

	/// <summary>
	///		GetStatuses method
	/// </summary>
	/// <returns>Task of List StatusModels</returns>
	public async Task<List<StatusModel>> GetStatuses()
	{
		List<StatusModel> output = _cache.Get<List<StatusModel>>(_cacheName);
		if (output is not null)
		{
			return output;
		}

		IEnumerable<StatusModel> results = await _repository.GetStatuses().ConfigureAwait(true);
		output = results.ToList();

		_cache.Set(_cacheName, output, TimeSpan.FromDays(1));

		return output;
	}

	/// <summary>
	///		UpdateStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task UpdateStatus(StatusModel status)
	{
		Guard.Against.Null(status, nameof(status));

		return _repository.UpdateStatus(status.Id, status);
	}
}