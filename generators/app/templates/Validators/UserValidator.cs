using FluentValidation;
using <%= ProjectName %>.Models;
using <%= ProjectName %>.Repositories;
using System.Linq;

namespace <%= ProjectName %>.Validators
{
    public class UserValidator: AbstractValidator<User>
    {
        private readonly IRepository _repository;
        
        public UserValidator(IRepository repository) 
        {
            _repository = repository;
            RuleFor(e => e.Name).NotEmpty().WithMessage("The name is required").Length(0,10).WithMessage("Name must be 10 characters or less");
            RuleFor(e => e.Email).NotEmpty().WithMessage("The email is required").EmailAddress().WithMessage("Invalid email");
            
            RuleFor(e => e).Must(Unique).WithMessage("The Email already exist");
        }
        
        private bool Unique(User entity)
        {            
            var result = _repository.Get<User>(x => x.Email == entity.Email).FirstOrDefault();
            if(result == null) return true;
            if(result.Id == entity.Id && result.Email == entity.Email) return true;
            return false;
        }
    }
}