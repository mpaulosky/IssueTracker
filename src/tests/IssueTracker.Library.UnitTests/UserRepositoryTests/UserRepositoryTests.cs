using FluentAssertions;

using IssueTrackerLibrary.Contracts;
using IssueTrackerLibrary.DataAccess;
using IssueTrackerLibrary.Models;

using IssueTracker.Library.UnitTests.Fixtures;

using MongoDB.Driver;

using Moq;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace IssueTracker.Library.UnitTests.UserRepositoryTests;

[ExcludeFromCodeCoverage]
public class UserRepositoryTests
{
	private readonly Mock<IMongoCollection<User>> _mockCollection;
	private readonly Mock<IMongoDbContext> _mockContext;
	private readonly Mock<IAsyncCursor<User>> _userCursor;
	private List<User> _list = new();

	public UserRepositoryTests()
	{
		_userCursor = TestFixtures.MockCursor<User>();

		_mockCollection = TestFixtures.MockCollection<User>(_userCursor);

		_mockContext = TestFixtures.MockContext<User>(_mockCollection);
	}

	[Fact(DisplayName = "Get User With Id")]
	public async Task GetUser_With_Valid_Id_Should_Returns_One_User_Test()
	{
		// Arrange

		var expected = TestUsers.GetKnownUser();

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<User> { expected };

		_userCursor.Setup(_ => _.Current).Returns(_list);

		var sut = new UserRepository(_mockContext.Object);

		//Act

		var result = await sut.Get(expected.Id);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
			It.IsAny<FindOptions<User>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
	}

	[Fact(DisplayName = "Get User With Invalid Id")]
	public async Task Get_With_Invalid_Id_Should_Return_Null_Result_TestAsync()
	{
		// Arrange

		var sut = new UserRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<IndexOutOfRangeException>(() => sut.Get(""));
	}

	[Fact(DisplayName = "GetUserFromAuthentication")]
	public async Task GetUserFromAuthentication_With_Valid_ObjectIdentifier_Should_Returns_One_User_Test()
	{
		// Arrange

		var expected = TestUsers.GetKnownUser();

		_mockCollection.Object.InsertOne(expected);

		_list = new List<User> { expected };

		_userCursor.Setup(_ => _.Current).Returns(_list);

		var sut = new UserRepository(_mockContext.Object);

		//Act

		var result = await sut.GetUserFromAuthentication(expected.ObjectIdentifier);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
			It.IsAny<FindOptions<User>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
	}

	[Fact(DisplayName = "Get Users")]
	public async Task GetUsers_With_Valid_Context_Should_Return_A_List_Of_Users_Test()
	{
		// Arrange

		var expected = TestUsers.GetUsers().ToList();

		await _mockCollection.Object.InsertManyAsync(expected);

		_list = new List<User>(expected);

		_userCursor.Setup(_ => _.Current).Returns(_list);

		var sut = new UserRepository(_mockContext.Object);

		// Act

		var result = await sut.Get().ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
			It.IsAny<FindOptions<User>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(3);
	}

	[Fact(DisplayName = "Create User")]
	public async Task Create_With_Valid_User_Should_Insert_A_New_User_TestAsync()
	{
		// Arrange

		var newUser = TestUsers.GetKnownUser();
		var sut = new UserRepository(_mockContext.Object);

		// Act

		await sut.Create(newUser);

		// Assert

		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c.InsertOneAsync(newUser, null, default(CancellationToken)), Times.Once);
	}

	[Fact(DisplayName = "Update User")]
	public async Task Update_With_A_Valid_Id_And_User_Should_UpdateUser_Test() 
	{
		// Arrange

		var expected = TestUsers.GetKnownUser();

		var updatedUser = TestUsers.GetUser(expected.Id, expected.ObjectIdentifier, "James", expected.LastName, expected.DisplayName, "james.test@test.com");

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<User>();

		_userCursor.Setup(_ => _.Current).Returns(_list);

		var sut = new UserRepository(_mockContext.Object);

		// Act

		await sut.Update(updatedUser.Id, updatedUser);

		// Assert

		_mockCollection.Verify(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<User>>(), updatedUser, It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>()), Times.Once);

	}
}