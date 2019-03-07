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

namespace DB_Example_WPF_and_Entity
{
    /// <summary>
    /// Логика взаимодействия для CompanyWindow.xaml
    /// </summary>
    public partial class CompanyWindow : Window
    {
        private Company company;        

        public CompanyWindow(Company company,bool IsNew)
        {
            InitializeComponent();
            this.company = company;            
            if (!IsNew)
            {
                CompanyName.Text = company.CompanyName;
                CompanyPublicName.Text = company.CompanyPublicName;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Text = "";
            if (!string.IsNullOrEmpty(CompanyName.Text) && !string.IsNullOrEmpty(CompanyPublicName.Text))
            {
                company.CompanyName = CompanyName.Text;
                company.CompanyPublicName = CompanyPublicName.Text;
                this.Close();
            }
            else
            {
                ErrorMessage.Text = "Нужно обязательно заполнить все поля!!!";
            }
        }
    }
    
}
