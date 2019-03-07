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
using System.ComponentModel;
using DB_Example_WPF_and_Entity.Windows;

namespace DB_Example_WPF_and_Entity
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppContext context;

        private string TableName;      

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


        private void TableCompany(object sender, RoutedEventArgs e)
        {
            context.Companies.Load();             
            Table.ItemsSource = context.Companies.Local.ToBindingList();
            Table.Columns[0].Visibility = Visibility.Hidden;
            TableName = "Company";
        }

        private void TableDevice(object sender, RoutedEventArgs e)
        {
            context.Devices.Include("Company").Load();            
            Table.ItemsSource = context.Devices.Local.ToBindingList();
            Table.Columns[0].Visibility = Visibility.Hidden;
            Table.Columns[3].Visibility = Visibility.Hidden;
            Table.Columns[4].Visibility = Visibility.Hidden;
            TableName = "Device";
        }

        private void Admin(object sender, RoutedEventArgs e)
        {
            AdminWindow window = new AdminWindow(context);
            window.Show();           
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if (Identity.CheckRole("Add"))
            {
                switch (TableName)
                {
                    case "Company":
                        {
                            try
                            {
                                var company = new Company();
                                new CompanyWindow(company, true).ShowDialog();
                                context.Companies.Add(company);
                                context.SaveChanges();
                            }
                            catch { }
                        }
                        break;
                    case "Device":
                        {
                            try
                            {
                                var device = new Device();
                                new DeviceWindow(device, context.Companies.ToArray(), true).ShowDialog();
                                context.Devices.Add(device);
                                context.SaveChanges();
                            }
                            catch { }
                        }
                        break;
                }
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (Identity.CheckRole("Edit"))
            {
                switch (TableName)
                {
                    case "Company":
                        {
                            try
                            {
                                var company = (Company)Table.SelectedItem;
                                new CompanyWindow(company, false).ShowDialog();
                                context.SaveChanges();
                            }
                            catch { }
                        }
                        break;
                    case "Device":
                        {
                            try
                            {
                                var device = (Device)Table.SelectedItem;
                                new DeviceWindow(device, context.Companies.ToArray(), false).ShowDialog();
                                context.SaveChanges();
                            }
                            catch { }
                        }
                        break;
                }
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (Identity.CheckRole("Delete"))
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Вы точно хотите удалить?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    switch (TableName)
                    {
                        case "Company":
                            {
                                try
                                {
                                    var company = (Company)Table.SelectedItem;
                                    context.Companies.Remove(company);
                                    context.SaveChanges();
                                }
                                catch { }
                            }
                            break;
                        case "Device":
                            {
                                try
                                {
                                    var device = (Device)Table.SelectedItem;
                                    context.Devices.Remove(device);
                                    context.SaveChanges();
                                }
                                catch { }
                            }
                            break;
                    }
                }
            }
        }
    }
}
