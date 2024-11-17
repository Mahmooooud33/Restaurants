﻿using MediatR;

namespace Restaurants.Application.Users.Commands;

public record UpdateUserDetailsCommand : IRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
}
