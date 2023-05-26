namespace IssueTracker.UseCases.Category;

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

	[Fact(DisplayName = "UpdateCategoryUseCase With Valid Data Test")]
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

	[Fact(DisplayName = "UpdateCategoryUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnArgumentNullException_TestAsync()
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
