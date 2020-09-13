using System.Collections.Generic;
using System.Linq;
using EnpalSharpTemplate.Model;

namespace EnpalSharpTemplate.Helper
{
    public static class ExtensionMethods
    {
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users)
        {
            return users.Select(x => WithoutPassword(x));
        }

        public static User WithoutPassword(this User user)
        {
            user.Password = null;
            return user;
        }
    }
}