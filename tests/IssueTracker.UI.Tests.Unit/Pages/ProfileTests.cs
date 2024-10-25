﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     ProfileTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI.Tests.Unit
// =============================================

using AngleSharp.Dom;

namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class ProfileTests : TestContext
{
	private readonly Mock<ICommentRepository> _commentRepositoryMock;
	private readonly List<CommentModel>? _expectedComments;
	private readonly List<IssueModel>? _expectedIssues;
	private readonly UserModel? _expectedUser;
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<IUserRepository> _userRepositoryMock;

	public ProfileTests()
	{
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_commentRepositoryMock = new Mock<ICommentRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();

		_expectedUser = FakeUser.GetNewUser(true);
		_expectedIssues = FakeIssue.GetIssues(5).ToList();
		_expectedComments = FakeComment.GetComments(5).ToList();
	}

	private IRenderedComponent<Profile> ComponentUnderTest()
	{
		SetupMocks();
		SetMemoryCache();
		RegisterServices();

		IRenderedComponent<Profile> component = RenderComponent<Profile>();

		return component;
	}

	[Fact]
	public void Profile_With_NullLoggedInUser_Should_ThrowArgumentNullException_Test()
	{
		// Arrange
		const string expectedParamName = "userObjectIdentifierId";
		const string expectedMessage = "Value cannot be null.?*";

		SetAuthenticationAndAuthorization(false, false);

		// Act
		Func<IRenderedComponent<Profile>> cut = ComponentUnderTest;

		// Assert
		cut.Should()
			.Throw<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact]
	public void Profile_With_ClosePageClick_Should_NavigateToTheIndexPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/";

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<Profile> cut = ComponentUnderTest();

		cut.Find("#close-page").Click();

		// Assert
		FakeNavigationManager navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	[Fact]
	public void Profile_With_ValidIssuesAndComments_Should_DisplayTheIssuesAndComments_Test()
	{
		// Arrange
		foreach (IssueModel? issue in _expectedIssues!)
		{
			issue.Author = new BasicUserModel(_expectedUser!);
			issue.ApprovedForRelease = true;
			issue.Archived = false;
			issue.Rejected = false;
		}

		foreach (CommentModel? comment in _expectedComments!)
		{
			comment.Author = new BasicUserModel(_expectedUser!);
			comment.Archived = false;
		}

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<Profile> cut = ComponentUnderTest();
		List<IElement> issueDivs = cut.FindAll("div.issue-container").ToList();
		List<IElement> commentDivs = cut.FindAll("div.comment-item-container").ToList();

		// Assert
		issueDivs.Count.Should().Be(5);
		commentDivs.Count.Should().Be(5);
	}

	private void SetupMocks()
	{
		_issueRepositoryMock
			.Setup(x => x
				.GetByUserAsync(_expectedUser!.Id))
			.ReturnsAsync(_expectedIssues!);

		_userRepositoryMock
			.Setup(x => x
				.GetFromAuthenticationAsync(It.IsAny<string>()))!
			.ReturnsAsync(_expectedUser);

		_commentRepositoryMock
			.Setup(x => x
				.GetByUserAsync(It.IsAny<string>()))
			.ReturnsAsync(_expectedComments!);
	}

	private void SetAuthenticationAndAuthorization(bool isAdmin, bool isAuth)
	{
		TestAuthorizationContext authContext = this.AddTestAuthorization();

		if (isAuth)
		{
			authContext.SetAuthorized(_expectedUser!.DisplayName);
			authContext.SetClaims(
				new Claim("objectidentifier", _expectedUser.Id)
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

		Services.AddSingleton<ICommentService>(
			new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object));

		Services.AddSingleton<IUserService>(new UserService(_userRepositoryMock.Object));
	}

	private void SetMemoryCache()
	{
		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);
	}
}