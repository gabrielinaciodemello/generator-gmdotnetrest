using <%= ProjectName %>.Models;
using <%= ProjectName %>.Repositories;
using <%= ProjectName %>.Utils;
using <%= ProjectName %>.Validators;

namespace <%= ProjectName %>.Controllers
{
    public class <%= ModelName %>sController : BaseController<<%= ModelName %>>
    {
        public <%= ModelName %>sController(IRepository repository, IRequestsHandler requestsHandler): base(repository, requestsHandler, new <%= ModelName %>Validator(repository))
        {
        }
    }
}