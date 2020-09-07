using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnrealEstate.Models;

namespace UnrealEstate.Services
{
    public static class GuardClauses
    {
        public static void IsValidStatus(int currentStatus, int validStatusId)
        {
            if (currentStatus != validStatusId)
            {
                throw new InvalidOperationException("Listing status is not valid to perform this action.");
            }
        }

        public static void IsNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
                throw new ArgumentNullException(argumentName);
        }

        public static void IsAuthorOrAdmin(string currentUserId, string authorId, string currentUserRole)
        {
            if (!currentUserRole.Equals("Admin") && !currentUserId.Equals(authorId))
            {
                throw new NotSupportedException("User not has privilege to perform this action.");
            }
        }

        public static void IsAdmin(string role)
        {
            if (!role.Equals("Admin"))
            {
                throw new NotSupportedException("User not has privilege to perform this action.");
            }
        }

        public static void IsAuthor(string currentUserId, string authorId)
        {
            if (!currentUserId.Equals(authorId))
            {
                throw new NotSupportedException("User not has privilege to perform this action.");
            }
        }

        public static void HasValue(object value, string paramName)
        {
            if (value is null)
            {
                throw new ArgumentOutOfRangeException(paramName, "Value not valid");
            }
        }
    }
}
