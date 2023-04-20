using IssueTracker.PlugIns.Mongo.Contracts;
using IssueTracker.PlugIns.Mongo.DataAccess;

using Moq;

using System;
using System.Threading.Tasks;

using Xunit;

namespace IssueTracker.PlugIns.Mongo.Tests.Unit.DataAccess
{
	public class StatusMongoRepositoryTests
	{
		private MockRepository mockRepository;

		private Mock<IMongoDbContextFactory> mockMongoDbContextFactory;

		public StatusMongoRepositoryTests()
		{
			this.mockRepository = new MockRepository(MockBehavior.Strict);

			this.mockMongoDbContextFactory = this.mockRepository.Create<IMongoDbContextFactory>();
		}

		private StatusMongoRepository CreateStatusMongoRepository()
		{
			return new StatusMongoRepository(
					this.mockMongoDbContextFactory.Object);
		}

		[Fact]
		public async Task CreateStatusAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var statusMongoRepository = this.CreateStatusMongoRepository();
			StatusModel status = null;

			// Act
			await statusMongoRepository.CreateStatusAsync(
				status);

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}

		[Fact]
		public async Task GetStatusByIdAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var statusMongoRepository = this.CreateStatusMongoRepository();
			string itemId = null;

			// Act
			var result = await statusMongoRepository.GetStatusByIdAsync(
				itemId);

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}

		[Fact]
		public async Task GetStatusesAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var statusMongoRepository = this.CreateStatusMongoRepository();

			// Act
			var result = await statusMongoRepository.GetStatusesAsync();

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}

		[Fact]
		public async Task UpdateStatusAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var statusMongoRepository = this.CreateStatusMongoRepository();
			StatusModel status = null;

			// Act
			await statusMongoRepository.UpdateStatusAsync(
				status);

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}

		[Fact]
		public async Task DeleteStatusAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var statusMongoRepository = this.CreateStatusMongoRepository();
			StatusModel status = null;

			// Act
			await statusMongoRepository.DeleteStatusAsync(
				status);

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}
	}
}
