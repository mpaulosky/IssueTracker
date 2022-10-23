﻿namespace IssueTracker.Library.Services.StatusServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class GetStatusTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusService _sut;

	public GetStatusTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IStatusRepository)_factory.Services.GetRequiredService(typeof(IStatusRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new StatusService(repo, memCache);

	}

	[Fact]
	public async Task GetStatus_With_WithData_Should_ReturnAValidStatus_TestAsync()
	{

		// Arrange
		var expected = FakeStatus.GetNewStatus();
		await _sut.CreateStatus(expected);

		// Act
		var result = await _sut.GetStatus(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Fact]
	public async Task GetStatus_With_WithoutData_Should_ReturnNothing_TestAsync()
	{
		// Arrange
		var id = "62cf2ad6326e99d665759e5a";

		// Act
		var result = await _sut.GetStatus(id);

		// Assert
		result.Should().BeNull();

	}

	[Fact]
	public async Task GetStatus_With_NullId_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		string id = null;

		// Act
		var act = async () => await _sut.GetStatus(id);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

	[Fact]
	public async Task GetStatus_With_EmptyStringId_Should_ThrowArgumentException_Test()
	{

		// Arrange
		var id = "";

		// Act
		var act = async () => await _sut.GetStatus(id);

		// Assert
		await act.Should().ThrowAsync<ArgumentException>();

	}

}