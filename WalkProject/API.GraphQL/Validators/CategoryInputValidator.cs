using FluentValidation;
using WalkProject.API.GraphQL.DTOs.Categories;

namespace WalkProject.API.GraphQL.Validators
{
    public class CategoryInputValidator : AbstractValidator<CategoryInput>
    {
        public CategoryInputValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50)
                .WithMessage("Category name must be between 3 and 50 characters.")
                .WithErrorCode("CATEGORY_NAME_LENGTH");
        }
    }
}
