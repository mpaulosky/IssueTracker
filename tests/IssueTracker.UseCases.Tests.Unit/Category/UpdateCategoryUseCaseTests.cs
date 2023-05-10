namespace IssueTracker.UseCases.Tests.Unit.Category;

[ExcludeFromCodeCoverage]
public class UpdateCategoryUseCaseTests
{

	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

	public UpdateCategoryUseCaseTests()
	{

		_categoryRepositoryMock = new Mock<ICategoryRepository>();

	}

	private UpdateCategoryUseCase CreateUseCase()
	{

		return new UpdateCategoryUseCase(_categoryRepositoryMock.Object);

	}

	[Fact(DisplayName = "EditCategoryUseCase With Valid Data Test")]
	public async Task ExecuteAsync_WithValidData_ShouldEditItem_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();

		var category = FakeCategory.GetCategories(1).First();
		category.CategoryDescription = "Description";

		// Act
		await sut.ExecuteAsync(category);

		// Assert
		_categoryRepositoryMock.Verify(x =>
				x.UpdateAsync(It.IsAny<CategoryModel>()), Times.Once);

	}

	[Fact(DisplayName = "EditCategoryUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnNull_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();
		
		// Act
		// Assert
		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!));
		
	}
	
}
