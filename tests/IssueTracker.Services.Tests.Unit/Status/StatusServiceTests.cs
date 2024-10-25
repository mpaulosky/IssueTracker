﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     StatusServiceTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services.Tests.Unit
// =============================================

namespace IssueTracker.Services.Status;

[ExcludeFromCodeCoverage]
public class StatusServiceTests
{
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<IStatusRepository> _statusRepositoryMock;

	public StatusServiceTests()
	{
		_statusRepositoryMock = new Mock<IStatusRepository>();
		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
	}

	private StatusService UnitUnderTest()
	{
		return new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object);
	}

	[Fact(DisplayName = "Archive Status With Invalid Status Throws Exception")]
	public async Task ArchiveStatus_With_Invalid_Status_Should_Return_ArgumentNullException_TestAsync()
	{
		// Arrange
		StatusService sut = UnitUnderTest();

		// Act
		Func<Task> act = async () => await sut.ArchiveStatus(null!);

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName("status")
			.WithMessage("Value cannot be null. (Parameter 'status')");
	}

	[Fact(DisplayName = "Archive Status With Valid Values")]
	public async Task ArchiveStatus_With_Valid_Values_Should_Return_Test()
	{
		// Arrange
		StatusService sut = UnitUnderTest();
		StatusModel expected = FakeStatus.GetNewStatus(true);

		// Act
		await sut.ArchiveStatus(expected);

		// Assert
		sut.Should().NotBeNull();
		expected.Id.Should().Be(expected.Id);

		_statusRepositoryMock
			.Verify(x =>
				x.ArchiveAsync(It.IsAny<StatusModel>()), Times.Once);
	}

	[Fact(DisplayName = "Create Status With Valid Values")]
	public async Task CreateStatus_With_Valid_Values_Should_Return_Test()
	{
		// Arrange
		StatusService sut = UnitUnderTest();

		StatusModel status = FakeStatus.GetNewStatus();

		// Act
		await sut.CreateStatus(status);

		// Assert
		sut.Should().NotBeNull();
		status.Id.Should().NotBeNull();

		_statusRepositoryMock
			.Verify(x =>
				x.CreateAsync(It.IsAny<StatusModel>()), Times.Once);
	}

	[Fact(DisplayName = "Create Status With Invalid Status Throws Exception")]
	public async Task CreateStatus_With_Invalid_Status_Should_Return_ArgumentNullException_TestAsync()
	{
		// Arrange
		StatusService sut = UnitUnderTest();
		const string expectedParamName = "status";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.CreateStatus(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Delete Status")]
	public async Task DeleteStatus_With_Valid_Value_Should_Delete_the_Status_TestAsync()
	{
		// Arrange
		StatusService sut = UnitUnderTest();

		StatusModel status = FakeStatus.GetNewStatus(true);

		// Act
		await sut.DeleteStatus(status);

		// Assert
		sut.Should().NotBeNull();

		_statusRepositoryMock
			.Verify(x =>
				x.ArchiveAsync(It.IsAny<StatusModel>()), Times.Once);
	}

	[Fact(DisplayName = "Delete Status With InValid Status Throws Exception")]
	public async Task DeleteStatus_With_Invalid_Data_Should_Throw_ArgumentNullException_TestAsync()
	{
		// Arrange
		StatusService sut = UnitUnderTest();
		const string expectedParamName = "status";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.DeleteStatus(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Get Status With Valid Id")]
	public async Task GetStatus_With_Valid_Id_Should_Return_Expected_Status_Test()
	{
		//Arrange
		StatusService sut = UnitUnderTest();

		StatusModel expected = FakeStatus.GetNewStatus(true);

		_statusRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(expected);

		//Act
		StatusModel result = await sut.GetStatus(expected.Id);

		//Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.StatusName.Should().Be(expected.StatusName);
		result.StatusDescription.Should().Be(expected.StatusDescription);
	}

	[Theory(DisplayName = "Get Status With Invalid Id")]
	[InlineData(null, "statusId", "Value cannot be null.?*")]
	[InlineData("", "statusId", "The value cannot be an empty string.?*")]
	public async Task GetStatus_With_Invalid_Id_Should_Return_An_ArgumentException_TestAsync(string value,
		string expectedParamName, string expectedMessage)
	{
		// Arrange
		StatusService sut = UnitUnderTest();

		// Act
		Func<Task> act = async () => { await sut.GetStatus(value); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Get Statuses")]
	public async Task GetStatuses_Should_Return_A_List_Of_Statuses_Test()
	{
		//Arrange
		StatusService sut = UnitUnderTest();

		const int expectedCount = 4;

		IEnumerable<StatusModel> expected = FakeStatus.GetStatuses();

		_statusRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		//Act
		List<StatusModel> results = await sut.GetStatuses();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Statuses with cache")]
	public async Task GetStatuses_With_Memory_Cache_Should_A_List_Of_Statuses_Test()
	{
		//Arrange
		StatusService sut = UnitUnderTest();

		const int expectedCount = 4;

		IEnumerable<StatusModel> expected = FakeStatus.GetStatuses();

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
		List<StatusModel> results = await sut.GetStatuses();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Update Status With Valid Status")]
	public async Task UpdateStatus_With_A_Valid_Status_Should_Succeed_Test()
	{
		// Arrange
		StatusService sut = UnitUnderTest();

		StatusModel updatedStatus = FakeStatus.GetNewStatus(true);

		// Act
		await sut.UpdateStatus(updatedStatus);

		// Assert
		sut.Should().NotBeNull();

		_statusRepositoryMock
			.Verify(x =>
				x.UpdateAsync(It.IsAny<string>(), It.IsAny<StatusModel>()), Times.Once);
	}

	[Fact(DisplayName = "Update With Invalid Status")]
	public async Task UpdateStatus_With_Invalid_Status_Should_Return_ArgumentNullException_Test()
	{
		// Arrange
		StatusService sut = UnitUnderTest();
		const string expectedParamName = "status";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.UpdateStatus(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	private delegate void OutDelegate<in TIn, TOut>(TIn input, out TOut output);
}