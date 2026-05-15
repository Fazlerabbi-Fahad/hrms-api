namespace HRMS.Application.Constants
{
    public class AppConstants
    {
        public static class Roles
        {  
            public const string Admin = "Admin";
            public const string HRAdmin = "HRAdmin";
            public const string User = "User";
        }

        public static class Messages
        {
            public const string NotFound = "Data not found!";
            public const string Success = "Data fetched successfully!";
            public const string Unathorized = "You are not authorized!";
            public const string ServerError = "An unexpected error occurred";
        }
        
    }
}
