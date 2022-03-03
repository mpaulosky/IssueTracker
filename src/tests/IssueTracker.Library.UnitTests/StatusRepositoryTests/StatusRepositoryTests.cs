using FluentAssertions;

using IssueTracker.Library.UnitTests.Fixtures;

using IssueTrackerLibrary.Contracts;
using IssueTrackerLibrary.DataAccess;
using IssueTrackerLibrary.Helpers;
using IssueTrackerLibrary.Models;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

using Moq;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace IssueTracker.Library.UnitTests.StatusRepositoryTests;

[ExcludeFromCodeCoverage]
public class StatusRepositoryTests
{
		private readonly IOptions<IssueTrackerDatabaseSettings> _options;
	private readonly Mock<IMongoCollection<Status>> _mockCollection;
	private readonly Mock<IMongoDbContext> _mockContext;
	private readonly Mock<IAsyncCursor<Status>> _cursor;
	private List<Status> _list = new();

	public StatusRepositoryTests()
	{
		_options = TestFixtures.Settings();

		_cursor = TestFixtures.MockCursor<Status>();

		_mockCollection = TestFixtures.MockCollection<Status>(_cursor);

		_mockContext = TestFixtures.MockContext<Status>(_mockCollection);
	}

	[Fact(DisplayName = "Get Status With a Valid Id")]
	public async Task Get_With_Valid_Id_Should_Returns_One_Status_Test()
	{
		// Arrange
		
		var expected = TestStatuses.GetKnownStatus();

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<Status> { expected };
		
		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = new StatusRepository(_mockContext.Object);

		//Act

		var result = await sut.Get(expected.Id);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Status>>(),
			It.IsAny<FindOptions<Status>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
	}

	[Fact(DisplayName = "Get Status With Invalid Id")]
	public async Task Get_With_Invalid_Id_Should_Return_Null_Result_TestAsync()
	{
		// Arrange

		var sut = new StatusRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<IndexOutOfRangeException>(() => sut.Get(""));
	}

	[Fact(DisplayName = "Get Statuses")]
	public async Task Get_With_Valid_Context_Should_Return_A_List_Of_Statuses_Test()
	{
		// Arrange

		var expected = TestStatuses.GetStatuses().ToList();

		await _mockCollection.Object.InsertManyAsync(expected);

		_list = new List<Status>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = new StatusRepository(_mockContext.Object);

		// Act

		var result = await sut.Get().ConfigureAwait(false);

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
		var sut = new StatusRepository(_mockContext.Object);

		// Act

		await sut.Create(newStatus);

		// Assert

		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c.InsertOneAsync(newStatus, null, default(CancellationToken)), Times.Once);
	}

	[Fact(DisplayName = "Update Status")]
	public async Task Update_With_A_Valid_Id_And_Status_Should_UpdateStatus_Test()
	{
		// Arrange

		var expected = TestStatuses.GetKnownStatus();

		var updatedStatus = TestStatuses.GetStatus(expected.Id, expected.StatusDescription, "Updated New");

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<Status>(){updatedStatus};

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = new StatusRepository(_mockContext.Object);

		// Act

		await sut.Update(updatedStatus.Id, updatedStatus);

		// Assert

		_mockCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<Status>>(), updatedStatus, It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}
}