using IssueTracker.PlugIns.Mongo.Contracts;
using IssueTracker.PlugIns.Mongo.DataAccess;

using Moq;

using System;
using System.Threading.Tasks;

using Xunit;

namespace IssueTracker.PlugIns.Mongo.Tests.Unit.DataAccess;

public class IssueMongoRepositoryTests
{
	private MockRepository mockRepository;

	private Mock<IMongoDbContextFactory> mockMongoDbContextFactory;

	public IssueMongoRepositoryTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockMongoDbContextFactory = this.mockRepository.Create<IMongoDbContextFactory>();
	}

	private IssueRepository CreateIssueMongoRepository()
	{
		return new IssueRepository(
				this.mockMongoDbContextFactory.Object);
	}

	[Fact]
	public async Task CreateIssueAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var issueMongoRepository = this.CreateIssueMongoRepository();
		IssueModel issue = null;

		// Act
		await issueMongoRepository.CreateIssueAsync(
			issue);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task GetIssuesAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var issueMongoRepository = this.CreateIssueMongoRepository();

		// Act
		var result = await issueMongoRepository.GetIssuesAsync();

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task GetIssuesWaitingForApprovalAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var issueMongoRepository = this.CreateIssueMongoRepository();

		// Act
		var result = await issueMongoRepository.GetIssuesWaitingForApprovalAsync();

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task GetIssuesApprovedAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var issueMongoRepository = this.CreateIssueMongoRepository();

		// Act
		var result = await issueMongoRepository.GetIssuesApprovedAsync();

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task GetIssuesByUserIdAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var issueMongoRepository = this.CreateIssueMongoRepository();
		string userId = null;

		// Act
		var result = await issueMongoRepository.GetIssuesByUserIdAsync(
			userId);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task UpdateIssueAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var issueMongoRepository = this.CreateIssueMongoRepository();
		IssueModel issue = null;

		// Act
		await issueMongoRepository.UpdateIssueAsync(
			issue);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task GetIssueByIdAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var issueMongoRepository = this.CreateIssueMongoRepository();
		string itemId = null;

		// Act
		var result = await issueMongoRepository.GetIssueByIdAsync(
			itemId);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
