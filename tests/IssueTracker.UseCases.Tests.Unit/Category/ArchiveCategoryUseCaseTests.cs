namespace IssueTracker.UseCases.Category;

[ExcludeFromCodeCoverage]
public class ArchiveCategoryUseCaseTests
{

	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

	public ArchiveCategoryUseCaseTests()
	{

		_categoryRepositoryMock = new Mock<ICategoryRepository>();

	}

	private ArchiveCategoryUseCase CreateUseCase()
	{

		return new ArchiveCategoryUseCase(_categoryRepositoryMock.Object);

	}

	[Fact(DisplayName = "ArchiveCategoryUseCase With Valid Data Test")]
	public async Task ExecuteAsync_WithValidCategoryModel_ShouldUpdateAsArchived_TestAsync()
	{
		// Arrange
		var sut = CreateUseCase();
		var category = FakeCategory.GetCategories(1).First();

		// Act
		await sut.ExecuteAsync(category);

		// Assert
		_categoryRepositoryMock.Verify(x =>
				x.ArchiveAsync(It.IsAny<CategoryModel>()), Times.Once);
	}

	[Fact(DisplayName = "ArchiveCategoryUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_WithInValidCategoryModel_ShouldReturnArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
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
