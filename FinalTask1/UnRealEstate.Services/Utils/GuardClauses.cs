using System;
using UnrealEstate.Models;

namespace UnrealEstate.Services
{
    public static class GuardClauses
    {
        /// <summary>
        /// Throws InvalidOperationException if not valid status.
        /// </summary>
        /// <param name="currentStatus">current status id.</param>
        /// <param name="validStatusId">valid status id.</param>
        public static void IsValidStatus(int currentStatus, int validStatusId)
        {
            if (currentStatus != validStatusId)
            {
                throw new InvalidOperationException("Listing status is not valid to perform this action.");
            }
        }

        /// <summary>
        /// Throws InvalidOperationException if current listing status is not valid for comment.
        /// </summary>
        /// <param name="currentListingStatus"></param>
        public static void IsAllowCommentStatus(int currentListingStatus)
        {
            if (currentListingStatus != (int) Status.Active && currentListingStatus != (int) Status.Canceled)
            {
                throw new InvalidOperationException("Listing status is not valid to perform this action.");
            }
        }

        /// <summary>
        /// Throws NotSupportedException if user is not admin or author.
        /// </summary>
        /// <param name="currentUserId">current user id.</param>
        /// <param name="authorId">author id.</param>
        /// <param name="currentUserRole">current user role.</param>
        public static void IsAuthorOrAdmin(string currentUserId, string authorId, string currentUserRole)
        {
            if (!currentUserRole.Equals("Admin") && !currentUserId.Equals(authorId))
            {
                throw new NotSupportedException("User not has privilege to perform this action.");
            }
        }

        public static void IsNotNull(object source, string paramName)
        {
            if (source is null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Throws NotSupportedException if user is not admin.
        /// </summary>
        /// <param name="role">user role.</param>
        public static void IsAdmin(string role)
        {
            if (!role.Equals("Admin"))
            {
                throw new NotSupportedException("User not has privilege to perform this action.");
            }
        }

        /// <summary>
        /// Throws NotSupportedException if user is not author.
        /// </summary>
        /// <param name="currentUserId">current user id.</param>
        /// <param name="authorId">author id.</param>
        public static void IsAuthor(string currentUserId, string authorId)
        {
            if (!currentUserId.Equals(authorId))
            {
                throw new NotSupportedException("User not has privilege to perform this action.");
            }
        }

        /// <summary>
        /// Throws ArgumentOutOfRangeException if value is null.
        /// </summary>
        /// <param name="value">value to check</param>
        /// <param name="paramName">parameter causes null value.</param>
        public static void HasValue(object value, string paramName)
        {
            if (value is null)
            {
                throw new ArgumentOutOfRangeException(paramName, "Value not valid");
            }
        }
    }
}
