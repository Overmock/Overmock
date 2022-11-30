using Microsoft.AspNetCore.Mvc;
using Overmock.Examples.Storage;

namespace Overmock.Examples.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserStoryController : Controller
    {
        private readonly IUserStoryService _service;

        public UserStoryController(IUserStoryService service) => _service = service;

        [HttpGet]
        public Response<UserStory> Get()
        {
            try
            {
                return _service.GetAll().ToList();
            }
            catch (Exception e)
            {
                return e;
            }
        }

        [HttpGet("/{id}")]
        public Response<UserStory> Get(int id)
        {
            try
            {
                return _service.Get(id);
            }
            catch (Exception e)
            {
                return e;
            }
        }

        [HttpPost]
        public Response<UserStory> Post(UserStory model)
        {
            try
            {
                return _service.Save(model);
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}