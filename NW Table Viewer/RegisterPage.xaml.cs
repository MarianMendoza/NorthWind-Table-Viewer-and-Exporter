﻿using System;
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
using System.Windows.Navigation;
using System.Configuration;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Automation.Peers;

namespace NW_Table_Viewer
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Goes into login page if button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginToButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LoginPage());
        }


        /// <summary>
        /// When the register button is clicked it checks the database to see if the username exists already, if it does, error message. Also checks that fields are not null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterAccountButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UsernameError.Visibility = Visibility.Hidden;
                PasswordMatchError.Visibility = Visibility.Hidden;
                UserNameBox.Text = "";
                passwordBox.Password = "";
                ConfirmPasswordBox.Password = "";

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
                con.Open();
                string addUserData = "INSERT INTO [LoginDB].[dbo].[tblUser] VALUES(@UserName, @Password)";
                string searchUserData = "SELECT COUNT(*) FROM [LoginDB].[dbo].[tblUser] WHERE UserName = @UserName and Password=@Password";
                SqlCommand cmd1 = new SqlCommand(searchUserData, con);
                SqlCommand cmd = new SqlCommand(addUserData, con);

                cmd1.Parameters.AddWithValue("@UserName", UserNameBox.Text);
                cmd1.Parameters.AddWithValue("@Password", passwordBox.Password);
                cmd1.ExecuteNonQuery(); 
                int Count=(int)cmd1.ExecuteScalar();

                if (UserNameBox.Text.Length == 0 || passwordBox.Password.Length == 0)
                {
                    MessageBox.Show("Please Enter Details");
                }
                else
                {
                    if (Count > 0)
                    {
                        UsernameError.Visibility = Visibility.Visible;
                        con.Close();
                    }
                    else
                    {
                        if (passwordBox.Password != ConfirmPasswordBox.Password)
                        {
                            PasswordMatchError.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@UserName", UserNameBox.Text);
                            cmd.Parameters.AddWithValue("@Password", passwordBox.Password);
                            MessageBox.Show("Account Successfully Made!");

                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }


                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
