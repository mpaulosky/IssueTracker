using MongoDB.Driver;

namespace IssueTracker.Library.Tests.Unit.DataAccess;

[ExcludeFromCodeCoverage]
public class StatusRepositoryTests
{
	private readonly Mock<IAsyncCursor<StatusModel>> _cursor;
	private readonly Mock<IMongoCollection<StatusModel>> _mockCollection;
	private readonly Mock<IMongoDbContext> _mockContext;
	private List<StatusModel> _list = new();
	private StatusRepository _sut;

	public StatusRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor(_list);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);

		_mockContext = TestFixtures.GetMockContext();

		_sut = new StatusRepository(_mockContext.Object);
	}

	[Fact(DisplayName = "Get Status With a Valid Id")]
	public async Task GetStatus_With_Valid_Id_Should_Returns_One_Status_Test()
	{
		// Arrange

		var expected = TestStatuses.GetKnownStatus();

		_list = new List<StatusModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<StatusModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new StatusRepository(_mockContext.Object);

		//Act

		var result = await _sut.GetStatus(expected.Id);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<StatusModel>>(),
			It.IsAny<FindOptions<StatusModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
		result.StatusName.Length.Should().BeGreaterThan(1);
	}

	[Fact(DisplayName = "Get Statuses")]
	public async Task GetStatuses_With_Valid_Context_Should_Return_A_List_Of_Statuses_Test()
	{
		// Arrange

		var expected = TestStatuses.GetStatuses().ToList();

		_list = new List<StatusModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<StatusModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new StatusRepository(_mockContext.Object);

		// Act

		var result = await _sut.GetStatuses().ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<StatusModel>>(),
			It.IsAny<FindOptions<StatusModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(3);
	}

	[Fact(DisplayName = "Create Status")]
	public async Task Create_With_Valid_Status_Should_Insert_A_New_Status_TestAsync()
	{
		// Arrange

		var newStatus = TestStatuses.GetKnownStatus();

		_mockContext.Setup(c => c.GetCollection<StatusModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new StatusRepository(_mockContext.Object);

		// Act

		await _sut.CreateStatus(newStatus);

		// Assert

		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c.InsertOneAsync(newStatus, null, default), Times.Once);
	}

	[Fact(DisplayName = "Update Status")]
	public async Task UpdateStatus_With_A_Valid_Id_And_Status_Should_UpdateStatus_Test()
	{
		// Arrange

		var expected = TestStatuses.GetKnownStatus();

		var updatedStatus = TestStatuses.GetStatus(expected.Id, expected.StatusDescription, "Updated New");

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<StatusModel> { updatedStatus };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<StatusModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new StatusRepository(_mockContext.Object);

		// Act

		await _sut.UpdateStatus(updatedStatus.Id, updatedStatus);

		// Assert

		_mockCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<StatusModel>>(), updatedStatus, It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}
}