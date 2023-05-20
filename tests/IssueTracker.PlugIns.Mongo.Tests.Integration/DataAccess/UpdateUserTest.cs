namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpdateUserTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly UserRepository _sut;
	private const string CleanupValue = "users";

	public UpdateUserTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new UserRepository(context);

	}

	[Fact(DisplayName = "UpdateAsync With Valid Data Should Update Successfully")]
	public async Task UpdateAsync_With_Valid_Data_Should_Update_Successfully_TestAsync()
	{

		// Arrange
		var expected = FakeUser.GetNewUser();

		await _sut.CreateAsync(expected).ConfigureAwait(false);

		var update = new UserModel
		{
			Id = expected.Id,
			FirstName = expected.FirstName,
			LastName = expected.LastName,
			EmailAddress = "test@test.com",
			Archived = expected.Archived
		};

		// Act
		await _sut.UpdateAsync(update).ConfigureAwait(false);

		var result = await _sut.GetAsync(expected.Id).ConfigureAwait(false);

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result.EmailAddress.Should().Be(update.EmailAddress);

	}

	public Task InitializeAsync()
	{

		return Task.CompletedTask;

	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(CleanupValue);

	}

}
