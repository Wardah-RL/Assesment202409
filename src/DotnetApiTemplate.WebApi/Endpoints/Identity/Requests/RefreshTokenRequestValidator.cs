﻿using FluentValidation;

namespace DotnetApiTemplate.WebApi.Endpoints.Identity.Requests;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(e => e.RefreshToken).NotNull().NotEmpty();
    }
}