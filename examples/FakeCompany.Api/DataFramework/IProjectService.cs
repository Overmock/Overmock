namespace FakeCompany.Api.Storage
{
    public interface IProjectService
    {
        List<Project> GetAll();
        Project Get(int id);
        Project Save(Project project);
        bool Delete(Project project);
    }
}
