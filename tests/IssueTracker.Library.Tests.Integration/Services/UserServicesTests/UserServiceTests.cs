﻿using IssueTracker.PlugIns.PlugInRepositoryInterfaces;
using IssueTracker.PlugIns.Services;

namespace IssueTracker.PlugIns.Mongo.Services.UserServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UserServiceTests : IAsyncLifetime
{
	private readonly IssueTrackerTestFactory _factory;
	private IUserRepository _repo;
	private const string _cleanupValue = "";

	public UserServiceTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		_repo = (IUserRepository)_factory.Services.GetRequiredService(typeof(IUserRepository));

	}

	[Fact]
	public void UserService_With_InvalidUserRepository_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_repo = null;

		// Act
		Func<UserService> act = () => new UserService(_repo);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(_cleanupValue);

	}

}