namespace IssueTrackerLibrary.Contracts;

public interface IBaseRepository<TEntity> where TEntity : class
{
	Task<TEntity> Get(string id);
	Task<IEnumerable<TEntity>> Get();
	Task Create(TEntity obj);
	Task Update(string id, TEntity obj);
	// void Delete(string id);
}