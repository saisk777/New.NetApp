using System;
using System.Security.Claims;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace API.Entities;

public static class ClaimsPrincipleExtensions
{
    public static string GetUsername(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.NameIdentifier)

            ?? throw new Exception("Cannot get Username from token");

        return username;
    }

}
