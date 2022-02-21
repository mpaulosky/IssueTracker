using FluentAssertions;

using IssueTrackerLibrary.Contracts;
using IssueTrackerLibrary.DataAccess;
using IssueTrackerLibrary.Models;

using IssueTrackerLibraryUnitTests.Fixtures;

using MongoDB.Driver;

using Moq;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace IssueTrackerLibraryUnitTests.UserRepositoryTests;

[ExcludeFromCodeCoverage]
public class UserRepositoryTests
{
	private readonly Mock<IMongoCollection<User>> _mockCollection;
	private readonly Mock<IMongoDbContext> _mockContext;
	private readonly Mock<IAsyncCursor<User>> _userCursor;
	private List<User> _list = new List<User>();

	public UserRepositoryTests()
	{
		_userCursor = new Mock<IAsyncCursor<User>>();
		_userCursor
			.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
			.Returns(true)
			.Returns(false);
		_userCursor
			.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
			.Returns(Task.FromResult(true))
			.Returns(Task.FromResult(false));

		_mockCollection = new Mock<IMongoCollection<User>>();
		_mockCollection.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<User>>(),
			It.IsAny<FindOptions<User, User>>(),
			It.IsAny<CancellationToken>())).ReturnsAsync(_userCursor.Object);

		_mockContext = new Mock<IMongoDbContext>();
		_mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name)).Returns(_mockCollection.Object);
	}

	[Fact()]
	public async Task GetUser_With_ValidContext_Should_Returns_OneUser_Test()
	{
		// Arrange

		const string userId = "5dc1039a1521eaa36835e541";
		const string objectIdentifier = "5dc1039a1521eaa36835e542";
		const string firstName = "Jim";
		const string lastName = "Text";
		const string displayName = "jimtest";
		const string email = "jim.test@test.com";

		var expected = TestFixtures.GetUser(userId, objectIdentifier, firstName, lastName, displayName, email);

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<User> { expected };

		_userCursor.Setup(_ => _.Current).Returns(_list);

		var sut = new UserRepository(_mockContext.Object);

		//Act

		var result = await sut.Get(userId);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
			It.IsAny<FindOptions<User>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Id.Should().BeSameAs(userId);
		result.ObjectIdentifier.Should().BeSameAs(objectIdentifier);
		result.FirstName.Should().BeSameAs(firstName);
		result.LastName.Should().BeSameAs(lastName);
		result.DisplayName.Should().BeSameAs(displayName);
		result.EmailAddress.Should().BeSameAs(email);
		result.AuthoredComments.Should().HaveCount(0);
		result.AuthoredIssues.Should().HaveCount(0);
		result.VotedOnComments.Should().HaveCount(0);
	}

	[Fact()]
	public async Task GetUserFromAuthentication_With_ValidContext_Should_Returns_OneUser_Test()
	{
		// Arrange

		const string userId = "5dc1039a1521eaa36835e541";
		const string objectIdentifier = "5dc1039a1521eaa36835e542";
		const string firstName = "Jim";
		const string lastName = "Text";
		const string displayName = "jimtest";
		const string email = "jim.test@test.com";

		var expected = TestFixtures.GetUser(userId, objectIdentifier, firstName, lastName, displayName, email);

		_mockCollection.Object.InsertOne(expected);

		_list = new List<User> { expected };

		_userCursor.Setup(_ => _.Current).Returns(_list);

		var sut = new UserRepository(_mockContext.Object);

		//Act

		var result = await sut.GetUserFromAuthentication(objectIdentifier);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
			It.IsAny<FindOptions<User>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Id.Should().BeSameAs(userId);
		result.ObjectIdentifier.Should().BeSameAs(objectIdentifier);
		result.FirstName.Should().BeSameAs(firstName);
		result.LastName.Should().BeSameAs(lastName);
		result.DisplayName.Should().BeSameAs(displayName);
		result.EmailAddress.Should().BeSameAs(email);
		result.AuthoredComments.Should().HaveCount(0);
		result.AuthoredIssues.Should().HaveCount(0);
		result.VotedOnComments.Should().HaveCount(0);
	}

	[Fact()]
	public async Task GetUsers_With_ValidContext_Should_Return_AListOfUsers_Test()
	{
		// Arrange

		var expected = TestFixtures.GetUsers().ToList();

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

	[Fact()]
	public async Task Create_With_InvalidItem_Should_Returns_ArgumentNullException_Test()
	{
		// Arrange

		var expected = TestFixtures.GetUsers().ToList();

		await _mockCollection.Object.InsertManyAsync(expected);

		_list = new List<User>(expected);

		_userCursor.Setup(_ => _.Current).Returns(_list);

		var sut = new UserRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Create(null!));
	}


	[Fact()]
	public async Task Update_With_InvalidItemOrId_Should_Returns_An_Exception_Test()
	{
		// Arrange

		const string userId = "5dc1039a1521eaa36835e541";
		const string objectIdentifier = "5dc1039a1521eaa36835e542";
		const string firstName = "Jim";
		const string lastName = "Text";
		const string displayName = "jimtest";
		const string email = "jim.test@test.com";

		var expected = TestFixtures.GetUser(userId, objectIdentifier, firstName, lastName, displayName, email);

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<User>();

		_userCursor.Setup(_ => _.Current).Returns(_list);

		var sut = new UserRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentException>(() => sut.Update(null!, expected));
		await Assert.ThrowsAsync<ArgumentException>(() => sut.Update("", expected));
		await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Update(userId, null!));
	}
}