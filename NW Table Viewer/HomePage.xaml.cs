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
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Policy;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace NW_Table_Viewer
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {

        //W
        public DataTable MainDataTable = new DataTable();
        public DataTable SelectedDataTable = new DataTable();
        public HomePage()
        {
           // Debug.WriteLine("Hello World!");
            InitializeComponent();
            TableComboBox.SelectedIndex = 0;
 
        }

        private void LoadData(string SelectedDatabase)
        {
            string query = "";
            string[] ColNames = null;
            string[] ColHidden = null;

            switch(SelectedDatabase)
            {
                case "Customers":
                    SelectedTable.Columns.Clear();
                    query = "SELECT * FROM [master].[dbo].[Customers]";
                    ColNames = new string[] {"CustomerID","CompanyName","ContactName","ContactName","ContactTitle","Address","City","Region","PostalCode","Country","Phone","Fax"};
                    ColHidden = new string[] {"Address","Region","PostalCode"};

                    break;
                case "Employees":
                    SelectedTable.Columns.Clear();
                    query = "SELECT * FROM [master].[dbo].[Employees]";
                    ColNames = new string[] { "EmployeeID", "LastName","FirstName","Title","TitleOfCourtesy","BirthDate","HireDate","Address","City","Region","PostalCode","Country","HomePhone","Extension","Photo","Notes","ReportsTo","PhotoPath" };
                    ColHidden = new string[] { "BirthDate", "Region","Photo", "Notes","PhotoPath" };

                    break;
                default:
                    query = "SELECT * FROM [master].[dbo].[Customers]";
                    ColNames = new string[] { "CustomerID", "CompanyName", "ContactName", "ContactName", "ContactTitle", "Address", "City", "Region", "PostalCode", "Country", "Phone", "Fax" };
                    ColHidden = new string[] { "Address", "Region", "PostalCode" };
                    return;
            }
            Debug.WriteLine(query, ColNames,ColHidden);
            Console.WriteLine(query, ColNames);

            BindToDataTable(ColNames, ColHidden);
            getData(query);

            MainTable.ItemsSource = MainDataTable.DefaultView;
        }

        private void BindToDataTable(string[] ColNames, string[] ColHidden)
        {
            MainDataTable.Columns.Clear();
            SelectedDataTable.Columns.Clear();
            SelectedTable.ItemsSource = null;
            MainTable.ItemsSource = null;

            MainTable.Columns.Clear();
            MainDataTable.Rows.Clear();

            ColumnsVisibilityComboBox.Items.Clear();

            foreach(string colName in ColNames)
            {
                DataGridTextColumn col = new DataGridTextColumn();
                col.Header = colName;
                col.Binding = new Binding(colName);

                SelectedTable.Columns.Add(col);

                foreach(string colsHidden in ColHidden)
                {
                    if (colName == colsHidden)
                    {
                        col.Visibility = Visibility.Hidden;
                        break;
                    }
                }
            }

            foreach (string colName in ColNames)
            {
                DataGridTextColumn col = new DataGridTextColumn();
                col.Header = colName;
                col.Binding = new Binding(colName);

                MainTable.Columns.Add(col);

                foreach (string colsHidden in ColHidden)
                {
                    if (colName == colsHidden)
                    {
                        col.Visibility = Visibility.Hidden;
                        break;
                    }
                }
                CheckBox checkItem = new CheckBox();
                if (col.Visibility == Visibility.Hidden)
                {
                    checkItem.IsChecked = false;
                } else
                {
                    checkItem.IsChecked = true;
                }
                //checkitem.borderbursh = null;
                //checkitem.setresourcereference(backgroundproperty, "")\
                //checkitem.setresourcereference(foregroundproperty, "")

                checkItem.Content = col.Header;
                //checkItem.Checked += CheckBox_Checked;
                //checkItem.Unchecked += CheckBox_Unchecked;
                ColumnsVisibilityComboBox.Items.Add(checkItem);
            }
        }

        private void getData(string query)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(MainDataTable);
            MainTable.ItemsSource = MainDataTable.DefaultView;
            con.Close();
        }


        private void TableComboBox_Initialized(object sender, EventArgs e)
        {
            TableComboBox.Items.Add("Customers");
            TableComboBox.Items.Add("Employees");
           
        }


        private void TableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string SelectedDatabase = TableComboBox.SelectedItem.ToString();
            Debug.WriteLine(SelectedDatabase);
            if (SelectedDatabase != null)
            {
                LoadData(SelectedDatabase);

            }
            else
            {
                Debug.WriteLine("Test");
            }
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LoginPage());
        }

    }
}
