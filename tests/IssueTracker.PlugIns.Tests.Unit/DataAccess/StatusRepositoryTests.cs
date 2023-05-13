namespace IssueTracker.PlugIns.Tests.Unit.DataAccess;

[ExcludeFromCodeCoverage]
public class StatusRepositoryTests
{
	private readonly Mock<IAsyncCursor<StatusModel>> _cursor;
	private readonly Mock<IMongoCollection<StatusModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private List<StatusModel> _list = new();

	public StatusRepositoryTests()
	{

		_cursor = TestFixturesMongo.GetMockCursor(_list);

		_mockCollection = TestFixturesMongo.GetMockCollection(_cursor);

		_mockContext = GetMockMongoContext();

	}

	private StatusRepository CreateRepository()
	{

		return new StatusRepository(_mockContext.Object);

	}

	[Fact(DisplayName = "Create Status")]
	public async Task Create_With_Valid_Status_Should_Insert_A_New_Status_TestAsync()
	{

		// Arrange
		var newStatus = FakeStatus.GetNewStatus(true);

		_mockContext.Setup(c => c.GetCollection<StatusModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.CreateAsync(newStatus);

		// Assert
		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c
		.InsertOneAsync(
			newStatus,
			null,
			default), Times.Once);

	}

	[Fact(DisplayName = "Archive Status")]
	public async Task ArchiveStatus_With_Valid_Status_Should_Archive_the_Status_TestAsync()
	{

		// Arrange
		var expected = FakeStatus.GetNewStatus(true);

		var updatedStatus = FakeStatus.GetNewStatus(true);
		updatedStatus.Archived = true;

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<StatusModel> { updatedStatus };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<StatusModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.ArchiveAsync(updatedStatus);

		// Assert
		_mockCollection.Verify(
			c => c
			.ReplaceOneAsync(
				It.IsAny<FilterDefinition<StatusModel>>(),
				updatedStatus,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Status With a Valid Id")]
	public async Task GetStatus_With_Valid_Id_Should_Returns_One_Status_Test()
	{

		// Arrange
		var expected = FakeStatus.GetNewStatus(true);

		_list = new List<StatusModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<StatusModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		StatusModel result = await sut.GetAsync(expected.Id);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);

		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
		.FindAsync(
			It.IsAny<FilterDefinition<StatusModel>>(),
			It.IsAny<FindOptions<StatusModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Statuses")]
	public async Task GetStatuses_With_Valid_Context_Should_Return_A_List_Of_Statuses_Test()
	{

		// Arrange
		const int expectedCount = 4;

		var expected = FakeStatus.GetStatuses().ToList();

		_list = new List<StatusModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<StatusModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var results = (await sut.GetAllAsync().ConfigureAwait(false)).ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);

		_mockCollection.Verify(c => c
		.FindAsync(
			It.IsAny<FilterDefinition<StatusModel>>(),
			It.IsAny<FindOptions<StatusModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Update Status")]
	public async Task UpdateStatus_With_A_Valid_Id_And_Status_Should_UpdateStatus_Test()
	{

		// Arrange
		var expected = FakeStatus.GetNewStatus(true);

		var updatedStatus = FakeStatus.GetNewStatus(true);
		updatedStatus.StatusName = "Updated Status";
		updatedStatus.Id = expected.Id;

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<StatusModel> { updatedStatus };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<StatusModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.UpdateAsync(updatedStatus.Id, updatedStatus);

		// Assert
		_mockCollection.Verify(
			c => c
			.ReplaceOneAsync(
				It.IsAny<FilterDefinition<StatusModel>>(),
				updatedStatus,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

}
