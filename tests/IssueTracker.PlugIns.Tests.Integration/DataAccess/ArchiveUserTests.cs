﻿namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class ArchiveUserTests : IAsyncLifetime
{
	private const string CleanupValue = "users";

	private readonly IssueTrackerTestFactory _factory;
	private readonly UserRepository _sut;

	public ArchiveUserTests(IssueTrackerTestFactory factory)
	{
		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new UserRepository(context);
	}

	[Fact]
	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	[Fact]
	public async Task DisposeAsync()
	{
		await _factory.ResetCollectionAsync(CleanupValue);
	}

	[Fact(DisplayName = "Archive User With Valid Data (Archive)")]
	public async Task ArchiveAsync_With_ValidData_Should_ArchiveAUser_TestAsync()
	{
		// Arrange
		var expected = FakeUser.GetNewUser();

		await _sut.CreateAsync(expected);

		// Act
		await _sut.ArchiveAsync(expected);

		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Archived.Should().BeTrue();
	}
}
