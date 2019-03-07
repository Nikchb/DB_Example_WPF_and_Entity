using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Entity;

namespace DB_Example_WPF_and_Entity
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private readonly AppContext context;
        private User user;
        private List<User> users;
        private List<Role> roles;
        private List<Role> userRoles;

        public AdminWindow(AppContext context)
        {         
                InitializeComponent();
                this.context = context;
                users = context.Users.ToList();
                roles = context.Roles.ToList();
                UsersBox.ItemsSource = users.Select(v => v.UserName + " - " + v.FullName);
                RolesBox.ItemsSource = roles.Select(v => v.RoleName);
                UsersBox.SelectedIndex = 0;                  
        }

        public new void Show()
        {
            if (Identity.CheckRole("Admin"))
            {
                base.Show();
            }
            else
            {
                this.Close();
                MessageBox.Show("Недостаточно прав для данного действия!");
            }
        }



        private void UsersBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ErrorText.Text = "";
            int index = UsersBox.SelectedIndex;
            if (index == -1)
            {
                index = 0;
            }
            int UserId = users[index].UserId;
            user = context.Users.Include("Roles").FirstOrDefault(v => v.UserId == UserId);
            UserName.Text = user.UserName;
            FullName.Text = user.FullName;
            userRoles = user.Roles.ToList();
            UserRolesBox.ItemsSource = userRoles.Select(v => v.RoleName);
        }

        private void SaveUser_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";
            if (!string.IsNullOrWhiteSpace(UserName.Text) && !string.IsNullOrWhiteSpace(FullName.Text))
            {
                try
                {
                    int index = UsersBox.SelectedIndex;
                    var user = context.Users.Find(users[index].UserId);
                    user.UserName = UserName.Text;
                    user.FullName = FullName.Text;
                    context.SaveChanges();
                    users[index] = user;
                }
                catch
                {
                    ErrorText.Text = "Такой логин уже занят!";
                }
            }
            else
            {
                ErrorText.Text = "Необходимо заполняить оба поля!";
            }
        }

        private void AddNewRole_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";
            var roleName = NewRole.Text;
            if (!string.IsNullOrWhiteSpace(roleName) && context.Roles.FirstOrDefault(v=>v.RoleName==roleName) == null)
            {
                var newRole = new Role { RoleName = roleName };
                context.Roles.Add(newRole);
                context.SaveChangesAsync();
                roles.Add(newRole);
                RolesBox.ItemsSource = roles.Select(v => v.RoleName);
            }
            else
            {
                ErrorText.Text = "Недопустимое имя роли!";
            }
        }

        private void DeleteUserRole_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";
            int index = UserRolesBox.SelectedIndex;
            if (index != -1)
            {

                int RoleId = userRoles[index].RoleId;
                var role = context.Roles.Find(RoleId);
                if (user.UserId != Identity.UserId || role.RoleName != "Admin")
                {
                    user.Roles.Remove(role);
                    context.SaveChanges();
                    userRoles.Remove(role);
                    UserRolesBox.ItemsSource = userRoles.Select(v => v.RoleName);
                }
                else
                {
                    ErrorText.Text = "Невозможно лишить себя права на администрирование!";
                }
            }
            else
            {
                ErrorText.Text = "Необходимо сначала выбрать роль в центральном столбце!";
            }
        }

        private void AddUserRole_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";
            int index = RolesBox.SelectedIndex;
            if (index != -1)
            {
                var role = context.Roles.Find(roles[index].RoleId);
                if (!user.Roles.Contains(role))
                {
                    user.Roles.Add(role);
                    context.SaveChanges();
                    userRoles.Add(role);
                    UserRolesBox.ItemsSource = userRoles.Select(v => v.RoleName);
                }
                else
                {
                    ErrorText.Text = "Пользователь уже обладает этой ролью!";
                }
            }
            else
            {
                ErrorText.Text = "Необходимо сначала выбрать роль в правом столбце!";
            }
        }

        private void DeleteRole_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";
            int index = RolesBox.SelectedIndex;
            if (index != -1)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Вы точно хотите удалить роль?", "Подтверждение удаления", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    var role = context.Roles.Find(roles[index].RoleId);
                    if (role.RoleName != "Admin")
                    {
                        context.Roles.Remove(role);
                        context.SaveChanges();
                        roles.RemoveAt(index);
                        RolesBox.ItemsSource = roles.Select(v => v.RoleName);
                        UsersBox_SelectionChanged(null, null);
                    }
                    else
                    {
                        ErrorText.Text = "Невозможно удалить роль администратора!";
                    }
                }
            }
            else
            {
                ErrorText.Text = "Необходимо сначала выбрать роль в правом столбце!";
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Вы точно хотите удалить пользователя?", "Подтверждение удаления", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (user.UserId != Identity.UserId)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                    users.RemoveAt(UsersBox.SelectedIndex);
                    UsersBox.ItemsSource = users.Select(v => v.UserName + " - " + v.FullName);
                    UsersBox.SelectedIndex = 0;
                }
                else
                {
                    ErrorText.Text = "Невозможно удалять своего пользователя!";
                }
            }
        }
    }
}
