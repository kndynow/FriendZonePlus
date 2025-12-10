using FluentValidation;
using FriendZonePlus.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendZonePlus.Application.Validators
{
    public class SendMessageRequestDtoValidator : AbstractValidator<SendMessageRequestDto>
    {
        public SendMessageRequestDtoValidator()
        {
            RuleFor(x => x.ReceiverId).NotEmpty().WithMessage("Receiver Id must be greater than 0");

            RuleFor(x => x.Content).NotEmpty().WithMessage("Message cannot be empty")
           .MaximumLength(300).WithMessage("Message cannot be longer than 300 characters");
        }
    }
}
