
namespace IssueTracker.UseCases.Tests.Unit.Category;

[ExcludeFromCodeCoverage]
public class CreateNewCategoryUseCaseTests
{

	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

	public CreateNewCategoryUseCaseTests()
	{

		_categoryRepositoryMock = new Mock<ICategoryRepository>();

	}

	private CreateCategoryUseCase CreateUseCase()
	{

		return new CreateCategoryUseCase(_categoryRepositoryMock.Object);

	}

	[Fact(DisplayName = "CreateCategoryUseCase Valid Data Test")]
	public async Task ExecuteAsync_WithValidCategoryModel_ShouldCreateCategory()
	{

		// Arrange
		var sut = CreateUseCase();

		CategoryModel category = FakeCategory.GetNewCategory();

		// Act
		await sut.ExecuteAsync(category);

		// Assert
		_categoryRepositoryMock.Verify(x =>
				x.CreateCategoryAsync(It.IsAny<CategoryModel>()), Times.Once);

	}



	[Fact(DisplayName = "CreateCategoryUseCase InValid Data Test")]
	public async Task ExecuteAsync_With_InValidData_Should_ReturnNull_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();

		CategoryModel? category = null;

		// Act
		await sut.ExecuteAsync(category: category);

		// Assert
		_categoryRepositoryMock.Verify(x =>
				x.CreateCategoryAsync(It.IsAny<CategoryModel>()), Times.Never);

	}

}
