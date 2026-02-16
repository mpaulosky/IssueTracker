// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     IStatusService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

namespace IssueTracker.Services.Status.Interface;

/// <summary>
/// Provides operations for managing issue statuses in the system.
/// </summary>
public interface IStatusService
{
	/// <summary>
	/// Archives an existing status, marking it as inactive.
	/// </summary>
	/// <param name="status">The status to archive.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="status"/> is null.</exception>
	Task ArchiveStatus(StatusModel status);

	/// <summary>
	/// Creates a new status in the system.
	/// </summary>
	/// <param name="status">The status to create.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="status"/> is null.</exception>
	Task CreateStatus(StatusModel status);

	/// <summary>
	/// Retrieves a specific status by its unique identifier.
	/// </summary>
	/// <param name="statusId">The unique identifier of the status.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains the requested <see cref="StatusModel"/>.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="statusId"/> is null or empty.</exception>
	Task<StatusModel> GetStatus(string statusId);

	/// <summary>
	/// Retrieves all statuses from the system with caching support.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains a list of all <see cref="StatusModel"/> instances.
	/// </returns>
	/// <remarks>
	/// This method uses in-memory caching with a 1-day expiration to improve performance.
	/// </remarks>
	Task<List<StatusModel>> GetStatuses();

	/// <summary>
	/// Updates an existing status with new information.
	/// </summary>
	/// <param name="status">The status with updated information.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="status"/> is null.</exception>
	Task UpdateStatus(StatusModel status);
}
