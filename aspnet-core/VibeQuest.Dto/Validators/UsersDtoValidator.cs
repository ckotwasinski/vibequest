using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto.Validators
{
    public class UsersDtoValidator : AbstractValidator<UsersDto>
    {
        public UsersDtoValidator()
        {
            RuleFor(c => c.Email).NotEmpty().MaximumLength(150);
            RuleFor(c => c.Password).MaximumLength(30);
        }
    }
}
