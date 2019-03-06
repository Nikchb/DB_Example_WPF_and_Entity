using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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

namespace DB_Example_WPF_and_Entity
{
    /// <summary>
    /// Логика взаимодействия для LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        private AppContext context;

        public LogInWindow()
        {
            InitializeComponent();
            context = new AppContext();
        }      

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = context.Users.FirstOrDefault(v => v.UserName == UserName.Text);
            if (user != null)
            {
                if (SHA512(Password.Password) == user.Password)
                {
                    Identity.LogInUser(user.UserId);
                    MainWindow window = new MainWindow(context);
                    window.Show();
                    this.Close();
                }
                else
                {
                   Password.Password = "";                
                   ErrorMessage.Text = "Не верный логин или пароль!";
                }
            }
            else
            {
                try
                {

                    context.Users.Add(new User { UserName = UserName.Text, Password = SHA512(Password.Password), FullName = "NewUser" });
                    context.SaveChanges();
                    MainWindow window = new MainWindow(context);
                    window.Show();
                    this.Close();
                }               
                catch
                {
                    ErrorMessage.Text = "Недопустимые логин или пароль!";
                }
            }
        }

        private static string SHA512(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }
    }
}
