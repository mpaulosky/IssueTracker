﻿namespace IssueTracker.UI.Tests.Integration.Services.StatusServicesTests;

public class StatusServiceTests : IClassFixture<IssueTrackerUIFactory>
{
	private readonly IssueTrackerUIFactory _factory;
	private IStatusRepository _repo;
	private IMemoryCache _cache;

	public StatusServiceTests(IssueTrackerUIFactory factory)
	{

		_factory = factory;
		_repo = (IStatusRepository)_factory.Services.GetRequiredService(typeof(IStatusRepository));
		_cache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));

	}

	[Fact]
	public void StatusService_With_InvalidStatusRepository_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_repo = null;

		// Act
		var act = () => new StatusService(_repo, _cache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

	[Fact]
	public void StatusService_With_InvalidMemoryCache_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cache = null;

		// Act
		var act = () => new StatusService(_repo, _cache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

}