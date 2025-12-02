using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using FriendZonePlus.Core.Interfaces;

namespace FriendZonePlus.Application.Validators
{
    public class RegisterUserRequestDtoValidator : AbstractValidator<RegisterUserRequestDto>
    {
        public RegisterUserRequestDtoValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters")
                .MaximumLength(20).WithMessage("Username cannot exceed 20 characters")
                .MustAsync(async (username, ct) => !await userRepository.ExistsByUsernameAsync(username))
                .WithMessage("Username already taken");

            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage("Email is required")
                 .EmailAddress().WithMessage("Invalid email")
                 .MinimumLength(5).WithMessage("Email must be longer than 5 characters")
                 .MaximumLength(50).WithMessage("Email cannot exceed 50 characters")
                 .MustAsync(async (email, ct) => !await userRepository.ExistsByEmailAsync(email))
                 .WithMessage("Email already taken");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be longer than 6 characters")
                .MaximumLength(30).WithMessage("Password cannot be longer than 30 characters")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(30).WithMessage("First name cannot exceed 30 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(30).WithMessage("Last name cannot exceed 30 characters");
        }
    }
}
