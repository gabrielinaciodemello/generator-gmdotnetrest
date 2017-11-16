using FluentValidation;
using <%= ProjectName %>.Models;
using <%= ProjectName %>.Repositories;
using System.Linq;

namespace <%= ProjectName %>.Validators
{
    public class <%= ModelName %>Validator: AbstractValidator<<%= ModelName %>>
    {
        private readonly IRepository _repository;
        
        public <%= ModelName %>Validator(IRepository repository) 
        {
            _repository = repository;
            <%- Rules %>
            <% if(Unique){%>
            RuleFor(e => e).Must(Unique).WithMessage("<%- Unique.Message %>");
            <%}%>        
        }
        
        <% if(Unique){%>
        private bool Unique(<%= ModelName %> entity)
        {
            var result = _repository.Get<<%= ModelName %>>(x => <%- Unique.Where %>).FirstOrDefault();
            if(result == null) return true;
            if(result.Id == entity.Id && <%- Unique.Conditional %>) return true;
            return false;            
        }
        <%}%>
        
    }
}