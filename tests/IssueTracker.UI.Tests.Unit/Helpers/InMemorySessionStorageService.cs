// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     InMemorySessionStorageService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI.Tests.Unit
// =============================================

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using Blazored.SessionStorage;

namespace IssueTracker.UI.Helpers;

/// <summary>
/// In-memory implementation of <see cref="ISessionStorageService"/> for bUnit 2.x tests.
/// Replaces Blazored.SessionStorage.TestExtensions which targeted bUnit 1.x TestContextBase.
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed class InMemorySessionStorageService : ISessionStorageService
{
	private readonly Dictionary<string, JsonElement> _store = [];

	public event EventHandler<ChangingEventArgs>? Changing;
	public event EventHandler<ChangedEventArgs>? Changed;

	public ValueTask ClearAsync(CancellationToken cancellationToken = default)
	{
		_store.Clear();
		return ValueTask.CompletedTask;
	}

	public ValueTask<T> GetItemAsync<T>(string key, CancellationToken cancellationToken = default)
	{
		if (_store.TryGetValue(key, out JsonElement element))
		{
			T? value = JsonSerializer.Deserialize<T>(element.GetRawText());
			return ValueTask.FromResult(value!);
		}

		return ValueTask.FromResult(default(T)!);
	}

	public ValueTask<string> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default)
	{
		if (_store.TryGetValue(key, out JsonElement element))
		{
			return ValueTask.FromResult(element.GetString() ?? string.Empty);
		}

		return ValueTask.FromResult(string.Empty);
	}

	public ValueTask<string> KeyAsync(int index, CancellationToken cancellationToken = default)
	{
		string[] keys = [.. _store.Keys];
		return index < keys.Length
			? ValueTask.FromResult(keys[index])
			: ValueTask.FromResult(string.Empty);
	}

	public ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default) =>
		ValueTask.FromResult<IEnumerable<string>>(_store.Keys.ToList());

	public ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default) =>
		ValueTask.FromResult(_store.ContainsKey(key));

	public ValueTask<int> LengthAsync(CancellationToken cancellationToken = default) =>
		ValueTask.FromResult(_store.Count);

	public ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
	{
		_store.Remove(key);
		return ValueTask.CompletedTask;
	}

	public ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
	{
		foreach (string key in keys)
		{
			_store.Remove(key);
		}

		return ValueTask.CompletedTask;
	}

	public ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default)
	{
		JsonElement element = JsonSerializer.SerializeToElement(data);
		_store[key] = element;
		return ValueTask.CompletedTask;
	}

	public ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default)
	{
		JsonElement element = JsonSerializer.SerializeToElement(data);
		_store[key] = element;
		return ValueTask.CompletedTask;
	}
}
