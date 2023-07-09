namespace Domain.Exeptions
{
    public class PermissionDeniedException : Exception
    {
        public PermissionDeniedException(int userId, string requiredRole)
            : base($"User with ID {userId} does not have the required role '{requiredRole}' to perform the operation.")
        {

        }
    }
}
