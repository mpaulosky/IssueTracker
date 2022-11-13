namespace IssueTracker.UI.Pages
{
	[ExcludeFromCodeCoverage]
	public class ProfileTests : TestContext
	{
		private readonly Mock<ICommentRepository> _commentRepositoryMock;
		private readonly Mock<IIssueRepository> _issueRepositoryMock;
		private readonly Mock<IMemoryCache> _memoryCacheMock;
		private readonly Mock<ICacheEntry> _mockCacheEntry;
		private readonly Mock<IUserRepository> _userRepositoryMock;
		private List<CommentModel> _expectedComments;
		private List<IssueModel> _expectedIssues;
		private UserModel _expectedUser;

		public ProfileTests()
		{
			_issueRepositoryMock = new Mock<IIssueRepository>();
			_commentRepositoryMock = new Mock<ICommentRepository>();
			_userRepositoryMock = new Mock<IUserRepository>();

			_memoryCacheMock = new Mock<IMemoryCache>();
			_mockCacheEntry = new Mock<ICacheEntry>();
		}

		[Fact]
		public void Profile_With_NullLoggedInUser_Should_ThrowArgumentNullException_Test()
		{
			// Arrange
			this.AddTestAuthorization();

			RegisterServices();

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => RenderComponent<Profile>()).Message.Should()
				.Be("Value cannot be null. (Parameter 'userObjectIdentifierId')");
		}

		[Fact]
		public void Profile_With_ClosePageClick_Should_NavigateToTheIndexPage_Test()
		{
			// Arrange
			const string expectedUri = "http://localhost/";
			_expectedUser = TestUsers.GetKnownUser();
			_expectedIssues = TestIssues.GetIssues().ToList();
			_expectedComments = TestComments.GetComments().ToList();

			SetupMocks();
			SetMemoryCache();

			SetAuthenticationAndAuthorization(false, true);
			RegisterServices();

			// Act
			var cut = RenderComponent<Profile>();

			cut.Find("#close-page").Click();

			// Assert
			var navMan = Services.GetRequiredService<FakeNavigationManager>();
			navMan.Uri.Should().NotBeNull();
			navMan.Uri.Should().Be(expectedUri);
		}

		[Fact]
		public void Profile_With_ValidIssuesAndComments_Should_DisplayTheIssuesAndComments_Test()
		{
			// Arrange
			_expectedUser = TestUsers.GetKnownUser();
			_expectedIssues = TestIssues.GetIssues().ToList();
			_expectedComments = TestComments.GetComments().ToList();

			SetupMocks();
			SetMemoryCache();

			SetAuthenticationAndAuthorization(false, true);
			RegisterServices();

			// Act
			var cut = RenderComponent<Profile>();

			// Assert
			cut.MarkupMatches
			(
				@"
					<h1 class=""page-heading text-light text-uppercase mb-4"">jim test Profile</h1>
					<div class=""form-layout mb-3"">
						<div class=""close-button-section"">
							<button id=""close-page"" class=""btn btn-close"" ></button>
						</div>
						<div class=""form-layout mb-3"">
							<h2 class=""my-issue-heading"">jim test Account</h2>
							<p class=""my-issue-text"">
								<a href=""MicrosoftIdentity/Account/EditProfile"">Edit My Profile</a>
							</p>
						</div>
					</div>
					<div class=""form-layout mb-3"">
						<h2 class=""my-issue-heading"">Approved Issues</h2>
						<p class=""my-issue-text"">These are your issues that are currently available for comment.</p>
						<div class=""issue-container"">
							<div class=""issue-entry"">
								<div class=""issue-entry-category issue-entry-category-documentation"">
									<div class=""issue-entry-category-text"" >Documentation</div>
								</div>
								<div class=""issue-entry-text"">
									<div class=""issue-entry-text-title"" >Test Issue 2</div>
									<div class=""issue-entry-text-description"">A new test issue 2</div>
									<div class=""issue-entry-bottom"">
										<div class=""issue-entry-text-category"" diff:ignoreChildren>11.12.2022</div>
										<div class=""issue-entry-text-author"">Tester</div>
										<div class=""issue-entry-text-category""></div>
									</div>
								</div>
								<div class=""issue-entry-status issue-entry-status-answered"">
									<div class=""issue-entry-status-text"">Answered</div>
								</div>
							</div>
						</div>
					</div>
					<div class=""form-layout mb-3"">
						<h2 class=""my-issue-heading"">Pending Issues</h2>
						<p class=""my-issue-text"">These are your issues that are currently under review by admin.</p>
						<div class=""issue-container"">
							<div class=""issue-entry"">
								<div class=""issue-entry-category issue-entry-category-design"">
									<div class=""issue-entry-category-text"" >Design</div>
								</div>
								<div class=""issue-entry-text"">
									<div class=""issue-entry-text-title"" >Test Issue 6</div>
									<div class=""issue-entry-text-description"">A new test issue 6</div>
									<div class=""issue-entry-bottom"">
										<div class=""issue-entry-text-category"" diff:ignoreChildren>11.12.2022</div>
										<div class=""issue-entry-text-author"">Tester</div>
									</div>
								</div>
								<div class=""issue-entry-status issue-entry-status-none"">
									<div class=""issue-entry-status-text""></div>
								</div>
							</div>
						</div>
						<div class=""issue-container"">
							<div class=""issue-entry"">
								<div class=""issue-entry-category issue-entry-category-miscellaneous"">
									<div class=""issue-entry-category-text"" >Miscellaneous</div>
								</div>
								<div class=""issue-entry-text"">
									<div class=""issue-entry-text-title"" >Test Issue 3</div>
									<div class=""issue-entry-text-description"">A new test issue 3</div>
									<div class=""issue-entry-bottom"">
										<div class=""issue-entry-text-category"" diff:ignoreChildren>11.12.2022</div>
										<div class=""issue-entry-text-author"">Tester</div>
									</div>
								</div>
								<div class=""issue-entry-status issue-entry-status-watching"">
									<div class=""issue-entry-status-text"">Watching</div>
								</div>
							</div>
						</div>
						<div class=""issue-container"">
							<div class=""issue-entry"">
								<div class=""issue-entry-category issue-entry-category-design"">
									<div class=""issue-entry-category-text"" >Design</div>
								</div>
								<div class=""issue-entry-text"">
									<div class=""issue-entry-text-title"" >Test Issue 1</div>
									<div class=""issue-entry-text-description"">A new test issue 1</div>
									<div class=""issue-entry-bottom"">
										<div class=""issue-entry-text-category"" diff:ignoreChildren>11.12.2022</div>
										<div class=""issue-entry-text-author"">jim test</div>
									</div>
								</div>
								<div class=""issue-entry-status issue-entry-status-watching"">
									<div class=""issue-entry-status-text"">Watching</div>
								</div>
							</div>
						</div>
					</div>
					<div class=""form-layout mb-3"">
						<h2 class=""my-issue-heading"">Rejected Issues</h2>
						<p>These are your issues that were rejected by the admin for being out of the scope of this application.</p>
						<div class=""issue-container"">
							<div class=""issue-entry"">
								<div class=""issue-entry-category issue-entry-category-clarification"">
									<div class=""issue-entry-category-text"" >Clarification</div>
								</div>
								<div class=""issue-entry-text"">
									<div class=""issue-entry-text-title"" >Test Issue 3</div>
									<div class=""issue-entry-text-description"">A new test issue 3</div>
									<div class=""issue-entry-bottom"">
										<div class=""issue-entry-text-category"" diff:ignoreChildren>11.12.2022</div>
										<div class=""issue-entry-text-author"">jim test</div>
										<div class=""issue-entry-text-category""></div>
									</div>
								</div>
								<div class=""issue-entry-status issue-entry-status-dismissed"">
									<div class=""issue-entry-status-text"">Dismissed</div>
								</div>
							</div>
						</div>
					</div>
					<div class=""form-layout mb-3"">
						<h2 class=""my-issue-heading"">Archived Issues</h2>
						<p>These are your issues that are archived for future review.</p>
						<div class=""issue-container"">
							<div class=""issue-entry"">
								<div class=""issue-entry-category issue-entry-category-implementation"">
									<div class=""issue-entry-category-text"" >Implementation</div>
								</div>
								<div class=""issue-entry-text"">
									<div class=""issue-entry-text-title"" >Test Issue 3</div>
									<div class=""issue-entry-text-description"">A new test issue 3</div>
									<div class=""issue-entry-bottom"">
										<div class=""issue-entry-text-category"" diff:ignoreChildren>11.12.2022</div>
										<div class=""issue-entry-text-author"">Tester</div>
										<div class=""issue-entry-text-category""></div>
									</div>
								</div>
								<div class=""issue-entry-status issue-entry-status-inwork"">
									<div class=""issue-entry-status-text"">In Work</div>
								</div>
							</div>
						</div>
					</div>
					<div class=""form-layout mb-3"">
						<h2 class=""my-issue-heading"">Comments</h2>
						<p class=""my-issue-text"">These are your comments that are currently active.</p>
						<div id=""comment-entry"">
							<div id=""vote"" class=""issue-detail-no-votes""  style=""grid-column-start: 1;"">
								<div class=""text-uppercase"">Click To</div>
								<span class=""oi oi-caret-top detail-upvote""></span>
								<div class=""text-uppercase"">UpVote</div>
							</div>
							<div>
								<div class=""issue-detail-text"">Test Comment 1</div>
								<div class=""comment-header"">
									<label class=""category-date"" diff:ignoreChildren>11.12.2022</label>
									<label class=""category-author"">TEST USER</label>
								</div>
							</div>
						</div>
						<div id=""comment-entry"">
							<div id=""vote"" class=""issue-detail-no-votes""  style=""grid-column-start: 1;"">
								<div class=""text-uppercase"">Awaiting</div>
								<span class=""oi oi-caret-top detail-upvote""></span>
								<div class=""text-uppercase"">UpVote</div>
							</div>
							<div>
								<div class=""issue-detail-text"">Test Comment 2</div>
								<div class=""comment-header"">
									<label class=""category-date"" diff:ignoreChildren>11.12.2022</label>
									<label class=""category-author"">JIM TEST</label>
								</div>
							</div>
						</div>
						<div id=""comment-entry"">
							<div id=""vote"" class=""issue-detail-voted""  style=""grid-column-start: 1;"">
								<div class=""text-uppercase"">02</div>
								<span class=""oi oi-caret-top detail-upvote""></span>
								<div class=""text-uppercase"">UpVotes</div>
							</div>
							<div>
								<div class=""issue-detail-text"">Test Comment 3</div>
								<div class=""comment-header"">
									<label class=""category-date"" diff:ignoreChildren>11.12.2022</label>
									<label class=""category-author"">TEST USER</label>
								</div>
							</div>
						</div>
					</div>"
			);
		}

		private void SetupMocks()
		{
			_issueRepositoryMock
				.Setup(x => x.GetIssuesByUser(_expectedUser.Id))
				.ReturnsAsync(_expectedIssues);

			_userRepositoryMock
				.Setup(x => x.GetUserFromAuthentication(It.IsAny<string>()))
				.ReturnsAsync(_expectedUser);

			_commentRepositoryMock
				.Setup(x => x.GetCommentsByUser(It.IsAny<string>()))
				.ReturnsAsync(_expectedComments);
		}

		private void SetAuthenticationAndAuthorization(bool isAdmin, bool isAuth)
		{
			var authContext = this.AddTestAuthorization();

			if (isAuth)
			{
				authContext.SetAuthorized(_expectedUser.DisplayName!);
				authContext.SetClaims(
					new Claim("objectidentifier", _expectedUser.Id!)
				);
			}

			if (isAdmin)
			{
				authContext.SetPolicies("Admin");
			}
		}

		private void RegisterServices()
		{
			Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
				_memoryCacheMock.Object));
			Services.AddSingleton<ICommentService>(new CommentService(_commentRepositoryMock.Object,
				_memoryCacheMock.Object));
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
}