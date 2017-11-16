using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using <%= ProjectName %>.Models;
using <%= ProjectName %>.Repositories;
using <%= ProjectName %>.Utils;
using <%= ProjectName %>.Validators;
using System;
using System.Linq;

namespace <%= ProjectName %>.Controllers
{
    public class UsersController : BaseController<User>
    {
        private readonly IRepository _repository;
        private readonly IRequestsHandler _requestsHandler;
        private readonly IBearerAuth _bearerAuth;

        public UsersController(IRepository repository, IRequestsHandler requestsHandler, IBearerAuth bearerAuth): base(repository, requestsHandler, new UserValidator(repository))
        {
            _repository = repository;
            _requestsHandler = requestsHandler;
            _bearerAuth = bearerAuth;
        }

        [AllowAnonymous]
        public override JsonResult Post([FromBody]User value)
        {
            value.Password = value.Password.ToMd5();
            return base.Post(value);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("~/api/token")]
        public JsonResult Login([FromBody]User value) => _requestsHandler.JsonHandler(Request, Response, () =>
        {
            var user = _repository.Get<User>(x => x.Email == value.Email).FirstOrDefault();
            if(user != null && user.Password == value.Password.ToMd5())
            {
                return new JsonReturn { Result = _bearerAuth.GenerateToken(user.Id, user.Name) };
            }
            throw new InvalidOperationException("Login or Password incorrect");
        });
    }
}