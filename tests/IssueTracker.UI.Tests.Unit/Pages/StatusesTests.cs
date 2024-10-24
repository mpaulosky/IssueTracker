// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     StatusesTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI.Tests.Unit
// =============================================

using AngleSharp.Dom;

namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class StatusesTests : TestContext
{
	private readonly IEnumerable<StatusModel> _expectedStatuses;
	private readonly UserModel _expectedUser;

	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;

	private readonly Mock<IStatusRepository> _statusRepositoryMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;

	public StatusesTests()
	{
		_statusRepositoryMock = new Mock<IStatusRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();

		_expectedUser = FakeUser.GetNewUser(true);
		_expectedStatuses = FakeStatus.GetStatuses();
	}

	private IRenderedComponent<Statuses> ComponentUnderTest()
	{
		SetupMocks();
		SetMemoryCache();
		RegisterServices();

		IRenderedComponent<Statuses> component = RenderComponent<Statuses>();

		return component;
	}

	[Fact]
	public void Statuses_CloseButton_Should_WhenClickedNavigateToIndexPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/";

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Statuses> cut = ComponentUnderTest();

		cut.Find("#close-page").Click();

		// Assert
		FakeNavigationManager navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	[Fact]
	public void Statuses_Should_DisplayMarkup_Test()
	{
		// Arrange
		const string expectedHtml =
			"""
			<h1 class="page-heading text-uppercase mb-4" >Statuses</h1>
			<div class="row justify-content-center create-form" >
			  <div class="form-layout col-xl-9 col-lg-11" >
			    <div class="close-button-section" >
			      <button id:ignore class="btn btn-close"  ></button>
			    </div>
			    <div class="" >
			      <button tabindex="0" type="button" class="rz-button rz-button-md rz-variant-filled rz-success rz-shade-default mt-2 mb-4" id:ignore>
			        <span class="rz-button-box">
			          <i class="rz-button-icon-left rzi">add_circle_outline</i>
			          <span class="rz-button-text">Add New Status</span>
			        </span>
			      </button>
			      <div class="rz-data-grid rz-has-pager rz-datatable  rz-datatable-scrollable " id:ignore tabindex="0">
			        <div class="rz-data-grid-data" tabindex="-1">
			          <table class="rz-grid-table rz-grid-table-fixed rz-grid-table-striped  ">
			            <colgroup>
			              <col id:ignore style="width:120px">
			              <col id:ignore style="width:200px">
			              <col id:ignore style="width:156px">
			            </colgroup>
			            <thead>
			              <tr>
			                <th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column    rz-text-align-left" scope="col" style="width:120px" >
			                  <div  >
			                    <span class="rz-column-title" title="Status Name">
			                      <span class="rz-column-title-content">Status Name</span>
			                      <span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
			                    </span>
			                  </div>
			                </th>
			                <th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column    rz-text-align-left" scope="col" style="width:200px" >
			                  <div  >
			                    <span class="rz-column-title" title="Description">
			                      <span class="rz-column-title-content">Description</span>
			                      <span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
			                    </span>
			                  </div>
			                </th>
			                <th rowspan="1" colspan="1" class="rz-unselectable-text     rz-text-align-right" scope="col" style="width:156px;text-align:right" >
			                  <div  >
			                    <span class="rz-column-title">
			                      <span class="rz-column-title-content"></span>
			                    </span>
			                  </div>
			                </th>
			              </tr>
			            </thead>
			            <tbody>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">Answered</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue was accepted and the corresponding item was created.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">Watching</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue is interesting. We are watching to see how much interest there is in it.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">InWork</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue was accepted and it is in work.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">Dismissed</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue was not something that we are going to undertake.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore >
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			            </tbody>
			          </table>
			        </div>
			      </div>
			    </div>
			  </div>
			</div>
			""";

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Statuses> cut = ComponentUnderTest();

		// Assert
		cut.MarkupMatches(expectedHtml);
	}

	[Fact]
	public void Statuses_AddNewStatusButton_Should_InsertNewInputRow_Test()
	{
		// Arrange
		const string expectedHtml =
			"""
			<h1 class="page-heading text-uppercase mb-4" >Statuses</h1>
			<div class="row justify-content-center create-form" >
			  <div class="form-layout col-xl-9 col-lg-11" >
			    <div class="close-button-section" >
			      <button id:ignore class="btn btn-close"  ></button>
			    </div>
			    <div class="" >
			      <button disabled="" tabindex="-1" type="button" class="rz-button rz-button-md rz-variant-filled rz-success rz-shade-default rz-state-disabled mt-2 mb-4" id:ignore  >
			        <span class="rz-button-box">
			          <i class="rz-button-icon-left rzi">add_circle_outline</i>
			          <span class="rz-button-text">Add New Status</span>
			        </span>
			      </button>
			      <div class="rz-data-grid rz-has-pager rz-datatable  rz-datatable-scrollable " id:ignore tabindex="0"   >
			        <div class="rz-data-grid-data" tabindex="-1">
			          <table class="rz-grid-table rz-grid-table-fixed rz-grid-table-striped  ">
			            <colgroup>
			              <col id:ignore style="width:120px">
			              <col id:ignore style="width:200px">
			              <col id:ignore style="width:156px">
			            </colgroup>
			            <thead>
			              <tr>
			                <th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column rz-text-align-left" scope="col" style="width:120px" >
			                  <div  >
			                    <span class="rz-column-title" title="Status Name">
			                      <span class="rz-column-title-content">Status Name</span>
			                      <span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
			                    </span>
			                  </div>
			                </th>
			                <th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column rz-text-align-left" scope="col" style="width:200px" >
			                  <div  >
			                    <span class="rz-column-title" title="Description">
			                      <span class="rz-column-title-content">Description</span>
			                      <span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
			                    </span>
			                  </div>
			                </th>
			                <th rowspan="1" colspan="1" class="rz-unselectable-text     rz-text-align-right" scope="col" style="width:156px;text-align:right" >
			                  <div  >
			                    <span class="rz-column-title">
			                      <span class="rz-column-title-content"></span>
			                    </span>
			                  </div>
			                </th>
			              </tr>
			            </thead>
			            <tbody>
			              <tr class="rz-data-row rz-datatable-edit ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">
			                    <input id:ignore name="StatusName" style="width: 100%; display: block;" class="rz-textbox valid rz-state-empty" tabindex="0" autocomplete="on" value=""  >
			                  </span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">
			                    <input id:ignore name="StatusDescription" style="width: 100%; display: block;" class="rz-textbox valid rz-state-empty" tabindex="0" autocomplete="on" value=""  >
			                  </span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button" class="rz-button rz-button-md rz-variant-flat rz-success rz-shade-default rz-button-icon-only" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">check</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button" class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only my-1 ms-1" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">close</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button" class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">Answered</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue was accepted and the corresponding item was created.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">Watching</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue is interesting. We are watching to see how much interest there is in it.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">InWork</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue was accepted and it is in work.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">Dismissed</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue was not something that we are going to undertake.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore >
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			            </tbody>
			          </table>
			        </div>
			      </div>
			    </div>
			  </div>
			</div>
			""";

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Statuses> cut = ComponentUnderTest();

		cut.FindAll("button")[1].Click(); // Add New Status Button

		// Assert
		cut.MarkupMatches(expectedHtml);
	}

	[Fact]
	public void Statuses_AddNewCategoryButton_Should_CancelInsertOnCancel_Test()
	{
		// Arrange
		const string expectedHtml =
			"""
			<h1 class="page-heading text-uppercase mb-4" >Statuses</h1>
			<div class="row justify-content-center create-form" >
			  <div class="form-layout col-xl-9 col-lg-11" >
			    <div class="close-button-section" >
			      <button id:ignore class="btn btn-close"  ></button>
			    </div>
			    <div class="" >
			      <button tabindex="0" type="button" class="rz-button rz-button-md rz-variant-filled rz-success rz-shade-default mt-2 mb-4" id:ignore>
			        <span class="rz-button-box">
			          <i class="rz-button-icon-left rzi">add_circle_outline</i>
			          <span class="rz-button-text">Add New Status</span>
			        </span>
			      </button>
			      <div class="rz-data-grid rz-has-pager rz-datatable  rz-datatable-scrollable " id:ignore tabindex="0">
			        <div class="rz-data-grid-data" tabindex="-1">
			          <table class="rz-grid-table rz-grid-table-fixed rz-grid-table-striped  ">
			            <colgroup>
			              <col id:ignore style="width:120px">
			              <col id:ignore style="width:200px">
			              <col id:ignore style="width:156px">
			            </colgroup>
			            <thead>
			              <tr>
			                <th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column    rz-text-align-left" scope="col" style="width:120px" >
			                  <div  >
			                    <span class="rz-column-title" title="Status Name">
			                      <span class="rz-column-title-content">Status Name</span>
			                      <span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
			                    </span>
			                  </div>
			                </th>
			                <th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column    rz-text-align-left" scope="col" style="width:200px" >
			                  <div  >
			                    <span class="rz-column-title" title="Description">
			                      <span class="rz-column-title-content">Description</span>
			                      <span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
			                    </span>
			                  </div>
			                </th>
			                <th rowspan="1" colspan="1" class="rz-unselectable-text     rz-text-align-right" scope="col" style="width:156px;text-align:right" >
			                  <div  >
			                    <span class="rz-column-title">
			                      <span class="rz-column-title-content"></span>
			                    </span>
			                  </div>
			                </th>
			              </tr>
			            </thead>
			            <tbody>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">Answered</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue was accepted and the corresponding item was created.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">Watching</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue is interesting. We are watching to see how much interest there is in it.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">InWork</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue was accepted and it is in work.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">Dismissed</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue was not something that we are going to undertake.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore>
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			            </tbody>
			          </table>
			        </div>
			      </div>
			    </div>
			  </div>
			</div>
			""";

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Statuses> cut = ComponentUnderTest();

		cut.FindAll("button")[1].Click(); // Add New Status Button

		List<IElement> buttons = cut.FindAll("button").ToList();

		buttons[3].Click(); // Cancel Button

		// Assert
		cut.MarkupMatches(expectedHtml);
	}

	[Fact]
	public void Statuses_AddNewStatusButton_Should_CreateNewStatus_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Statuses> cut = ComponentUnderTest();

		cut.FindAll("button")[1].Click(); // Add New Status Button

		IRefreshableElementCollection<IElement> inputs = cut.FindAll("input");
		inputs[0].Change("Test Name");
		inputs[1].Change("Test Description");

		IRefreshableElementCollection<IElement> btns = cut.FindAll("button");
		btns[2].Click(); // Click Submit Button

		// Assert
		_statusRepositoryMock
			.Verify(x =>
				x.CreateAsync(It.IsAny<StatusModel>()), Times.Once);
	}

	[Fact]
	public void Statuses_OnClickEditButton_Should_UpdateStatusOnSubmit_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Statuses> cut = ComponentUnderTest();

		cut.FindAll("button")[2].Click(); // Edit Button

		IRefreshableElementCollection<IElement> inputs = cut.FindAll("input");
		inputs[0].Change("Test Name");
		inputs[1].Change("Test Description");

		IRefreshableElementCollection<IElement> btns = cut.FindAll("button");
		btns[2].Click(); // Click Submit Button

		// Assert
		_statusRepositoryMock
			.Verify(x =>
				x.UpdateAsync(It.IsAny<string>(), It.IsAny<StatusModel>()), Times.Once);
	}

	[Fact]
	public void Statuses_OnClickDeleteButton_Should_Archive_The_Status_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Statuses> cut = ComponentUnderTest();

		cut.FindAll("button")[3].Click(); // Delete Button

		// Assert
		_statusRepositoryMock
			.Verify(x =>
				x.ArchiveAsync(It.IsAny<StatusModel>()), Times.Once);
	}

	[Fact]
	public void Statuses_OnClickEditButton_Should_CancelEditOnCancelClick_Test()
	{
		// Arrange
		const string expectedHtml =
			"""
			<h1 class="page-heading text-uppercase mb-4" >Statuses</h1>
			<div class="row justify-content-center create-form" >
			  <div class="form-layout col-xl-9 col-lg-11" >
			    <div class="close-button-section" >
			      <button id:ignore class="btn btn-close"  ></button>
			    </div>
			    <div class="" >
			      <button tabindex="0" type="button" class="rz-button rz-button-md rz-variant-filled rz-success rz-shade-default mt-2 mb-4" id:ignore  >
			        <span class="rz-button-box">
			          <i class="rz-button-icon-left rzi">add_circle_outline</i>
			          <span class="rz-button-text">Add New Status</span>
			        </span>
			      </button>
			      <div class="rz-data-grid rz-has-pager rz-datatable  rz-datatable-scrollable " id:ignore tabindex="0">
			        <div class="rz-data-grid-data" tabindex="-1">
			          <table class="rz-grid-table rz-grid-table-fixed rz-grid-table-striped  ">
			            <colgroup>
			              <col id:ignore style="width:120px">
			              <col id:ignore style="width:200px">
			              <col id:ignore style="width:156px">
			            </colgroup>
			            <thead>
			              <tr>
			                <th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column    rz-text-align-left" scope="col" style="width:120px" >
			                  <div  >
			                    <span class="rz-column-title" title="Status Name">
			                      <span class="rz-column-title-content">Status Name</span>
			                      <span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
			                    </span>
			                  </div>
			                </th>
			                <th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column    rz-text-align-left" scope="col" style="width:200px" >
			                  <div  >
			                    <span class="rz-column-title" title="Description">
			                      <span class="rz-column-title-content">Description</span>
			                      <span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
			                    </span>
			                  </div>
			                </th>
			                <th rowspan="1" colspan="1" class="rz-unselectable-text     rz-text-align-right" scope="col" style="width:156px;text-align:right" >
			                  <div  >
			                    <span class="rz-column-title">
			                      <span class="rz-column-title-content"></span>
			                    </span>
			                  </div>
			                </th>
			              </tr>
			            </thead>
			            <tbody>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">Answered</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue was accepted and the corresponding item was created.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore  >
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore  >
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">Watching</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue is interesting. We are watching to see how much interest there is in it.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore >
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore >
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">InWork</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue was accepted and it is in work.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore  >
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore  >
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			              <tr class="rz-data-row  ">
			                <td style="width:120px" >
			                  <span class="rz-cell-data">Dismissed</span>
			                </td>
			                <td style="width:200px" >
			                  <span class="rz-cell-data">The issue was not something that we are going to undertake.</span>
			                </td>
			                <td style="width:156px;text-align:right;" >
			                  <span class="rz-cell-data">
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore  >
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">edit</i>
			                      </span>
			                    </button>
			                    <button tabindex="0" type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore  >
			                      <span class="rz-button-box">
			                        <i class="rz-button-icon-left rzi">delete</i>
			                      </span>
			                    </button>
			                  </span>
			                </td>
			              </tr>
			            </tbody>
			          </table>
			        </div>
			      </div>
			    </div>
			  </div>
			</div>
			""";

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Statuses> cut = ComponentUnderTest();

		List<IElement> buttons = cut.FindAll("button").ToList();
		buttons[2].Click(); // Edit Button

		List<IElement> buttons2 = cut.FindAll("button").ToList();
		buttons2[3].Click(); // Click Cancel Button

		// Assert
		cut.MarkupMatches(expectedHtml);
	}

	private void SetupMocks()
	{
		_statusRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_expectedStatuses);

		_userRepositoryMock.Setup(x => x.GetFromAuthenticationAsync(It.IsAny<string>())).ReturnsAsync(_expectedUser);
	}

	private void SetAuthenticationAndAuthorization(bool isAdmin, bool isAuth)
	{
		TestAuthorizationContext authContext = this.AddTestAuthorization();

		if (isAuth)
		{
			authContext.SetAuthorized(_expectedUser.DisplayName);
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
		Services.AddSingleton<IStatusService>(new StatusService(_statusRepositoryMock.Object,
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