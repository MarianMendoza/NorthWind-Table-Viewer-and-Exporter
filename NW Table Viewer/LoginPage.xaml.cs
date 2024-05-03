using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.Xml;
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

namespace NW_Table_Viewer
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void RegisterToButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegisterPage());
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                CredentialsError.Visibility = Visibility.Hidden;

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
                con.Open();
                string searchUserData = "SELECT COUNT(*) FROM [LoginDB].[dbo].[tblUser] WHERE UserName = @UserName and Password=@Password";

                SqlCommand cmd = new SqlCommand(searchUserData, con);


                    cmd.Parameters.AddWithValue("@UserName", UserNameBox.Text);
                    cmd.Parameters.AddWithValue("@Password", passwordBox.Password);

                    cmd.ExecuteNonQuery();
                    int Count = (int)cmd.ExecuteScalar();
                    con.Close();
                    UserNameBox.Text = "";
                    passwordBox.Password = "";
                if (Count > 0)
                {
                    this.NavigationService.Navigate(new HomePage());
                    con.Close();

                }
                else
                {
                    CredentialsError.Visibility= Visibility.Visible;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
