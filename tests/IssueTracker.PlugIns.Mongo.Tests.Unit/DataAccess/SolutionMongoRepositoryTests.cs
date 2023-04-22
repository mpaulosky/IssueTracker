using IssueTracker.PlugIns.Mongo.Contracts;
using IssueTracker.PlugIns.Mongo.DataAccess;

using Moq;

using System;
using System.Threading.Tasks;

using Xunit;

namespace IssueTracker.PlugIns.Mongo.Tests.Unit.DataAccess;

public class SolutionMongoRepositoryTests
{
	private MockRepository mockRepository;

	private Mock<IMongoDbContextFactory> mockMongoDbContextFactory;

	public SolutionMongoRepositoryTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockMongoDbContextFactory = this.mockRepository.Create<IMongoDbContextFactory>();
	}

	private SolutionRepository CreateSolutionMongoRepository()
	{
		return new SolutionRepository(
				this.mockMongoDbContextFactory.Object);
	}

	[Fact]
	public async Task CreateSolutionAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var solutionMongoRepository = this.CreateSolutionMongoRepository();
		SolutionModel solution = null;

		// Act
		await solutionMongoRepository.CreateSolutionAsync(
			solution);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task GetSolutionByIssueIdAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var solutionMongoRepository = this.CreateSolutionMongoRepository();
		string issueId = null;

		// Act
		var result = await solutionMongoRepository.GetSolutionsByIssueIdAsync(
			issueId);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task GetSolutionsAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var solutionMongoRepository = this.CreateSolutionMongoRepository();

		// Act
		var result = await solutionMongoRepository.GetSolutionsAsync();

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task GetSolutionsByUserIdAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var solutionMongoRepository = this.CreateSolutionMongoRepository();
		string userId = null;

		// Act
		var result = await solutionMongoRepository.GetSolutionsByUserIdAsync(
			userId);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task UpdateSolutionAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var solutionMongoRepository = this.CreateSolutionMongoRepository();
		SolutionModel solution = null;

		// Act
		await solutionMongoRepository.UpdateSolutionAsync(
			solution);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
