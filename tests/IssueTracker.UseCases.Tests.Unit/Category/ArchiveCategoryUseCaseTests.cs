namespace IssueTracker.UseCases.Tests.Unit.Category;

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
		var _sut = CreateUseCase();
		CategoryModel? category = FakeCategory.GetCategories(1).First();

		// Act
		await _sut.ExecuteAsync(category);

		// Assert
		_categoryRepositoryMock.Verify(x =>
				x.UpdateCategoryAsync(It.IsAny<CategoryModel>()), Times.Once);
	}

	[Fact(DisplayName = "ArchiveCategoryUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_WithInValidCategoryModel_ShouldReturnNull_TestAsync()
	{

		// Arrange
		var _sut = this.CreateUseCase();

		CategoryModel? category = null;

		// Act
		await _sut.ExecuteAsync(category);

		// Assert
		_categoryRepositoryMock.Verify(x =>
				x.UpdateCategoryAsync(It.IsAny<CategoryModel>()), Times.Never);

	}

}
