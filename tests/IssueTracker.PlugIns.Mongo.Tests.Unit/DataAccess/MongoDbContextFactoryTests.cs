using IssueTracker.PlugIns.Mongo.DataAccess;
using IssueTracker.PlugIns.Mongo.Helpers;

using Moq;

using System;

using Xunit;

namespace IssueTracker.PlugIns.Mongo.Tests.Unit.DataAccess
{
	public class MongoDbContextFactoryTests
	{
		private MockRepository mockRepository;

		private Mock<DatabaseSettings> mockDatabaseSettings;

		public MongoDbContextFactoryTests()
		{
			this.mockRepository = new MockRepository(MockBehavior.Strict);

			this.mockDatabaseSettings = this.mockRepository.Create<DatabaseSettings>();
		}

		private MongoDbContextFactory CreateFactory()
		{
			return new MongoDbContextFactory(
					this.mockDatabaseSettings.Object);
		}

		[Fact]
		public void GetCollection_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var factory = this.CreateFactory();
			string name = null;

			// Act
			var result = factory.GetCollection(
				name);

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}
	}
}
