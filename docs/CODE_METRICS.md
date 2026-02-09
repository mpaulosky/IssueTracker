<!-- markdownlint-capture -->
<!-- markdownlint-disable -->

# Code Metrics

This file is dynamically maintained by a bot, *please do not* edit this by hand. It represents
various [code metrics](https://aka.ms/dotnet/code-metrics), such as cyclomatic complexity, maintainability index, and so
on.

<div id='issuetrackerlibrary'></div>

## IssueTrackerLibrary :heavy_check_mark:

The *IssueTrackerLibrary.csproj* project file contains:

- 2 namespaces.
- 17 named types.
- 537 total lines of source code.
- Approximately 193 lines of executable code.
- The highest cyclomatic complexity is 3 :heavy_check_mark:.

<details>
<summary>
  <strong id="issuetrackerlibrary-dataaccess">
    IssueTrackerLibrary.DataAccess :heavy_check_mark:
  </strong>
</summary>
<br>

The `IssueTrackerLibrary.DataAccess` namespace contains 10 named types.

- 10 named types.
- 434 total lines of source code.
- Approximately 165 lines of executable code.
- The highest cyclomatic complexity is 3 :heavy_check_mark:.

<details>
<summary>
  <strong id="dbconnection">
    DbConnection :heavy_check_mark:
  </strong>
</summary>
<br>

- The `DbConnection` contains 14 members.
- 32 total lines of source code.
- Approximately 13 lines of executable code.
- The highest cyclomatic complexity is 2 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Field
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/DbConnection.cs#L7 "IConfiguration DbConnection._config")
| 100 | 0 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Field
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/DbConnection.cs#L9 "string DbConnection._connectionId")
| 93 | 0 :heavy_check_mark: | 0 | 0 | 1 / 1 |
| Field
| [8](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/DbConnection.cs#L8 "IMongoDatabase DbConnection._db")
| 100 | 0 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Method
| [24](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/DbConnection.cs#L24 "DbConnection.DbConnection(IConfiguration config)")
| 64 | 1 :heavy_check_mark: | 0 | 9 | 12 / 8 |
| Property
| [16](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/DbConnection.cs#L16 "MongoClient DbConnection.Client")
| 100 | 2 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Property
| [21](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/DbConnection.cs#L21 "IMongoCollection<CommentModel> DbConnection.CommentCollection")
| 100 | 2 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Property
| [15](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/DbConnection.cs#L15 "string DbConnection.CommentCollectionName")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 1 |
| Property
| [11](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/DbConnection.cs#L11 "string DbConnection.DbName")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [20](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/DbConnection.cs#L20 "IMongoCollection<IssueModel> DbConnection.IssueCollection")
| 100 | 2 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Property
| [14](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/DbConnection.cs#L14 "string DbConnection.IssueCollectionName")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 1 |
| Property
| [18](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/DbConnection.cs#L18 "IMongoCollection<StatusModel> DbConnection.StatusCollection")
| 100 | 2 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Property
| [12](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/DbConnection.cs#L12 "string DbConnection.StatusCollectionName")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 1 |
| Property
| [19](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/DbConnection.cs#L19 "IMongoCollection<UserModel> DbConnection.UserCollection")
| 100 | 2 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Property
| [13](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/DbConnection.cs#L13 "string DbConnection.UserCollectionName")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 1 |

<a href="#issuetrackerlibrary-dataaccess">:top: back to IssueTrackerLibrary.DataAccess</a>

</details>

<details>
<summary>
  <strong id="icommentdata">
    ICommentData :heavy_check_mark:
  </strong>
</summary>
<br>

- The `ICommentData` contains 8 members.
- 11 total lines of source code.
- Approximately 0 lines of executable code.
- The highest cyclomatic complexity is 1 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Method
| [12](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/ICommentData.cs#L12 "Task ICommentData.CreateComment(CommentModel comment)")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Method
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/ICommentData.cs#L7 "Task<List<CommentModel>> ICommentData.GetAllApprovedComments()")
| 100 | 1 :heavy_check_mark: | 0 | 3 | 1 / 0 |
| Method
| [5](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/ICommentData.cs#L5 "Task<List<CommentModel>> ICommentData.GetAllComments()")
| 100 | 1 :heavy_check_mark: | 0 | 3 | 1 / 0 |
| Method
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/ICommentData.cs#L9 "Task<List<CommentModel>> ICommentData.GetAllCommentsWaitingForApproval()")
| 100 | 1 :heavy_check_mark: | 0 | 3 | 1 / 0 |
| Method
| [8](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/ICommentData.cs#L8 "Task<CommentModel> ICommentData.GetComment(string id)")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Method
| [6](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/ICommentData.cs#L6 "Task<List<CommentModel>> ICommentData.GetUsersComments(string userId)")
| 100 | 1 :heavy_check_mark: | 0 | 3 | 1 / 0 |
| Method
| [10](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/ICommentData.cs#L10 "Task ICommentData.UpdateComment(CommentModel suggestion)")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Method
| [11](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/ICommentData.cs#L11 "Task ICommentData.UpvoteComment(string commentId, string userId)")
| 100 | 1 :heavy_check_mark: | 0 | 1 | 1 / 0 |

<a href="#issuetrackerlibrary-dataaccess">:top: back to IssueTrackerLibrary.DataAccess</a>

</details>

<details>
<summary>
  <strong id="idbconnection">
    IDbConnection :heavy_check_mark:
  </strong>
</summary>
<br>

- The `IDbConnection` contains 10 members.
- 13 total lines of source code.
- Approximately 0 lines of executable code.
- The highest cyclomatic complexity is 1 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Property
| [10](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IDbConnection.cs#L10 "MongoClient IDbConnection.Client")
| 100 | 1 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Property
| [14](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IDbConnection.cs#L14 "IMongoCollection<CommentModel> IDbConnection.CommentCollection")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Property
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IDbConnection.cs#L9 "string IDbConnection.CommentCollectionName")
| 100 | 1 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [5](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IDbConnection.cs#L5 "string IDbConnection.DbName")
| 100 | 1 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [13](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IDbConnection.cs#L13 "IMongoCollection<IssueModel> IDbConnection.IssueCollection")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Property
| [8](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IDbConnection.cs#L8 "string IDbConnection.IssueCollectionName")
| 100 | 1 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [11](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IDbConnection.cs#L11 "IMongoCollection<StatusModel> IDbConnection.StatusCollection")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Property
| [6](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IDbConnection.cs#L6 "string IDbConnection.StatusCollectionName")
| 100 | 1 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [12](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IDbConnection.cs#L12 "IMongoCollection<UserModel> IDbConnection.UserCollection")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Property
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IDbConnection.cs#L7 "string IDbConnection.UserCollectionName")
| 100 | 1 :heavy_check_mark: | 0 | 0 | 1 / 0 |

<a href="#issuetrackerlibrary-dataaccess">:top: back to IssueTrackerLibrary.DataAccess</a>

</details>

<details>
<summary>
  <strong id="iissuedata">
    IIssueData :heavy_check_mark:
  </strong>
</summary>
<br>

- The `IIssueData` contains 8 members.
- 11 total lines of source code.
- Approximately 0 lines of executable code.
- The highest cyclomatic complexity is 1 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Method
| [12](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IIssueData.cs#L12 "Task IIssueData.CreateSuggestion(IssueModel suggestion)")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Method
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IIssueData.cs#L7 "Task<List<IssueModel>> IIssueData.GetAllApprovedSuggestions()")
| 100 | 1 :heavy_check_mark: | 0 | 3 | 1 / 0 |
| Method
| [5](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IIssueData.cs#L5 "Task<List<IssueModel>> IIssueData.GetAllSuggestions()")
| 100 | 1 :heavy_check_mark: | 0 | 3 | 1 / 0 |
| Method
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IIssueData.cs#L9 "Task<List<IssueModel>> IIssueData.GetAllSuggestionsWaitingForApproval()")
| 100 | 1 :heavy_check_mark: | 0 | 3 | 1 / 0 |
| Method
| [8](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IIssueData.cs#L8 "Task<IssueModel> IIssueData.GetSuggestion(string id)")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Method
| [6](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IIssueData.cs#L6 "Task<List<IssueModel>> IIssueData.GetUsersSuggestions(string userId)")
| 100 | 1 :heavy_check_mark: | 0 | 3 | 1 / 0 |
| Method
| [10](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IIssueData.cs#L10 "Task IIssueData.UpdateSuggestion(IssueModel suggestion)")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Method
| [11](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IIssueData.cs#L11 "Task IIssueData.UpvoteSuggestion(string suggestionId, string userId)")
| 100 | 1 :heavy_check_mark: | 0 | 1 | 1 / 0 |

<a href="#issuetrackerlibrary-dataaccess">:top: back to IssueTrackerLibrary.DataAccess</a>

</details>

<details>
<summary>
  <strong id="istatusdata">
    IStatusData :heavy_check_mark:
  </strong>
</summary>
<br>

- The `IStatusData` contains 2 members.
- 5 total lines of source code.
- Approximately 0 lines of executable code.
- The highest cyclomatic complexity is 1 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Method
| [6](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IStatusData.cs#L6 "Task IStatusData.CreateStatus(StatusModel status)")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Method
| [5](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IStatusData.cs#L5 "Task<List<StatusModel>> IStatusData.GetAllStatuses()")
| 100 | 1 :heavy_check_mark: | 0 | 3 | 1 / 0 |

<a href="#issuetrackerlibrary-dataaccess">:top: back to IssueTrackerLibrary.DataAccess</a>

</details>

<details>
<summary>
  <strong id="iuserdata">
    IUserData :heavy_check_mark:
  </strong>
</summary>
<br>

- The `IUserData` contains 5 members.
- 8 total lines of source code.
- Approximately 0 lines of executable code.
- The highest cyclomatic complexity is 1 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Method
| [8](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IUserData.cs#L8 "Task IUserData.CreateUser(UserModel user)")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Method
| [6](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IUserData.cs#L6 "Task<UserModel> IUserData.GetUser(string id)")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Method
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IUserData.cs#L7 "Task<UserModel> IUserData.GetUserFromAuthentication(string objectId)")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Method
| [5](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IUserData.cs#L5 "Task<List<UserModel>> IUserData.GetUsers()")
| 100 | 1 :heavy_check_mark: | 0 | 3 | 1 / 0 |
| Method
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Contracts/IUserData.cs#L9 "Task IUserData.UpdateUser(UserModel user)")
| 100 | 1 :heavy_check_mark: | 0 | 2 | 1 / 0 |

<a href="#issuetrackerlibrary-dataaccess">:top: back to IssueTrackerLibrary.DataAccess</a>

</details>

<details>
<summary>
  <strong id="mongocommentdata">
    MongoCommentData :heavy_check_mark:
  </strong>
</summary>
<br>

- The `MongoCommentData` contains 14 members.
- 143 total lines of source code.
- Approximately 68 lines of executable code.
- The highest cyclomatic complexity is 3 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Field
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoCommentData.cs#L9 "IMemoryCache MongoCommentData._cache")
| 100 | 0 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Field
| [11](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoCommentData.cs#L11 "string MongoCommentData._cacheName")
| 93 | 0 :heavy_check_mark: | 0 | 0 | 1 / 1 |
| Field
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoCommentData.cs#L7 "IDbConnection MongoCommentData._db")
| 100 | 0 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Field
| [10](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoCommentData.cs#L10 "IMongoCollection<CommentModel> MongoCommentData._suggestions")
| 100 | 0 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Field
| [8](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoCommentData.cs#L8 "IUserData MongoCommentData._userData")
| 100 | 0 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Method
| [13](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoCommentData.cs#L13 "MongoCommentData.MongoCommentData(IDbConnection db, IUserData userData, IMemoryCache cache)")
| 75 | 1 :heavy_check_mark: | 0 | 5 | 7 / 4 |
| Method
| [120](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoCommentData.cs#L120 "Task MongoCommentData.CreateComment(CommentModel comment)")
| 58 | 1 :heavy_check_mark: | 0 | 13 | 27 / 14 |
| Method
| [49](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoCommentData.cs#L49 "Task<List<CommentModel>> MongoCommentData.GetAllApprovedComments()")
| 87 | 1 :heavy_check_mark: | 0 | 4 | 5 / 2 |
| Method
| [21](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoCommentData.cs#L21 "Task<List<CommentModel>> MongoCommentData.GetAllComments()")
| 68 | 2 :heavy_check_mark: | 0 | 8 | 13 / 7 |
| Method
| [61](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoCommentData.cs#L61 "Task<List<CommentModel>> MongoCommentData.GetAllCommentsWaitingForApproval()")
| 87 | 1 :heavy_check_mark: | 0 | 4 | 5 / 2 |
| Method
| [55](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoCommentData.cs#L55 "Task<CommentModel> MongoCommentData.GetComment(string id)")
| 80 | 1 :heavy_check_mark: | 0 | 6 | 5 / 3 |
| Method
| [35](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoCommentData.cs#L35 "Task<List<CommentModel>> MongoCommentData.GetUsersComments(string userId)")
| 68 | 2 :heavy_check_mark: | 0 | 8 | 13 / 7 |
| Method
| [67](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoCommentData.cs#L67 "Task MongoCommentData.UpdateComment(CommentModel suggestion)")
| 79 | 1 :heavy_check_mark: | 0 | 5 | 5 / 3 |
| Method
| [73](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoCommentData.cs#L73 "Task MongoCommentData.UpvoteComment(string commentId, string userId)")
| 51 | 3 :heavy_check_mark: | 0 | 14 | 46 / 25 |

<a href="#issuetrackerlibrary-dataaccess">:top: back to IssueTrackerLibrary.DataAccess</a>

</details>

<details>
<summary>
  <strong id="mongoissuedata">
    MongoIssueData :heavy_check_mark:
  </strong>
</summary>
<br>

- The `MongoIssueData` contains 14 members.
- 128 total lines of source code.
- Approximately 60 lines of executable code.
- The highest cyclomatic complexity is 2 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Field
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoIssueData.cs#L9 "IMemoryCache MongoIssueData._cache")
| 100 | 0 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Field
| [11](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoIssueData.cs#L11 "string MongoIssueData._cacheName")
| 93 | 0 :heavy_check_mark: | 0 | 0 | 1 / 1 |
| Field
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoIssueData.cs#L7 "IDbConnection MongoIssueData._db")
| 100 | 0 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Field
| [10](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoIssueData.cs#L10 "IMongoCollection<IssueModel> MongoIssueData._issues")
| 100 | 0 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Field
| [8](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoIssueData.cs#L8 "IUserData MongoIssueData._userData")
| 100 | 0 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Method
| [13](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoIssueData.cs#L13 "MongoIssueData.MongoIssueData(IDbConnection db, IUserData userData, IMemoryCache cache)")
| 75 | 1 :heavy_check_mark: | 0 | 5 | 7 / 4 |
| Method
| [105](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoIssueData.cs#L105 "Task MongoIssueData.CreateSuggestion(IssueModel suggestion)")
| 58 | 1 :heavy_check_mark: | 0 | 12 | 27 / 14 |
| Method
| [49](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoIssueData.cs#L49 "Task<List<IssueModel>> MongoIssueData.GetAllApprovedSuggestions()")
| 87 | 1 :heavy_check_mark: | 0 | 4 | 5 / 2 |
| Method
| [21](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoIssueData.cs#L21 "Task<List<IssueModel>> MongoIssueData.GetAllSuggestions()")
| 68 | 2 :heavy_check_mark: | 0 | 8 | 13 / 7 |
| Method
| [61](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoIssueData.cs#L61 "Task<List<IssueModel>> MongoIssueData.GetAllSuggestionsWaitingForApproval()")
| 87 | 1 :heavy_check_mark: | 0 | 4 | 5 / 2 |
| Method
| [55](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoIssueData.cs#L55 "Task<IssueModel> MongoIssueData.GetSuggestion(string id)")
| 80 | 1 :heavy_check_mark: | 0 | 6 | 5 / 3 |
| Method
| [35](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoIssueData.cs#L35 "Task<List<IssueModel>> MongoIssueData.GetUsersSuggestions(string userId)")
| 68 | 2 :heavy_check_mark: | 0 | 8 | 13 / 7 |
| Method
| [67](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoIssueData.cs#L67 "Task MongoIssueData.UpdateSuggestion(IssueModel suggestion)")
| 79 | 1 :heavy_check_mark: | 0 | 5 | 5 / 3 |
| Method
| [73](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoIssueData.cs#L73 "Task MongoIssueData.UpvoteSuggestion(string suggestionId, string userId)")
| 56 | 1 :heavy_check_mark: | 0 | 13 | 31 / 17 |

<a href="#issuetrackerlibrary-dataaccess">:top: back to IssueTrackerLibrary.DataAccess</a>

</details>

<details>
<summary>
  <strong id="mongostatusdata">
    MongoStatusData :heavy_check_mark:
  </strong>
</summary>
<br>

- The `MongoStatusData` contains 6 members.
- 31 total lines of source code.
- Approximately 11 lines of executable code.
- The highest cyclomatic complexity is 2 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Field
| [8](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoStatusData.cs#L8 "IMemoryCache MongoStatusData._cache")
| 100 | 0 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Field
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoStatusData.cs#L7 "IMongoCollection<StatusModel> MongoStatusData._statuses")
| 100 | 0 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Method
| [11](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoStatusData.cs#L11 "MongoStatusData.MongoStatusData(IDbConnection db, IMemoryCache cache)")
| 84 | 1 :heavy_check_mark: | 0 | 4 | 5 / 2 |
| Field
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoStatusData.cs#L9 "string MongoStatusData.CacheName")
| 93 | 0 :heavy_check_mark: | 0 | 0 | 1 / 1 |
| Method
| [31](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoStatusData.cs#L31 "Task MongoStatusData.CreateStatus(StatusModel status)")
| 96 | 1 :heavy_check_mark: | 0 | 4 | 4 / 1 |
| Method
| [17](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoStatusData.cs#L17 "Task<List<StatusModel>> MongoStatusData.GetAllStatuses()")
| 68 | 2 :heavy_check_mark: | 0 | 6 | 13 / 7 |

<a href="#issuetrackerlibrary-dataaccess">:top: back to IssueTrackerLibrary.DataAccess</a>

</details>

<details>
<summary>
  <strong id="mongouserdata">
    MongoUserData :heavy_check_mark:
  </strong>
</summary>
<br>

- The `MongoUserData` contains 7 members.
- 38 total lines of source code.
- Approximately 13 lines of executable code.
- The highest cyclomatic complexity is 1 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Field
| [5](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoUserData.cs#L5 "IMongoCollection<UserModel> MongoUserData._users")
| 100 | 0 :heavy_check_mark: | 0 | 2 | 1 / 0 |
| Method
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoUserData.cs#L7 "MongoUserData.MongoUserData(IDbConnection db)")
| 93 | 1 :heavy_check_mark: | 0 | 3 | 4 / 1 |
| Method
| [30](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoUserData.cs#L30 "Task MongoUserData.CreateUser(UserModel user)")
| 96 | 1 :heavy_check_mark: | 0 | 4 | 4 / 1 |
| Method
| [18](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoUserData.cs#L18 "Task<UserModel> MongoUserData.GetUser(string id)")
| 80 | 1 :heavy_check_mark: | 0 | 4 | 5 / 3 |
| Method
| [24](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoUserData.cs#L24 "Task<UserModel> MongoUserData.GetUserFromAuthentication(string objectId)")
| 80 | 1 :heavy_check_mark: | 0 | 4 | 5 / 3 |
| Method
| [12](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoUserData.cs#L12 "Task<List<UserModel>> MongoUserData.GetUsers()")
| 82 | 1 :heavy_check_mark: | 0 | 5 | 5 / 3 |
| Method
| [35](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/DataAccess/MongoUserData.cs#L35 "Task MongoUserData.UpdateUser(UserModel user)")
| 83 | 1 :heavy_check_mark: | 0 | 5 | 5 / 2 |

<a href="#issuetrackerlibrary-dataaccess">:top: back to IssueTrackerLibrary.DataAccess</a>

</details>

</details>

<details>
<summary>
  <strong id="issuetrackerlibrary-models">
    IssueTrackerLibrary.Models :heavy_check_mark:
  </strong>
</summary>
<br>

The `IssueTrackerLibrary.Models` namespace contains 7 named types.

- 7 named types.
- 103 total lines of source code.
- Approximately 28 lines of executable code.
- The highest cyclomatic complexity is 2 :heavy_check_mark:.

<details>
<summary>
  <strong id="basiccommentmodel">
    BasicCommentModel :heavy_check_mark:
  </strong>
</summary>
<br>

- The `BasicCommentModel` contains 4 members.
- 17 total lines of source code.
- Approximately 4 lines of executable code.
- The highest cyclomatic complexity is 2 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Method
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/BasicCommentModel.cs#L9 "BasicCommentModel.BasicCommentModel()")
| 100 | 1 :heavy_check_mark: | 0 | 0 | 4 / 0 |
| Method
| [14](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/BasicCommentModel.cs#L14 "BasicCommentModel.BasicCommentModel(CommentModel comment)")
| 83 | 1 :heavy_check_mark: | 0 | 1 | 5 / 2 |
| Property
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/BasicCommentModel.cs#L7 "string BasicCommentModel.Comment")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [6](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/BasicCommentModel.cs#L6 "string BasicCommentModel.Id")
| 100 | 2 :heavy_check_mark: | 0 | 2 | 2 / 2 |

<a href="#issuetrackerlibrary-models">:top: back to IssueTrackerLibrary.Models</a>

</details>

<details>
<summary>
  <strong id="basicissuemodel">
    BasicIssueModel :heavy_check_mark:
  </strong>
</summary>
<br>

- The `BasicIssueModel` contains 4 members.
- 17 total lines of source code.
- Approximately 4 lines of executable code.
- The highest cyclomatic complexity is 2 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Method
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/BasicIssueModel.cs#L9 "BasicIssueModel.BasicIssueModel()")
| 100 | 1 :heavy_check_mark: | 0 | 0 | 4 / 0 |
| Method
| [14](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/BasicIssueModel.cs#L14 "BasicIssueModel.BasicIssueModel(IssueModel issue)")
| 83 | 1 :heavy_check_mark: | 0 | 1 | 5 / 2 |
| Property
| [6](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/BasicIssueModel.cs#L6 "string BasicIssueModel.Id")
| 100 | 2 :heavy_check_mark: | 0 | 2 | 2 / 2 |
| Property
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/BasicIssueModel.cs#L7 "string BasicIssueModel.Issue")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |

<a href="#issuetrackerlibrary-models">:top: back to IssueTrackerLibrary.Models</a>

</details>

<details>
<summary>
  <strong id="basicusermodel">
    BasicUserModel :heavy_check_mark:
  </strong>
</summary>
<br>

- The `BasicUserModel` contains 4 members.
- 15 total lines of source code.
- Approximately 4 lines of executable code.
- The highest cyclomatic complexity is 2 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Method
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/BasicUserModel.cs#L9 "BasicUserModel.BasicUserModel()")
| 100 | 1 :heavy_check_mark: | 0 | 0 | 3 / 0 |
| Method
| [12](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/BasicUserModel.cs#L12 "BasicUserModel.BasicUserModel(UserModel user)")
| 83 | 1 :heavy_check_mark: | 0 | 1 | 5 / 2 |
| Property
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/BasicUserModel.cs#L7 "string BasicUserModel.DisplayName")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [6](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/BasicUserModel.cs#L6 "string BasicUserModel.Id")
| 100 | 2 :heavy_check_mark: | 0 | 2 | 2 / 2 |

<a href="#issuetrackerlibrary-models">:top: back to IssueTrackerLibrary.Models</a>

</details>

<details>
<summary>
  <strong id="commentmodel">
    CommentModel :heavy_check_mark:
  </strong>
</summary>
<br>

- The `CommentModel` contains 7 members.
- 12 total lines of source code.
- Approximately 5 lines of executable code.
- The highest cyclomatic complexity is 2 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Property
| [13](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/CommentModel.cs#L13 "bool CommentModel.Archived")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 1 |
| Property
| [10](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/CommentModel.cs#L10 "BasicUserModel CommentModel.Author")
| 100 | 2 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Property
| [8](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/CommentModel.cs#L8 "string CommentModel.Comment")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/CommentModel.cs#L9 "DateTime CommentModel.DateCreated")
| 100 | 2 :heavy_check_mark: | 0 | 2 | 1 / 1 |
| Property
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/CommentModel.cs#L7 "string CommentModel.Id")
| 100 | 2 :heavy_check_mark: | 0 | 3 | 3 / 2 |
| Property
| [12](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/CommentModel.cs#L12 "StatusModel CommentModel.IssueStatus")
| 100 | 2 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Property
| [11](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/CommentModel.cs#L11 "HashSet<string> CommentModel.UserVotes")
| 100 | 2 :heavy_check_mark: | 0 | 1 | 1 / 1 |

<a href="#issuetrackerlibrary-models">:top: back to IssueTrackerLibrary.Models</a>

</details>

<details>
<summary>
  <strong id="issuemodel">
    IssueModel :heavy_check_mark:
  </strong>
</summary>
<br>

- The `IssueModel` contains 8 members.
- 13 total lines of source code.
- Approximately 4 lines of executable code.
- The highest cyclomatic complexity is 2 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Property
| [14](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/IssueModel.cs#L14 "bool IssueModel.Archived")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 1 |
| Property
| [11](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/IssueModel.cs#L11 "BasicUserModel IssueModel.Author")
| 100 | 2 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Property
| [10](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/IssueModel.cs#L10 "DateTime IssueModel.DateCreated")
| 100 | 2 :heavy_check_mark: | 0 | 2 | 1 / 1 |
| Property
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/IssueModel.cs#L9 "string IssueModel.Description")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/IssueModel.cs#L7 "string IssueModel.Id")
| 100 | 2 :heavy_check_mark: | 0 | 3 | 3 / 2 |
| Property
| [8](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/IssueModel.cs#L8 "string IssueModel.Issue")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [12](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/IssueModel.cs#L12 "StatusModel IssueModel.IssueStatus")
| 100 | 2 :heavy_check_mark: | 0 | 1 | 1 / 0 |
| Property
| [13](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/IssueModel.cs#L13 "string IssueModel.OwnerNotes")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |

<a href="#issuetrackerlibrary-models">:top: back to IssueTrackerLibrary.Models</a>

</details>

<details>
<summary>
  <strong id="statusmodel">
    StatusModel :heavy_check_mark:
  </strong>
</summary>
<br>

- The `StatusModel` contains 3 members.
- 8 total lines of source code.
- Approximately 2 lines of executable code.
- The highest cyclomatic complexity is 2 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Property
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/StatusModel.cs#L7 "string StatusModel.Id")
| 100 | 2 :heavy_check_mark: | 0 | 3 | 3 / 2 |
| Property
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/StatusModel.cs#L9 "string StatusModel.StatusDescription")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [8](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/StatusModel.cs#L8 "string StatusModel.StatusName")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |

<a href="#issuetrackerlibrary-models">:top: back to IssueTrackerLibrary.Models</a>

</details>

<details>
<summary>
  <strong id="usermodel">
    UserModel :heavy_check_mark:
  </strong>
</summary>
<br>

- The `UserModel` contains 9 members.
- 14 total lines of source code.
- Approximately 5 lines of executable code.
- The highest cyclomatic complexity is 2 :heavy_check_mark:.

| Member kind | Line number | Maintainability index | Cyclomatic complexity | Depth of inheritance | Class coupling |
Lines of source / executable code |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| Property
| [15](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/UserModel.cs#L15 "List<BasicCommentModel> UserModel.AuthoredComments")
| 100 | 2 :heavy_check_mark: | 0 | 2 | 1 / 1 |
| Property
| [13](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/UserModel.cs#L13 "List<BasicIssueModel> UserModel.AuthoredIssues")
| 100 | 2 :heavy_check_mark: | 0 | 2 | 1 / 1 |
| Property
| [11](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/UserModel.cs#L11 "string UserModel.DisplayName")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [12](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/UserModel.cs#L12 "string UserModel.EmailAddress")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [9](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/UserModel.cs#L9 "string UserModel.FirstName")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [7](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/UserModel.cs#L7 "string UserModel.Id")
| 100 | 2 :heavy_check_mark: | 0 | 3 | 3 / 2 |
| Property
| [10](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/UserModel.cs#L10 "string UserModel.LastName")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [8](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/UserModel.cs#L8 "string UserModel.ObjectIdentifier")
| 100 | 2 :heavy_check_mark: | 0 | 0 | 1 / 0 |
| Property
| [14](https://github.com/mpaulosky/IssueTracker/blob/main/src/IssueTrackerLibrary/Models/UserModel.cs#L14 "List<BasicCommentModel> UserModel.VotedOnComments")
| 100 | 2 :heavy_check_mark: | 0 | 2 | 1 / 1 |

<a href="#issuetrackerlibrary-models">:top: back to IssueTrackerLibrary.Models</a>

</details>

</details>

<a href="#issuetrackerlibrary">:top: back to IssueTrackerLibrary</a>

<div id='issuetrackerlibraryunittests'></div>

## IssueTrackerLibraryUnitTests :question:

The *IssueTrackerLibraryUnitTests.csproj* project file contains:

- 0 namespaces.
- 0 named types.
- 0 total lines of source code.
- Approximately 0 lines of executable code.
- The highest cyclomatic complexity is 0 :question:.

<a href="#issuetrackerlibraryunittests">:top: back to IssueTrackerLibraryUnitTests</a>

## Metric definitions

- **Maintainability index**: Measures ease of code maintenance. Higher values are better.
- **Cyclomatic complexity**: Measures the number of branches. Lower values are better.
- **Depth of inheritance**: Measures length of object inheritance hierarchy. Lower values are better.
- **Class coupling**: Measures the number of classes that are referenced. Lower values are better.
- **Lines of source code**: Exact number of lines of source code. Lower values are better.
- **Lines of executable code**: Approximates the lines of executable code. Lower values are better.

*This file is maintained by a bot.*

<!-- markdownlint-restore -->