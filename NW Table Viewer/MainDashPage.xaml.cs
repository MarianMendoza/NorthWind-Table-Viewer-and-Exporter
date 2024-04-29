using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for MainDashPage.xaml
    /// </summary>
    public partial class MainDashPage : Page
    {
        private DataTable MainDataTable = new DataTable();
        private DataTable SelectDataTable = new DataTable();

        public MainDashPage()
        {
            InitializeComponent();
            LoadData("Customers");
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LoginPage());
        }

        private void LoadData(string SelectedDatabase)
        {
            string Query = "";
            string[] ColNames = null;
            string[] HiddenCols = null;


            switch (SelectedDatabase)
            {

                case "Customers":
                   //SelectTable.Columns.Clear();

                    Query = "SELECT * [CustomerID],[CompanyName]      ,[ContactName]      ,[ContactTitle]      ,[Address]      ,[City]      ,[Region]      ,[PostalCode]      ,[Country]      ,[Phone]      ,[Fax]  FROM [master].[dbo].[Customers]";
                    HiddenCols = new string[] { "ContactTitle", "Region" };
                    ColNames = new string[] { "CustomerID", "CompanyName", "ContactName", "ContactTitle", "Address", "City", "Region", "PostalCode", "Country", "Phone", "Fax" };
                    break;
                case "Employees":
                    //SelectTable.Columns.Clear();

                    Query = "";
                    HiddenCols = new string[] { };
                    ColNames = new string[] { };
                    break;
                case "Orders":
                    //SelectTable.Columns.Clear();

                    Query = "";
                    HiddenCols = new string[] { };
                    ColNames = new string[] { };
                    break;
                case "Order Details":
                    //SelectTable.Columns.Clear();

                    Query = "";
                    HiddenCols = new string[] { };
                    ColNames = new string[] { };
                    break;
                case "Products":
                    //SelectTable.Columns.Clear();

                    Query = "";
                    HiddenCols = new string[] { };
                    ColNames = new string[] { };
                    break;
                case "Shippers":
                    //SelectTable.Columns.Clear();

                    Query = "";
                    HiddenCols = new string[] { };
                    ColNames = new string[] { };
                    break;
                case "Suppliers":
                    //SelectTable.Columns.Clear();

                    Query = "";
                    HiddenCols = new string[] { };
                    ColNames = new string[] { };
                    break;
                default:
                    SelectTable.Columns.Clear();

                    Query = "SELECT * [CustomerID],[CompanyName]      ,[ContactName]      ,[ContactTitle]      ,[Address]      ,[City]      ,[Region]      ,[PostalCode]      ,[Country]      ,[Phone]      ,[Fax]  FROM [master].[dbo].[Customers]";
                    HiddenCols = new string[] { "ContactTitle", "Region" };
                    ColNames = new string[] { "CustomerID", "CompanyName", "ContactName", "ContactTitle", "Address", "City", "Region", "PostalCode", "Country", "Phone", "Fax" };
                    return;
            }
            BindToDataTable(ColNames, HiddenCols);
            GetData(Query);
            MainTable.ItemsSource = MainDataTable.DefaultView;

        }

        private void CheckBox_Check(object sender, RoutedEventArgs e)
        {
            string ColName = "";
            ColName += (string)((CheckBox)sender).Content;
            foreach (DataGridColumn Col in MainTable.Columns)
            {
                if ($"{Col.Header}" == ColName)
                {
                    Col.Visibility = Visibility.Visible;
                }
            }

            foreach (DataGridColumn Col in SelectTable.Columns)
                if ($"{Col.Header}" == ColName)
                {
                    Col.Visibility = Visibility.Visible;
                }
        }


        private void UnCheckBox_Check(object sender, RoutedEventArgs e)
        {
            string ColName = "";
            ColName += (string)((CheckBox)sender).Content;
            foreach (DataGridColumn Col in MainTable.Columns)
            {
                if ($"{Col.Header}" == ColName)
                {
                    Col.Visibility = Visibility.Hidden;
                }
            }

            foreach (DataGridColumn Col in SelectTable.Columns)
                if ($"{Col.Header}" == ColName)
                {
                    Col.Visibility = Visibility.Hidden;
                }
        }


        private void BindToDataTable(string[] ColNames, string[] HiddenCols)
        {
            MainDataTable.Columns.Clear();
            SelectDataTable.Columns.Clear();
            //SelectTable.ItemsSource = null;

            //MainTable.Columns.Clear();
            MainDataTable.Rows.Clear();

            //ColumnsVisibilityComboBox.Items.Clear();

            foreach (string ColumnName in ColNames)
            {
                DataGridTextColumn Col = new DataGridTextColumn();
                Col.Header = ColumnName;
                Col.Binding = new Binding(ColumnName);
                SelectTable.Columns.Add(Col);

                foreach (string ColumnHidden in HiddenCols)
                {
                    if (ColumnName == ColumnHidden)
                    {
                        Col.Visibility = Visibility.Hidden;
                    }
                }
            }


            foreach (string ColumnName in ColNames)
            {
                DataGridTextColumn Col = new DataGridTextColumn();
                Col.Header = ColumnName;
                Col.Binding = new Binding(ColumnName);
                MainTable.Columns.Add(Col);

                foreach (string ColumnHidden in HiddenCols)
                {
                    if (ColumnName == ColumnHidden)
                    {
                        Col.Visibility = Visibility.Hidden;
                    }
                }
                CheckBox CheckColItem = new CheckBox();
                if (Col.Visibility == Visibility.Hidden)
                {
                    CheckColItem.IsChecked = false;
                }
                else
                {
                    CheckColItem.IsChecked = true;
                }

                CheckColItem.Content = Col.Header;
                CheckColItem.Checked += CheckBox_Check;
                CheckColItem.Unchecked += UnCheckBox_Check;
                ColumnsVisibilityComboBox.Items.Add(CheckColItem);
            }
            MainTable.ItemsSource = MainDataTable.DefaultView;

        }



        private void GetData(string query)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(MainDataTable);
            MainTable.ItemsSource = MainDataTable.DefaultView;
        }


        private void TableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string SelectedDatabase = TableComboBox.SelectedItem.ToString();
            LoadData(SelectedDatabase);
        }

        private void TableComboBox_Initialized(object sender, EventArgs e)
        {
            TableComboBox.Items.Add("Customers");
            TableComboBox.Items.Add("Employees");
            TableComboBox.Items.Add("Orders");
            TableComboBox.Items.Add("Order Detail");
            TableComboBox.Items.Add("Products");
            TableComboBox.Items.Add("Shippers");
            TableComboBox.Items.Add("Suppliers");
            TableComboBox.SelectedIndex = 0;


        }
    }
}
