using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuizAppApi.Controllers
{
    [Route("[controller]")]
    [ApiController]



    public class AuthorsController : ControllerBase
    {

        private static readonly String[] Authors = new[]
        {
        "Aldas Vertelis", "Kanstantinas Piatrashka", "Motiejus Šveikauskas", "Danielius Podbielski"
        };

        [HttpGet(Name = "GetAuthors")]
       public IEnumerable<String> Get()
        {
            return Authors;
        }
    }
}
