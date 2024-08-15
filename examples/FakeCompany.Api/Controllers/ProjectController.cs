using FakeCompany.Api.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FakeCompany.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public EnumerableResponse<Project> Get()
        {
            var projects = _projectService.GetAll();
            return projects;
        }

        [HttpGet("{id}")]
        public Response<Project> Get(int id)
        {
            var project = _projectService.Get(id);
            if (project == null)
            {
                return new Exception("Project not found");
            }
            return project;
        }

        [HttpPost]
        public Response<Project> Post([FromBody] Project project)
        {
            var savedProject = _projectService.Save(project);
            return savedProject;
        }

        [HttpDelete]
        public Response<bool> Delete([FromBody] Project project)
        {
            var result = _projectService.Delete(project);
            return result;
        }
    }
}
