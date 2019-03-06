using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DB_Example_WPF_and_Entity
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppContext context;        

        public MainWindow(AppContext context)
        {
            InitializeComponent();
            this.context = context;                  
        }

        public new void Show()
        {
            if (Identity.CheckRole("CanRead","Admin"))
            {
                base.Show();
            }
            else
            {
                this.Close();
                MessageBox.Show("Недостаточно прав для данного действия!");
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Table.ItemsSource = context.Companies.Local.ToBindingList();            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Table.ItemsSource = context.Devices.Local.ToBindingList();
            
        }

        private void Button_Click_Admin(object sender, RoutedEventArgs e)
        {
            AdminWindow window = new AdminWindow(context);
            window.Show();           
        }
    }
}
