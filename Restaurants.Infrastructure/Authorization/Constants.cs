namespace Restaurants.Infrastructure.Authorization;

public static class Policies
{
    public const string HasNationality = "HasNationality";
}

public static class AppClaimTypes
{
    public const string Nationality = "Nationality";
    public const string DateOfBirth = "DateOfBirth";
}

