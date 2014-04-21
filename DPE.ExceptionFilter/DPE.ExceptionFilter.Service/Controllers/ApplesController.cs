using System.Web.Http;
using DPE.ExceptionFilter.Service.Repositories;

namespace DPE.ExceptionFilter.Service.Controllers
{
    public class ApplesController : ApiController
    {
        private readonly ApplesRepository _applesRepository;

        public ApplesController(ApplesRepository applesRepository)
        {
            _applesRepository = applesRepository;
        }

        public string Get()
        {
            return _applesRepository.ListAll();
        }

    }
}