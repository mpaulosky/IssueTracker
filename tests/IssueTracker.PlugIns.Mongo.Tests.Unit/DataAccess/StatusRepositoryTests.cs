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

	[Fact(DisplayName = "Archive Status Test")]
	public async Task ArchiveAsync_With_Valid_Status_Should_Archive_the_Status_TestAsync()
	{

		// Arrange
		var expected = FakeStatus.GetNewStatus(true);

		var updatedStatus = FakeStatus.GetNewStatus(true);
		updatedStatus.Archived = true;

		await _mockCollection.Object.InsertOneAsync(expected);

		_mockContext.Setup(c => c
				.GetCollection<StatusModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.ArchiveAsync(updatedStatus);

		// Assert

		_mockCollection.Verify(
			c => c.
				ReplaceOneAsync(It.IsAny<FilterDefinition<StatusModel>>(),
					updatedStatus,
					It.IsAny<ReplaceOptions>(),
					It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Create Status Test")]
	public async Task CreateAsync_With_Valid_Status_Should_Insert_A_New_Status_TestAsync()
	{

		// Arrange
		var newStatus = FakeStatus.GetNewStatus();

		_mockContext.Setup(c => c
			.GetCollection<StatusModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.CreateAsync(newStatus);

		// Assert
		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c
			.InsertOneAsync(newStatus, null, default), Times.Once);

	}

	[Fact(DisplayName = "Get Status By Id")]
	public async Task GetAsync_With_Valid_Id_Should_Returns_One_Status_TestAsync()
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
		var result = await sut.GetAsync(expected.Id);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);

		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<StatusModel>>(),
			It.IsAny<FindOptions<StatusModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Statuses Without Archived")]
	public async Task GetAllAsync_With_Valid_Context_Should_Return_A_List_Of_Statuses_Without_Archived_TestAsync()
	{

		// Arrange
		const int expectedCount = 5;
		var expected = FakeStatus.GetStatuses(expectedCount).ToList();
		foreach (var item in expected)
		{
			item.Archived = false;
		}
		
		await _mockCollection.Object.InsertManyAsync(expected);

		_mockContext.Setup(c => c
			.GetCollection<StatusModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		_list = new List<StatusModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = CreateRepository();

		// Act
		var results = (await sut.GetAllAsync().ConfigureAwait(false))!.ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);
		results.Should().BeEquivalentTo(expected);
		results.Any(x=>x.Archived).Should().BeFalse();

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<StatusModel>>(),
			It.IsAny<FindOptions<StatusModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Statuses With Archived")]
	public async Task GetAllAsync_With_Valid_Context_Should_Return_A_List_Of_Statuses_With_Archived_TestAsync()
	{

		// Arrange
		const int expectedCount = 5;
		var expected = FakeStatus.GetStatuses(expectedCount).ToList();
		
		await _mockCollection.Object.InsertManyAsync(expected);

		_mockContext.Setup(c => c
				.GetCollection<StatusModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		_list = new List<StatusModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = CreateRepository();

		// Act
		var results = (await sut.GetAllAsync(true).ConfigureAwait(false))!.ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);
		results.Should().BeEquivalentTo(expected);
		results.Any(x=>x.Archived).Should().BeTrue();

		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<StatusModel>>(),
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

		_mockContext.Setup(c => c
			.GetCollection<StatusModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.UpdateAsync(updatedStatus).ConfigureAwait(false);

		// Assert
		_mockCollection.Verify(
			c => c
				.ReplaceOneAsync(It.IsAny<FilterDefinition<StatusModel>>(), updatedStatus,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}
}
