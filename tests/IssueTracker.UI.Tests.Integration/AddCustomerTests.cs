
////-----------------------------------------------------------------------
//// <copyright File="AddCustomerTests"
////	Company="mpaulosky">
////	Author: Matthew Paulosky
////	Copyright (c) 2022. All rights reserved.
//// </copyright>
////-----------------------------------------------------------------------

//using IssueTracker.Library.Models;

//namespace IssueTracker.UI.Tests.Integration;

//[Collection("Test collection")]
//public class AddCustomerTests
//{

//	private readonly SharedTestContext _testContext;

//	private readonly Faker<UserModel> _customerGenerator = new Faker<UserModel>()
//			.RuleFor(x => x.EmailAddress, faker => faker.Person.Email)
//			.RuleFor(x => x.FirstName, faker => faker.Person.FirstName)
//			.RuleFor(x => x.LastName, faker => faker.Person.LastName);


//	public AddCustomerTests(SharedTestContext testContext)
//	{

//		_testContext = testContext;

//	}

//	[Fact]
//	public async Task Create_CreateCustomer_WhenDataIsValid()
//	{

//		// Arrange
//		var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
//		{

//			BaseURL = SharedTestContext.AppUrl

//		});

//		await page.GotoAsync("add-customer");

//		var customer = _customerGenerator.Generate();

//		// Act
//		//await page.FillAsync("input[id=fullname]", customer.FullName);
//		//await page.FillAsync("input[id=email]", customer.Email);
//		//await page.FillAsync("input[id=github-username]", customer.GitHubUsername);
//		//await page.FillAsync("input[id=dob]", customer.DateOfBirth.ToString("yyyy-MM-dd"));

//		//await page.ClickAsync("button[type=submit]");

//		// Assert
//		var linkElement = page.Locator("article>p>a").First;
//		var link = await linkElement.GetAttributeAsync("href");
//		await page.GotoAsync(link!);

//		//(await page.Locator("p[id=fullname-field]").InnerTextAsync()).Should().Be(customer.FullName);
//		//(await page.Locator("p[id=email-field]").InnerTextAsync()).Should().Be(customer.Email);
//		//(await page.Locator("p[id=github-username-field]").InnerTextAsync()).Should().Be(customer.GitHubUsername);
//		//(await page.Locator("p[id=dob-field]").InnerTextAsync()).Should().Be(customer.DateOfBirth.ToString("dd/MM/yyyy"));

//	}

//	[Fact]
//	public async Task Create_ShowsError_WhenEmailIsInvalid()
//	{
//		// Arrange
//		var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
//		{
//			BaseURL = SharedTestContext.AppUrl
//		});
//		await page.GotoAsync("add-customer");
//		var customer = _customerGenerator.Generate();

//		// Act
//		////await page.FillAsync("input[id=fullname]", customer.FullName);
//		////await page.FillAsync("input[id=email]", "notanemail");
//		////await page.FillAsync("input[id=github-username]", customer.GitHubUsername);
//		////await page.FillAsync("input[id=dob]", customer.DateOfBirth.ToString("yyyy-MM-dd"));

//		// Assert
//		var element = page.Locator("li.validation-message").First;
//		var text = await element.InnerTextAsync();
//		text.Should().Be("Invalid email format");
//	}
//}
