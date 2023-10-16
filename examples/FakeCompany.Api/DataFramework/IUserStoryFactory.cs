namespace DataCompany.Framework
{
    public interface IUserStoryFactory<T> where T : Entity
    {
        IQueryable<T> AsQueryable();

        bool Delete(T model);

        T Find(int id);

        T Upsert(T model, Func<T, T> update);
    }
}