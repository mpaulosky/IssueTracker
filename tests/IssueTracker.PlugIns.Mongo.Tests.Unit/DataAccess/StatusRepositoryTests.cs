namespace IssueTracker.PlugIns.Mongo.Tests.Unit.DataAccess;

[ExcludeFromCodeCoverage]
public class StatusRepositoryTests
{

	private readonly Mock<IAsyncCursor<StatusModel>> _cursor;
	private readonly Mock<IMongoCollection<StatusModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private List<StatusModel> _list = new();

	public StatusRepositoryTests()
	{

		_cursor = TestFixtures.GetMockCursor(_list);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);

		_mockContext = TestFixtures.GetMockContext();

	}

	private StatusRepository CreateRepository()
	{

		return new StatusRepository(_mockContext.Object);

	}

	[Fact(DisplayName = "Create Status Test")]
	public async Task CreateStatusAsync_With_Valid_Status_Should_Insert_A_New_Status_TestAsync()
	{

		// Arrange
		var newStatus = FakeStatus.GetNewStatus();

		_mockContext.Setup(c => c
			.GetCollection<StatusModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.CreateStatusAsync(newStatus);

		// Assert
		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c
			.InsertOneAsync(newStatus, null, default), Times.Once);

	}

	[Fact(DisplayName = "Get Status By Id")]
	public async Task GetStatusByIdAsync_With_Valid_Id_Should_Returns_One_Status_TestAsync()
	{

		// Arrange
		var expected = FakeStatus.GetStatuses(1).First();

		_list = new List<StatusModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<StatusModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		var result = await sut.GetStatusByIdAsync(expected.Id);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);
		result.StatusName.Length.Should().BeGreaterThan(1);


		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<StatusModel>>(),
			It.IsAny<FindOptions<StatusModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Statuses Test")]
	public async Task GetStatusesAsync_With_Valid_Context_Should_Return_A_List_Of_Statuses_Test()
	{

		// Arrange
		const int expectedCount = 5;
		var expected = FakeStatus.GetStatuses(expectedCount);

		_list = new List<StatusModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<StatusModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var results = (await sut.GetStatusesAsync()).ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);
		results.Should().BeEquivalentTo(expected);

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<StatusModel>>(),
			It.IsAny<FindOptions<StatusModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Update Status Test")]
	public async Task UpdateStatusAsync_With_A_Valid_Id_And_Status_Should_UpdateStatus_Test()
	{

		// Arrange
		var expected = FakeStatus.GetNewStatus();
		expected.Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString();

		await _mockCollection.Object.InsertOneAsync(expected);

		var updatedStatus = FakeStatus.GetNewStatus();

		updatedStatus.Id = expected.Id;
		updatedStatus.StatusName = expected.StatusName;
		updatedStatus.StatusDescription = "Updated New StatusDescription";

		//_list = new List<StatusModel> { updatedStatus };

		//_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<StatusModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.UpdateStatusAsync(updatedStatus).ConfigureAwait(false);

		// Assert
		_mockCollection.Verify(
			c => c
				.ReplaceOneAsync(It.IsAny<FilterDefinition<StatusModel>>(), updatedStatus,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}
}
