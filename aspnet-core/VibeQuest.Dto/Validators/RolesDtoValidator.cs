using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto.Validators
{
    public class RolesDtoValidator : AbstractValidator<RolesDto>
    {
        public RolesDtoValidator()
        {
            RuleFor(c => c.Code).NotEmpty().MaximumLength(50);
            RuleFor(c => c.Name).NotEmpty().MaximumLength(50);         
        }
    }
}
