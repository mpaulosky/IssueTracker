using IssueTracker.PlugIns.Mongo.Contracts;
using IssueTracker.PlugIns.Mongo.DataAccess;

using Moq;

using System;
using System.Threading.Tasks;

using Xunit;

namespace IssueTracker.PlugIns.Mongo.Tests.Unit.DataAccess
{
	public class UserMongoRepositoryTests
	{
		private MockRepository mockRepository;

		private Mock<IMongoDbContextFactory> mockMongoDbContextFactory;

		public UserMongoRepositoryTests()
		{
			this.mockRepository = new MockRepository(MockBehavior.Strict);

			this.mockMongoDbContextFactory = this.mockRepository.Create<IMongoDbContextFactory>();
		}

		private UserMongoRepository CreateUserMongoRepository()
		{
			return new UserMongoRepository(
					this.mockMongoDbContextFactory.Object);
		}

		[Fact]
		public async Task GetUserByIdAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var userMongoRepository = this.CreateUserMongoRepository();
			string itemId = null;

			// Act
			var result = await userMongoRepository.GetUserByIdAsync(
				itemId);

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}

		[Fact]
		public async Task GetUsersAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var userMongoRepository = this.CreateUserMongoRepository();

			// Act
			var result = await userMongoRepository.GetUsersAsync();

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}

		[Fact]
		public async Task CreateUserAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var userMongoRepository = this.CreateUserMongoRepository();
			UserModel user = null;

			// Act
			await userMongoRepository.CreateUserAsync(
				user);

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}

		[Fact]
		public async Task UpdateUserAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var userMongoRepository = this.CreateUserMongoRepository();
			UserModel user = null;

			// Act
			await userMongoRepository.UpdateUserAsync(
				user);

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}

		[Fact]
		public async Task GetUserByAuthenticationIdAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var userMongoRepository = this.CreateUserMongoRepository();
			string userObjectIdentifierId = null;

			// Act
			var result = await userMongoRepository.GetUserByAuthenticationIdAsync(
				userObjectIdentifierId);

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}
	}
}
