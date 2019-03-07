namespace DB_Example_WPF_and_Entity.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DB_Example_WPF_and_Entity.AppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DB_Example_WPF_and_Entity.AppContext context)
        {
            Role Admin = new Role
            {
                RoleName = "Admin"
            };
            Role Delete = new Role
            {
                RoleName = "Delete"
            };
            Role Edit = new Role
            {
                RoleName = "Edit"
            };
            Role Add = new Role
            {
                RoleName = "Add"
            };
            Role Read = new Role
            {
                RoleName = "Read"
            };
            context.Roles.Add(Admin);
            context.Roles.Add(Delete);
            context.Roles.Add(Edit);
            context.Roles.Add(Add);
            context.Roles.Add(Read);
            context.SaveChanges();
            User user = new User
            {
                UserName = "Admin",
                Password = LogInWindow.SHA512("123456"), 
                FullName = "Admin"
            };
            user.Roles.Add(Admin);
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
