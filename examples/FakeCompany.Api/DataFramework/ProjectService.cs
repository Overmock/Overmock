using System.Collections.Generic;
using System.Linq;

namespace FakeCompany.Api.Storage
{
    public class ProjectService : IProjectService
    {
        private readonly List<Project> _projects = new List<Project>();

        public List<Project> GetAll()
        {
            return _projects;
        }

        public Project Get(int id)
        {
            return _projects.FirstOrDefault(p => p.Id == id);
        }

        public Project Save(Project project)
        {
            var existingProject = _projects.FirstOrDefault(p => p.Id == project.Id);
            if (existingProject != null)
            {
                existingProject.Name = project.Name;
                existingProject.Description = project.Description;
            }
            else
            {
                project.Id = _projects.Count > 0 ? _projects.Max(p => p.Id) + 1 : 1;
                _projects.Add(project);
            }
            return project;
        }

        public bool Delete(Project project)
        {
            return _projects.Remove(project);
        }
    }
}
