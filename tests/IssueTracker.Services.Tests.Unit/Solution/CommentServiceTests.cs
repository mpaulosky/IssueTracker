// Copyright (c) 2023. All rights reserved.
// File Name :     SolutionServiceTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services.Tests.Unit

namespace IssueTracker.Services.Solution;

[ExcludeFromCodeCoverage]
public class SolutionServiceTests
{
	private readonly Mock<ISolutionRepository> _solutionRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;

	public SolutionServiceTests()
	{
		_solutionRepositoryMock = new Mock<ISolutionRepository>();
		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
	}

	private SolutionService UnitUnderTest()
	{
		return new SolutionService(_solutionRepositoryMock.Object, _memoryCacheMock.Object);
	}

	[Fact(DisplayName = "Create Solution With Valid Values")]
	public async Task CreateSolution_With_Valid_Values_Should_Return_Test()
	{
		// Arrange
		SolutionService sut = UnitUnderTest();

		SolutionModel solution = FakeSolution.GetNewSolution();

		// Act
		await sut.CreateSolution(solution);

		// Assert
		sut.Should().NotBeNull();
		solution.Id.Should().NotBeNull();

		_solutionRepositoryMock
			.Verify(x =>
				x.CreateAsync(It.IsAny<SolutionModel>()), Times.Once);
	}

