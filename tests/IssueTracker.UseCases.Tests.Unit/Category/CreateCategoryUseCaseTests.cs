
namespace IssueTracker.UseCases.Category;

[ExcludeFromCodeCoverage]
public class CreateCategoryUseCaseTests
{

	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

	public CreateCategoryUseCaseTests()
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
				x.CreateAsync(It.IsAny<CategoryModel>()), Times.Once);

	}



	[Fact(DisplayName = "CreateCategoryUseCase InValid Data Test")]
	public async Task ExecuteAsync_With_InValidData_Should_ReturnArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();
		const string expectedParamName = "category";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}
}
