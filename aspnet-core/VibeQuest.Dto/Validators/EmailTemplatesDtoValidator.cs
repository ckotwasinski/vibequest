using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto.Validators
{
    public class EmailTemplatesDtoValidator : AbstractValidator<EmailTemplatesDto>
    {
        public EmailTemplatesDtoValidator()
        {
            RuleFor(c => c.TemplateCode).NotEmpty().MaximumLength(50);
            RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Subject).NotEmpty().MaximumLength(500);
            RuleFor(c => c.Body).NotEmpty();
        }
    }
}
