namespace IssueTrackerLibrary.Contracts;

public interface IBaseRepository<TEntity> where TEntity : class
{
	public Task<TEntity> Get(string id);
	
	public Task<IEnumerable<TEntity>> Get();
	
	public Task Create(TEntity obj);

	public Task Update(string id, TEntity obj);
	
	// void Delete(string id);
}