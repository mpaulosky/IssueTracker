using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IssueTracker.Library.UnitTests.StatusRepositoryTests;

[ExcludeFromCodeCoverage]
public class StatusRepositoryTests
{
	private StatusRepository _sut;
	private readonly Mock<IMongoCollection<Status>> _mockCollection;
	private readonly Mock<IMongoDbContext> _mockContext;
	private readonly Mock<IAsyncCursor<Status>> _cursor;
	private List<Status> _list = new();

	public StatusRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor<Status>(_list);

		_mockCollection = TestFixtures.GetMockCollection<Status>(_cursor);

		_mockContext = TestFixtures.GetMockContext();
	}

	[Fact(DisplayName = "Get Status With a Valid Id")]
	public async Task GetStatus_With_Valid_Id_Should_Returns_One_Status_Test()
	{
		// Arrange

		var expected = TestStatuses.GetKnownStatus();

		_list = new List<Status> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<Status>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new StatusRepository(_mockContext.Object);

		//Act

		var result = await _sut.GetStatus(expected.Id);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Status>>(),
			It.IsAny<FindOptions<Status>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
		result.StatusName.Length.Should().BeGreaterThan(1);
	}

	[Fact(DisplayName = "Get Status With Empty String Id")]
	public async Task GetStatus_With_Empty_String_Id_Should_Return_A_IndexOutOfRangeException_TestAsync()
	{
		// Arrange

		_mockContext.Setup(c => c.GetCollection<Status>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new StatusRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<IndexOutOfRangeException>(() => _sut.GetStatus(""));
	}

	[Fact(DisplayName = "Get Status With Null Id")]
	public async Task Get_With_Null_Id_Should_Return_An_ArgumentNullException_TestAsync()
	{
		// Arrange

		_mockContext.Setup(c => c.GetCollection<Status>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new StatusRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetStatus(null));
	}

	[Fact(DisplayName = "Get Statuses")]
	public async Task GetStatuses_With_Valid_Context_Should_Return_A_List_Of_Statuses_Test()
	{
		// Arrange

		var expected = TestStatuses.GetStatuses().ToList();

		_list = new List<Status>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<Status>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new StatusRepository(_mockContext.Object);

		// Act

		var result = await _sut.GetStatuses().ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Status>>(),
			It.IsAny<FindOptions<Status>>(),
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
		
		_mockContext.Setup(c => c.GetCollection<Status>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new StatusRepository(_mockContext.Object);

		// Act

		await _sut.CreateStatus(newStatus);

		// Assert

		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c.InsertOneAsync(newStatus, null, default(CancellationToken)), Times.Once);
	}

	[Fact(DisplayName = "Update Status")]
	public async Task UpdateStatus_With_A_Valid_Id_And_Status_Should_UpdateStatus_Test()
	{
		// Arrange

		var expected = TestStatuses.GetKnownStatus();

		var updatedStatus = TestStatuses.GetStatus(expected.Id, expected.StatusDescription, "Updated New");

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<Status>() { updatedStatus };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<Status>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new StatusRepository(_mockContext.Object);

		// Act

		await _sut.UpdateStatus(updatedStatus.Id, updatedStatus);

		// Assert

		_mockCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<Status>>(), updatedStatus, It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}
}