using System;
using FluentValidation;
using FriendZonePlus.Application.DTOs;

namespace FriendZonePlus.Application.Validators;

public class CreateWallPostValidator : AbstractValidator<CreateWallPostDto>
{
    public CreateWallPostValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(300);
        RuleFor(x => x.TargetUserId).GreaterThan(0);
    }

}

public class UpdateWallPostValidator : AbstractValidator<UpdateWallPostDto>
{
    public UpdateWallPostValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(300);
    }
}