	[Fact(DisplayName = "Create Solution With Invalid Solution Throws Exception")]
	public async Task Create_With_Invalid_Solution_Should_Return_ArgumentNullException_TestAsync()
	{
		// Arrange
		SolutionService sut = UnitUnderTest();
		const string expectedParamName = "solution";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.CreateSolution(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Get Solution With Valid Id")]
	public async Task GetSolution_With_Valid_Id_Should_Return_Expected_Solution_Test()
	{
		//Arrange
		SolutionService sut = UnitUnderTest();

		SolutionModel expected = FakeSolution.GetNewSolution(true);

		_solutionRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(expected);

		//Act
		SolutionModel result = await sut.GetSolution(expected.Id);

		//Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
	}

	[Theory(DisplayName = "Get Solution With Invalid Id")]
	[InlineData(null, "solutionId", "Value cannot be null.?*")]
	[InlineData("", "solutionId", "The value cannot be an empty string.?*")]
	public async Task GetSolution_With_Invalid_Id_Should_Return_An_ArgumentException_TestAsync(string value,
		string expectedParamName, string expectedMessage)
	{
		// Arrange
		SolutionService sut = UnitUnderTest();

		// Act
		Func<Task> act = async () => { await sut.GetSolution(value); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Get Solutions")]
	public async Task GetSolutions_Should_Return_A_List_Of_Solutions_Test()
	{
		//Arrange
		SolutionService sut = UnitUnderTest();

		const int expectedCount = 3;

		List<SolutionModel> expected = FakeSolution.GetSolutions(expectedCount).ToList();

		foreach (SolutionModel? solution in expected)
		{
			solution.Archived = false;
		}

		_solutionRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		//Act
		List<SolutionModel> results = await sut.GetSolutions();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Solutions with cache")]
	public async Task GetSolutions_With_Memory_Cache_Should_A_List_Of_Solutions_Test()
	{
		//Arrange
		SolutionService sut = UnitUnderTest();

		const int expectedCount = 3;

		IEnumerable<SolutionModel> expected = FakeSolution.GetSolutions(expectedCount);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		object whatever = expected;

		_memoryCacheMock
			.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out whatever!))
			.Callback(new OutDelegate<object, object>((object _, out object v) =>
				v = whatever)) // mocked value here (and/or breakpoint)
			.Returns(true);

		//Act
		List<SolutionModel> results = await sut.GetSolutions();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Users Solutions With Valid Id")]
	public async Task GetByUserAsync_With_A_Valid_Id_Should_Return_A_List_Of_User_Solutions_Test()
	{
		//Arrange
		SolutionService sut = UnitUnderTest();

		const int expectedCount = 2;
		List<SolutionModel> solutions = FakeSolution.GetSolutions(expectedCount).ToList();

		const string expectedUserId = "5dc1039a1521eaa36835e541";

		foreach (SolutionModel? solution in solutions)
		{
			solution.Author = new BasicUserModel(expectedUserId, "Jim", "Jones", "jimjones@test.com", "jimjones");
		}

		List<SolutionModel> expected = solutions.ToList();

		_solutionRepositoryMock.Setup(x => x.GetByUserAsync(It.IsAny<string>())).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		//Act
		List<SolutionModel> results = await sut.GetSolutionsByUser(expectedUserId);

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Users Solutions with memory cache")]
	public async Task GetByUserAsync_With_Cache_Should_Return_A_ListOfSolutions_TestAsync()
	{
		//Arrange
		SolutionService sut = UnitUnderTest();

		const int expectedCount = 3;
		List<SolutionModel> solutions = FakeSolution.GetSolutions(expectedCount).ToList();

		const string expectedUserId = "5dc1039a1521eaa36835e541";

		foreach (SolutionModel? solution in solutions)
		{
			solution.Author = new BasicUserModel(expectedUserId, "Jim", "Jones", "jimjones@test.com", "jimjones");
		}

		List<SolutionModel> expected = solutions.ToList();

		_solutionRepositoryMock.Setup(x => x.GetByUserAsync(It.IsAny<string>())).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		//Act
		List<SolutionModel> results = await sut.GetSolutionsByUser(expectedUserId);

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Theory(DisplayName = "Get Solutions By User With Invalid Id")]
	[InlineData(null, "userId", "Value cannot be null.?*")]
	[InlineData("", "userId", "The value cannot be an empty string.?*")]
	public async Task GetUsersSolutions_With_Empty_String_Users_Id_Should_Return_An_ArgumentException_TestAsync(
		string value, string expectedParamName, string expectedMessage)
	{
		// Arrange
		SolutionService sut = UnitUnderTest();

		// Act
		Func<Task> act = async () => { await sut.GetSolutionsByUser(value); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Update Solution With Valid Solution")]
	public async Task UpdateSolution_With_A_Valid_Solution_Should_Succeed_Test()
	{
		// Arrange
		SolutionService sut = UnitUnderTest();

		SolutionModel updatedSolution = FakeSolution.GetNewSolution(true);

		// Act
		await sut.UpdateSolution(updatedSolution);

		// Assert
		sut.Should().NotBeNull();

		_solutionRepositoryMock
			.Verify(x =>
				x.UpdateAsync(It.IsAny<string>(), It.IsAny<SolutionModel>()), Times.Once);
	}

	[Fact(DisplayName = "Update With Invalid Solution")]
	public async Task UpdateSolution_With_Invalid_Solution_Should_Return_ArgumentNullException_Test()
	{
		// Arrange
		SolutionService sut = UnitUnderTest();
		const string expectedParamName = "solution";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.UpdateSolution(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Up Vote Solution")]
	public async Task UpVoteSolution_With_Valid_Inputs_Should_Be_Successful_Test()
	{
		// Arrange
		SolutionService sut = UnitUnderTest();

		const string testId = "5dc1039a1521eaa36835e543";

		SolutionModel solution = FakeSolution.GetNewSolution(true);

		// Act
		await sut.UpVoteSolution(solution.Id, testId);

		// Assert
		_solutionRepositoryMock
			.Verify(x =>
				x.UpVoteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
	}

	[Theory(DisplayName = "UpVote Solution With Invalid inputs")]
	[InlineData(null, "1", "solutionId", "Value cannot be null.?*")]
	[InlineData("1", null, "userId", "Value cannot be null.?*")]
	public async Task UpVoteSolution_With_Invalid_Inputs_Should_Return_An_ArgumentNullException_TestAsync(
		string solutionId,
		string userId,
		string expectedParamName,
		string expectedMessage)
	{
		// Arrange
		SolutionService sut = UnitUnderTest();

		// Act
		Func<Task> act = async () => { await sut.UpVoteSolution(solutionId, userId); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	private delegate void OutDelegate<in TIn, TOut>(TIn input, out TOut output);
}