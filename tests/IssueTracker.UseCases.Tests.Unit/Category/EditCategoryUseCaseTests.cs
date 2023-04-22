namespace IssueTracker.UseCases.Tests.Unit.Category;

[ExcludeFromCodeCoverage]
public class EditCategoryUseCaseTests
{

	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

	public EditCategoryUseCaseTests()
	{

		_categoryRepositoryMock = new Mock<ICategoryRepository>();

	}

	private EditCategoryUseCase CreateUseCase()
	{

		return new EditCategoryUseCase(_categoryRepositoryMock.Object);

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
				x.UpdateCategoryAsync(It.IsAny<CategoryModel>()), Times.Once);

	}

	[Fact(DisplayName = "EditCategoryUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnNull_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();

		CategoryModel? category = null;

		// Act
		await sut.ExecuteAsync(category);

		// Assert
		_categoryRepositoryMock.Verify(x =>
				x.UpdateCategoryAsync(It.IsAny<CategoryModel>()), Times.Never);

	}

}
