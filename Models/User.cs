using System;
using Pathway1.Models;
using SQLite;

namespace Pathway.Models
{
    /// <summary>
    /// Represents an application user
    /// </summary>
    [Table("Users")]
    public class User : BaseModel
    {
        // Basic user information
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }

        // Role and permissions
        public UserRole Role { get; set; }

        // Profile data
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public string PhoneNumber { get; set; }

        // Authentication (hashed password would be stored on server, not locally)
        public string AuthToken { get; set; }
        public DateTime? AuthTokenExpiry { get; set; }

        // User preferences
        public string PreferencesJson { get; set; }

        // Avatar/profile image
        public string AvatarPath { get; set; }

        // Last activity tracking
        public DateTime LastLoginDate { get; set; }
        public DateTime LastActiveDate { get; set; }
    }

    /// <summary>
    /// Enumeration of user roles
    /// </summary>
    public enum UserRole
    {
        Viewer,
        FieldWorker,
        Supervisor,
        Administrator
    }
}