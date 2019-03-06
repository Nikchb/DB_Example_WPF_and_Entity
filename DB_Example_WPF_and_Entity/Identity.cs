using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Example_WPF_and_Entity
{
    class Identity
    {
        private static readonly AppContext context = new AppContext();

        public static int UserId { get; private set; }

        public static void LogInUser(int UserId)
        {
            Identity.UserId = UserId;
        }

        public static bool CheckRole(params string[] RoleName)
        {
            var user = context.Users.Include("Roles").FirstOrDefault(v => v.UserId == UserId);
            return RoleName.Any(x=>user.Roles.Select(v => v.RoleName).Contains(x));
        }
    }
}
