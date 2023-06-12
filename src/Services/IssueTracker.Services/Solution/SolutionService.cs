﻿// Copyright (c) 2023. All rights reserved.
// File Name :     SolutionService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services

namespace IssueTracker.Services.Solution;

/// <summary>
///   SolutionService class
/// </summary>
public class SolutionService : ISolutionService
{
	private const string CacheName = "SolutionData";
	private readonly IMemoryCache _cache;
	private readonly ISolutionRepository _repository;

	/// <summary>
	///   SolutionService
	/// </summary>
	/// <param name="repository">ISolutionRepository</param>
	/// <param name="cache">IMemoryCache</param>
	/// <exception cref="ArgumentNullException"></exception>
	public SolutionService(ISolutionRepository repository, IMemoryCache cache)
	{
		ArgumentNullException.ThrowIfNull(repository);
		ArgumentNullException.ThrowIfNull(cache);

		_repository = repository;
		_cache = cache;
	}

	/// <summary>
	///   CreateSolution method
	/// </summary>
	/// <param name="solution">SolutionModel</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task CreateSolution(SolutionModel solution)
	{
		ArgumentNullException.ThrowIfNull(solution);

		await _repository.CreateAsync(solution);
	}

	/// <summary>
	///   GetSolution method
	/// </summary>
	/// <param name="solutionId">string</param>
	/// <returns>Task of SolutionModel</returns>
	/// <exception cref="ArgumentException"></exception>
	public async Task<SolutionModel> GetSolution(string? solutionId)
	{
		ArgumentException.ThrowIfNullOrEmpty(solutionId);

		SolutionModel results = await _repository.GetAsync(solutionId);

		return results;
	}

	/// <summary>
	///   GetSolutions method
	/// </summary>
	/// <returns>Task of List SolutionModels</returns>
	public async Task<List<SolutionModel>> GetSolutions()
	{
		List<SolutionModel>? output = _cache.Get<List<SolutionModel>>(CacheName);

		if (output is not null)
		{
			return output;
		}

		IEnumerable<SolutionModel> results = await _repository.GetAllAsync();

		output = results.ToList();

		_cache.Set(CacheName, output, TimeSpan.FromMinutes(1));

		return output;
	}

	/// <summary>
	///   GetSolutionsByUser method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of List SolutionModels</returns>
	/// <exception cref="ArgumentException"></exception>
	public async Task<List<SolutionModel>> GetSolutionsByUser(string userId)
	{
		ArgumentException.ThrowIfNullOrEmpty(userId);

		List<SolutionModel>? output = _cache.Get<List<SolutionModel>>(userId);

		if (output is not null)
		{
			return output;
		}

		IEnumerable<SolutionModel> results = await _repository.GetByUserAsync(userId);

		output = results.ToList();

		_cache.Set(userId, output, TimeSpan.FromMinutes(1));

		return output;
	}

	/// <summary>
	///   GetSolutionsByIssue method
	/// </summary>
	/// <param name="issueId">string</param>
	/// <returns>Task of List SolutionModels</returns>
	/// <exception cref="ArgumentException"></exception>
	public async Task<List<SolutionModel>> GetSolutionsByIssue(string issueId)
	{
		ArgumentException.ThrowIfNullOrEmpty(issueId);

		List<SolutionModel>? output = _cache.Get<List<SolutionModel>>(issueId);

		if (output is not null)
		{
			return output;
		}

		IEnumerable<SolutionModel> results = await _repository.GetByIssueAsync(issueId);

		output = results.ToList();

		_cache.Set(issueId, output, TimeSpan.FromMinutes(1));

		return output;
	}

	/// <summary>
	///   UpdateSolution
	/// </summary>
	/// <param name="solution">SolutionModel</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task UpdateSolution(SolutionModel solution)
	{
		ArgumentNullException.ThrowIfNull(solution);

		await _repository.UpdateAsync(solution.Id, solution);

		_cache.Remove(CacheName);
	}
}