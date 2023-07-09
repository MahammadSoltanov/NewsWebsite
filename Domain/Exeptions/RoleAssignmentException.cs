namespace Domain.Exeptions
{
    public class RoleAssignmentException : Exception
    {
        public RoleAssignmentException(string roleToAssign, int userId)
            : base($"Failed to assign the role '{roleToAssign}' to User with ID {userId}")
        { }
    }
}
