﻿namespace IssueTracker.Library;

[CollectionDefinition("Test Collection")]
public partial class DatabaseCollection : ICollectionFixture<IssueTrackerTestFactory>
{
	
	// This class has no code, and is never created. Its purpose is simply
	// to be the place to apply [CollectionDefinition] and all the
	// ICollectionFixture<> interfaces.
	
}