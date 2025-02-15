﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     SetStatusComponentTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI.Tests.Unit
// =============================================

namespace IssueTracker.UI.Components;

[ExcludeFromCodeCoverage]
public class SetStatusComponentTests : TestContext
{
	private readonly IssueModel _expectedIssue;
	private readonly UserModel _expectedUser;

	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IIssueService> _issueServiceMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<IStatusRepository> _statusRepositoryMock;

	public SetStatusComponentTests()
	{
		_issueServiceMock = new Mock<IIssueService>();
		_issueRepositoryMock = new Mock<IIssueRepository>();

		_statusRepositoryMock = new Mock<IStatusRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();

		_expectedUser = FakeUser.GetNewUser(true);
		_expectedIssue = FakeIssue.GetNewIssue(true);
	}

	private IRenderedComponent<SetStatusComponent> ComponentUnderTest()
	{
		SetupMocks();
		SetMemoryCache();
		RegisterServices();

		IRenderedComponent<SetStatusComponent> component = RenderComponent<SetStatusComponent>(parameter =>
		{
			parameter.Add(p => p.Issue, _expectedIssue);
		});

		return component;
	}

	[Fact(DisplayName = "SetStatusComponent not Admin")]
	public void SetStatusComponent_With_NotAdmin_Should_NotDisplaysArchiveButton_Test()
	{
		// Arrange
		const string expected = "";

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<SetStatusComponent> cut = ComponentUnderTest();

		// Assert 
		cut.MarkupMatches(expected);
	}

	[Fact(DisplayName = "SetStatusComponent is Admin")]
	public void SetStatusComponent_With_Admin_Should_DisplayTheStatusForm_Test()
	{
		// Arrange
		const string expected =
			"""
			<div class="issue-container">
				<div class="status-layout flex-container">
					<button class="btn btn-status btn-status-status fw-bold">Set Status</button>
					<button id="answered" class="btn btn-status btn-status-answered">
						answered
					</button>
					<button id="inwork" class="btn btn-status btn-status-inwork">
						in work
					</button>
					<button id="watching" class="btn btn-status btn-status-watching">
						watching
					</button>
					<button id="dismissed" class="btn btn-status btn-status-dismissed">
						dismissed
					</button>
				</div>
			</div>
			""";

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<SetStatusComponent> cut = ComponentUnderTest();

		// Assert 
		cut.MarkupMatches(expected);
	}

	[Theory(DisplayName = "SetStatusComponent work setting the statuses")]
	[InlineData("#answered", "Answered")]
	[InlineData("#inwork", "InWork")]
	[InlineData("#watching", "Watching")]
	[InlineData("#dismissed", "Dismissed")]
	public void SetupStatusComponent_With_Statuses_Should_AllowSettingEachStatus_TestAsync(
		string value,
		string expectedStatusName)
	{
		//Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<SetStatusComponent> cut = ComponentUnderTest();
		cut.Find(value).Click();
		cut.Find("#confirm-status-change").Click();
		IssueModel result = cut.Instance.Issue;

		//Assert
		result.IssueStatus.StatusName.Should().Be(expectedStatusName);

		_issueRepositoryMock.Verify(x =>
			x.UpdateAsync(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);
	}

	private void SetupMocks()
	{
		_issueServiceMock.Setup(x => x
			.UpdateIssue(It.IsAny<IssueModel>())).Verifiable();

		_issueRepositoryMock.Setup(x => x
			.UpdateAsync(It.IsAny<string>(), It.IsAny<IssueModel>())).Verifiable();

		_statusRepositoryMock.Setup(x => x
			.GetAllAsync()).ReturnsAsync(FakeStatus.GetStatuses().ToList());
	}

	private void SetAuthenticationAndAuthorization(bool isAdmin, bool isAuth)
	{
		TestAuthorizationContext authContext = this.AddTestAuthorization();

		if (isAuth)
		{
			authContext.SetAuthorized(_expectedUser.DisplayName);
			authContext.SetClaims(
				new Claim("objectidentifier", _expectedUser.ObjectIdentifier)
			);
		}

		if (isAdmin)
		{
			authContext.SetPolicies("Admin");
		}
	}

	private void RegisterServices()
	{
		Services.AddSingleton<IIssueService>(
			new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));

		Services.AddSingleton<IStatusService>(
			new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object));
	}

	private void SetMemoryCache()
	{
		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);
	}
}