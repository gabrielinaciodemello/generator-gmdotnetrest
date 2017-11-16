using System;
using Microsoft.AspNetCore.Mvc;
using <%= ProjectName %>.Repositories;
using <%= ProjectName %>.Utils;
using <%= ProjectName %>.Models;
using <%= ProjectName %>.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace <%= ProjectName %>.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class BaseController<T> : Controller where T : Entity
    {
        private readonly IRepository _repository;
        private readonly IRequestsHandler _requestsHandler;
        protected readonly AbstractValidator<T> _validator;

        public BaseController(IRepository repository, IRequestsHandler requestsHandler, AbstractValidator<T> validator)
        {
            _repository = repository;
            _requestsHandler = requestsHandler;
            _validator = validator;
        }

        // GET api/values
        [HttpGet]
        public virtual JsonResult Get() => _requestsHandler.JsonHandler(Request, Response, () => 
        {
            return new JsonReturn { Result = _repository.Get<T>() };
        });   

        // GET api/values/5
        [HttpGet("{id}")]
        public virtual JsonResult Get(string id) => _requestsHandler.JsonHandler(Request, Response, () => 
        {
            return new JsonReturn { Result = _repository.Get<T>(id) };
        });

        // POST api/values
        [HttpPost]
        public virtual JsonResult Post([FromBody]T value) => _requestsHandler.JsonHandler(Request, Response, () =>
        {
            var validationResult = _validator.Validate(value);
            if(!validationResult.IsValid){
                throw new InvalidOperationException(validationResult.Errors.ToText());
            }
            return new JsonReturn { Result = _repository.Create<T>(value) };
        });

        // PUT api/values/5
        [HttpPut("{id}")]
        public virtual JsonResult Put(string id, [FromBody]T value)  => _requestsHandler.JsonHandler(Request, Response, () =>
        {
            value.Id = id;
            var validationResult = _validator.Validate(value);
            if(!validationResult.IsValid){
                throw new InvalidOperationException(validationResult.Errors.ToText());
            }
            _repository.Update<T>(value);
            return new JsonReturn { Result = "OK" };
        });

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public virtual JsonResult Delete(string id)  => _requestsHandler.JsonHandler(Request, Response, () =>
        {
            _repository.Remove<T>(id);
            return new JsonReturn { Result = "OK" };
        });
    }
}