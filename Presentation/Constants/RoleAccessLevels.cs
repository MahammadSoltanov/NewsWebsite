using Domain.Constants;

namespace Presentation.Constants;

public static class RoleAccessLevels
{
    public const string AdminOnly = UserRole.Admin;

    public const string AdminAndModerator = UserRole.Admin + "," + UserRole.Moderator;

    public const string AllRoles = UserRole.Admin + "," + UserRole.Moderator + "," + UserRole.Journalist;
}
