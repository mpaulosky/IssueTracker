﻿namespace IssueTracker.Library.Services;

[ExcludeFromCodeCoverage]
public class StatusServiceTests
{
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<IStatusRepository> _statusRepositoryMock;
	private StatusService _sut;

	public StatusServiceTests()
	{

		_statusRepositoryMock = new Mock<IStatusRepository>();
		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
		_sut = new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object);

	}

	[Fact(DisplayName = "Create Status With Valid Values")]
	public async Task CreateStatus_With_Valid_Values_Should_Return_Test()
	{

		// Arrange

		StatusModel status = TestStatuses.GetNewStatus();

		_sut = new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		await _sut.CreateStatus(status);

		// Assert

		_sut.Should().NotBeNull();

		_statusRepositoryMock
			.Verify(x =>
				x.CreateStatus(It.IsAny<StatusModel>()), Times.Once);

	}

	[Fact(DisplayName = "Create Status With Invalid Status Throws Exception")]
	public async Task CreateStatus_With_Invalid_Status_Should_Return_ArgumentNullException_TestAsync()
	{

		// Arrange

		_sut = new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateStatus(null));

	}

	[Fact(DisplayName = "Delete Status")]
	public async Task DeleteStatus_With_Valid_Value_Should_Delete_the_Status_TestAsync()
	{

		// Arrange

		StatusModel status = TestStatuses.GetKnownStatus();

		_sut = new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		await _sut.DeleteStatus(status);

		// Assert

		_sut.Should().NotBeNull();

		_statusRepositoryMock
			.Verify(x =>
				x.DeleteStatus(It.IsAny<StatusModel>()), Times.Once);

	}

	[Fact(DisplayName = "Delete Status With InValid Status Throws Exception")]
	public async Task DeleteStatus_With_Invalid_Data_Should_Throw_ArgumentNullException_TestAsync()
	{

		// Arrange

		_sut = new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.DeleteStatus(null));

	}

	[Fact(DisplayName = "Get Status With Valid Id")]
	public async Task GetStatus_With_Valid_Id_Should_Return_Expected_Status_Test()
	{

		//Arrange

		StatusModel expected = TestStatuses.GetKnownStatus();

		_statusRepositoryMock.Setup(x => x.GetStatus(It.IsAny<string>())).ReturnsAsync(expected);

		_sut = new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		StatusModel result = await _sut.GetStatus(expected!.Id!);

		//Assert

		result.Should().NotBeNull();
		result!.Id!.Should().Be(expected!.Id!);

	}

	[Fact(DisplayName = "Get Status With Empty String Id")]
	public async Task GetStatus_With_Empty_String_Id_Should_Return_An_ArgumentException_TestAsync()
	{

		// Arrange

		_sut = new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetStatus(""));

	}

	[Fact(DisplayName = "Get Status With Null Id")]
	public async Task GetStatus_With_Null_Id_Should_Return_An_ArgumentNullException_TestAsync()
	{

		// Arrange

		_sut = new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetStatus(null));

	}

	[Fact(DisplayName = "Get Statuses")]
	public async Task GetStatuses_Should_Return_A_List_Of_Statuses_Test()
	{

		//Arrange

		const int expectedCount = 4;

		IEnumerable<StatusModel> expected = TestStatuses.GetStatuses();

		_statusRepositoryMock.Setup(x => x.GetStatuses()).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		_sut = new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		List<StatusModel> results = await _sut.GetStatuses();

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);

	}

	[Fact(DisplayName = "Get Statuses with cache")]
	public async Task GetStatuses_With_Memory_Cache_Should_A_List_Of_Statuses_Test()
	{

		//Arrange

		const int expectedCount = 4;

		IEnumerable<StatusModel> expected = TestStatuses.GetStatuses();


		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		object whatever = expected;
		_memoryCacheMock
			.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out whatever))
			.Callback(new OutDelegate<object, object>((object _, out object v) =>
				v = whatever)) // mocked value here (and/or breakpoint)
			.Returns(true);

		_sut = new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		List<StatusModel> results = await _sut.GetStatuses();

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);

	}

	[Fact(DisplayName = "Update Status With Valid Status")]
	public async Task UpdateStatus_With_A_Valid_Status_Should_Succeed_Test()
	{

		// Arrange

		StatusModel updatedStatus = TestStatuses.GetUpdatedStatus();

		_sut = new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		await _sut.UpdateStatus(updatedStatus);

		// Assert

		_sut.Should().NotBeNull();

		_statusRepositoryMock
			.Verify(x =>
				x.UpdateStatus(It.IsAny<string>(), It.IsAny<StatusModel>()), Times.Once);

	}

	[Fact(DisplayName = "Update With Invalid Status")]
	public async Task UpdateStatus_With_Invalid_Status_Should_Return_ArgumentNullException_Test()
	{

		// Arrange

		_sut = new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.UpdateStatus(null));

	}

	private delegate void OutDelegate<in TIn, TOut>(TIn input, out TOut output);
}