using IssueTracker.PlugIns.Mongo.Contracts;
using IssueTracker.PlugIns.Mongo.DataAccess;

using Moq;

using System;
using System.Threading.Tasks;

using Xunit;

namespace IssueTracker.PlugIns.Mongo.Tests.Unit.DataAccess
{
	public class CategoryMongoRepositoryTests
	{
		private MockRepository mockRepository;

		private Mock<IMongoDbContextFactory> mockMongoDbContextFactory;

		public CategoryMongoRepositoryTests()
		{
			this.mockRepository = new MockRepository(MockBehavior.Strict);

			this.mockMongoDbContextFactory = this.mockRepository.Create<IMongoDbContextFactory>();
		}

		private CategoryMongoRepository CreateCategoryMongoRepository()
		{
			return new CategoryMongoRepository(
					this.mockMongoDbContextFactory.Object);
		}

		[Fact]
		public async Task GetCategoryByIdAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var categoryMongoRepository = this.CreateCategoryMongoRepository();
			string categoryId = null;

			// Act
			var result = await categoryMongoRepository.GetCategoryByIdAsync(
				categoryId);

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}

		[Fact]
		public async Task GetCategoriesAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var categoryMongoRepository = this.CreateCategoryMongoRepository();

			// Act
			var result = await categoryMongoRepository.GetCategoriesAsync();

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}

		[Fact]
		public async Task CreateCategoryAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var categoryMongoRepository = this.CreateCategoryMongoRepository();
			CategoryModel category = null;

			// Act
			await categoryMongoRepository.CreateCategoryAsync(
				category);

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}

		[Fact]
		public async Task UpdateCategoryAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var categoryMongoRepository = this.CreateCategoryMongoRepository();
			CategoryModel category = null;

			// Act
			await categoryMongoRepository.UpdateCategoryAsync(
				category);

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}
	}
}
