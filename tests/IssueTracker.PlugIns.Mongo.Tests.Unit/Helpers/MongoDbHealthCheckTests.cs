using IssueTracker.PlugIns.Mongo.Helpers;

using Moq;

using System;
using System.Threading.Tasks;

using Xunit;

namespace IssueTracker.PlugIns.Mongo.Tests.Unit.Helpers
{
	public class MongoDbHealthCheckTests
	{
		private MockRepository mockRepository;



		public MongoDbHealthCheckTests()
		{
			this.mockRepository = new MockRepository(MockBehavior.Strict);


		}

		private MongoDbHealthCheck CreateMongoDbHealthCheck()
		{
			return new MongoDbHealthCheck(
					TODO,
					TODO);
		}

		[Fact]
		public async Task CheckHealthAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var mongoDbHealthCheck = this.CreateMongoDbHealthCheck();
			HealthCheckContext context = null;
			CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

			// Act
			var result = await mongoDbHealthCheck.CheckHealthAsync(
				context,
				cancellationToken);

			// Assert
			Assert.True(false);
			this.mockRepository.VerifyAll();
		}
	}
}
