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

namespace DB_Example_WPF_and_Entity.Windows
{
    /// <summary>
    /// Логика взаимодействия для DeviceWindow.xaml
    /// </summary>
    public partial class DeviceWindow : Window
    {
        private Device device;

        private Company[] companies;

        public DeviceWindow(Device device, Company[] companies, bool IsNew)
        {
            InitializeComponent();
            this.device = device;
            this.companies = companies;
            foreach(var comp in companies)
            {
                Company.Items.Add(comp.CompanyName);
            }
            if (!IsNew)
            {
                Name.Text = device.Name;
                Price.Text = device.Price.ToString();
                Company.SelectedIndex = Array.IndexOf(companies.Select(v => v.CompanyId).ToArray(), device.CompanyId);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Text = "";
            if (!string.IsNullOrEmpty(Name.Text) && !string.IsNullOrEmpty(Price.Text))
            {
                try
                {
                    device.Name = Name.Text;
                    device.Price = decimal.Parse(Price.Text);
                    if (Company.SelectedIndex >= 0)
                    {
                        device.CompanyId = companies.First(v => (string)Company.SelectedItem == v.CompanyName).CompanyId;
                    }
                    this.Close();
                }
                catch
                {
                    ErrorMessage.Text = "Поля заполнены не верно!!!";
                }
            }
            else
            {
                ErrorMessage.Text = "Нужно обязательно заполнить все поля!!!";
            }
        }
    }
}
