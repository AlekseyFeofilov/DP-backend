using System.Security.Claims;
using DP_backend.Common.Exceptions;
using DP_backend.Models;

namespace DP_backend.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal) 
        => Guid.TryParse(claimsPrincipal.FindFirstValue(DPClaimtTypes.Id), out var userId) ? userId : throw new NoPermissionException();
}