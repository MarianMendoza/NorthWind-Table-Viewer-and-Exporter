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
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
using System.IO;
using Paragraph = iTextSharp.text.Paragraph;

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


/// <summary>
/// This will view whatever database depending on combo box selection. This will determine, query, colname, and colhidden.
/// </summary>
/// <param name="SelectedDatabase"> String input from comboboxtable</param>
        private void LoadData(string SelectedDatabase)
        {
            string query = "";
            string[] ColNames = null;
            string[] ColHidden = null;

            switch (SelectedDatabase)
            {
                case "Customers":
                    SelectedTable.Columns.Clear();
                    query = "SELECT * FROM [master].[dbo].[Customers]";
                    ColNames = new string[] { "CustomerID", "CompanyName", "ContactName", "ContactName", "ContactTitle", "Address", "City", "Region", "PostalCode", "Country", "Phone", "Fax" };
                    ColHidden = new string[] { "Address", "Region", "PostalCode" };

                    break;
                case "Employees":
                    SelectedTable.Columns.Clear();
                    query = "SELECT * FROM [master].[dbo].[Employees]";
                    ColNames = new string[] { "EmployeeID", "LastName", "FirstName", "Title", "TitleOfCourtesy", "BirthDate", "HireDate", "Address", "City", "Region", "PostalCode", "Country", "HomePhone", "Extension", "Photo", "Notes", "ReportsTo", "PhotoPath" };
                    ColHidden = new string[] { "BirthDate", "Region", "Photo", "Notes", "PhotoPath" };

                    break;
                case "Orders":
                    SelectedTable.Columns.Clear();

                    query = "SELECT * FROM [master].[dbo].[Orders]";
                    ColNames = new string[] { "OrderID", "CustomerID", "EmployeeID", "OrderDate", "RequiredDate", "ShippedDate", "ShipVia", "Freight", "ShipName", "ShipAddress", "ShipCity", "ShipRegion", "ShipPostalCode", "ShipCountry" };
                    ColHidden = new string[] { "Freight", "ShipRegion" };
                    break;
                default:
                    query = "SELECT * FROM [master].[dbo].[Customers]";
                    ColNames = new string[] { "CustomerID", "CompanyName", "ContactName", "ContactName", "ContactTitle", "Address", "City", "Region", "PostalCode", "Country", "Phone", "Fax" };
                    ColHidden = new string[] { "Address", "Region", "PostalCode" };
                    return;
            }
            Debug.WriteLine(query, ColNames, ColHidden);
            Console.WriteLine(query, ColNames);

            BindToDataTable(ColNames, ColHidden);
            getData(query);

            MainTable.ItemsSource = MainDataTable.DefaultView;
        }


        /// <summary>
        /// This binds columns and hides columns based on the lists provided.
        /// </summary>
        /// <param name="ColNames">Columns added to the table, and chekcboxes in the filtercolumns</param>
        /// <param name="ColHidden">Columns that should be hidden when switched views.</param>
        private void BindToDataTable(string[] ColNames, string[] ColHidden)
        {
            MainDataTable.Columns.Clear();
            SelectedDataTable.Columns.Clear();
            SelectedTable.ItemsSource = null;
            MainTable.ItemsSource = null;

            MainTable.Columns.Clear();
            MainDataTable.Rows.Clear();

            ColumnsVisibilityComboBox.Items.Clear();

            foreach (string colName in ColNames)
            {
                DataGridTextColumn col = new DataGridTextColumn();
                col.Header = colName;
                col.Binding = new Binding(colName);

                SelectedTable.Columns.Add(col);

                foreach (string colsHidden in ColHidden)
                {
                    if (colName == colsHidden)
                    {
                        col.Visibility = Visibility.Hidden;
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

                    }
                }
                CheckBox checkItem = new CheckBox();
                if (col.Visibility == Visibility.Hidden)
                {
                    checkItem.IsChecked = false;
                }
                else
                {
                    checkItem.IsChecked = true;
                }
                checkItem.BorderBrush = null;
                checkItem.SetResourceReference(BackgroundProperty, "CheckBoxBG");
                checkItem.SetResourceReference(ForegroundProperty, "TextColour");

                checkItem.Content = col.Header;
                checkItem.Checked += CheckBox_Checked;
                checkItem.Unchecked += CheckBox_Unchecked;
                ColumnsVisibilityComboBox.Items.Add(checkItem);
            }
        }


        /// <summary>
        /// If the box is checked it will show the column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            string colName = "";
            colName += (string)((CheckBox)sender).Content;

            foreach (DataGridColumn col in MainTable.Columns)
            {
                if ($"{col.Header}" == colName)
                {
                    col.Visibility = Visibility.Visible;
                }
            }

            foreach (DataGridColumn col in SelectedTable.Columns)
            {
                if ($"{col.Header}" == colName)
                {
                    col.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// If the checkbox is unchecked it will hide the column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            string colName = "";
            colName += (string)((CheckBox)sender).Content;

            foreach (DataGridColumn col in MainTable.Columns)
            {
                if ($"{col.Header}" == colName)
                {
                    col.Visibility = Visibility.Hidden;
                }
            }

            foreach (DataGridColumn col in SelectedTable.Columns)
            {
                if ($"{col.Header}" == colName)
                {
                    col.Visibility = Visibility.Hidden;
                }
            }

        }

        /// <summary>
        /// This will fill the table by connection to the database and making rows.
        /// </summary>
        /// <param name="query">Query that is provided above by loaddata.</param>

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

        /// <summary>
        /// Adds items to tablecombobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TableComboBox_Initialized(object sender, EventArgs e)
        {
            TableComboBox.Items.Add("Customers");
            TableComboBox.Items.Add("Employees");
            TableComboBox.Items.Add("Orders");

        } 

        /// <summary>
        /// This will allow us to search in the datatables based on the different table views.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchDataGrid_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchString = SearchDataGrid.Text;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                switch (TableComboBox.SelectedItem)
                {
                    case "Employees":
                        cmd.CommandText = @"SELECT * FROM [master].[dbo].[Employees] WHERE [EmployeeID] LIKE @searchText OR [FirstName] LIKE @searchText OR [LastName] LIKE @searchText";
                        cmd.Parameters.AddWithValue("@searchText", $"%{searchString}%");
                        cmd.Connection = con;
                        MainDataTable.Clear();
                        
                        break;
                    case "Customers":
                        cmd.CommandText = @"SELECT * FROM [master].[dbo].[Customers] WHERE [CustomerID] LIKE @searchText OR [CompanyName] LIKE @searchText OR [ContactName] LIKE @searchText";
                        cmd.Parameters.AddWithValue("@searchText", $"%{searchString}%");
                        cmd.Connection = con;
                        MainDataTable.Clear();

                        break;
                    case "Orders":
                        cmd.CommandText = @"SELECT * FROM [master].[dbo].[Orders] WHERE [OrderID] LIKE @searchText OR [CustomerID] LIKE @searchText OR [EmployeeID] LIKE @searchText";
                        cmd.Parameters.AddWithValue("@searchText", $"%{searchString}%");
                        cmd.Connection = con;
                        MainDataTable.Clear();

                        break;

                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(MainDataTable);
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }


            
        }



        /// <summary>
        /// If the selection is changed loaddata.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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

        /// <summary>
        /// If the logout button is pressed then the user is returned to the login page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LoginPage());
        }

        /// <summary>
        /// Row selection, adding items into the selected datatable to view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void MainTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataTable selectedDataTable = MainDataTable.Clone();

            if (MainTable.SelectedItems.Count > 0)
            {


                for (int i = 0; i < MainTable.SelectedItems.Count; i++)
                {

                    int numSelected = i + 1;
                    CountLabel.Content = numSelected.ToString();

                }

                foreach (DataRowView selectedItem in MainTable.SelectedItems)
                {
                    DataRow dr = selectedDataTable.NewRow();
                    dr.ItemArray = selectedItem.Row.ItemArray;
                    selectedDataTable.Rows.Add(dr);
                }
                SelectedTable.ItemsSource = selectedDataTable.DefaultView;


            }
            else
            {
                CountLabel.Content = "0";
                SelectedTable.ItemsSource = null;
                SelectedTable.Items.Clear();
            }


        }

        /// <summary>
        /// If the export button is clicked, then export the items in the selected table, check if pdf, or csv, if all check is selected export main table else, use SelectedDataTable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            DataTable SelectedDataTable = MainDataTable.Clone();

            foreach (DataRowView SelectedItem in MainTable.SelectedItems)
            {
                DataRow dr = SelectedDataTable.NewRow();
                dr.ItemArray = SelectedItem.Row.ItemArray;
                SelectedDataTable.Rows.Add(dr);
            }

            if (PdfCheck.IsChecked == true)
            {
                if (SelectCheck.IsChecked == true)
                {

                    ExportPdfTable(MainDataTable);
                }
                else
                {
                    ExportPdfTable(SelectedDataTable);
                }
            }

            if (CSVCheck.IsChecked == true)
            {
                if (SelectCheck.IsChecked == true)
                {
                    ExportCSVTable(MainDataTable);
                }
                else
                {
                    ExportCSVTable(SelectedDataTable);
                }
            }

        }


        /// <summary>
        /// This allows us to export to pdf with iTextSharp
        /// </summary>
        /// <param name="DataTable">This is determined on wether the allcheck button is checked or not, Options are MainDataTable, or SelectedDataTable</param>
        private void ExportPdfTable(DataTable DataTable)


        {
            SaveFileDialog exportDialog = new SaveFileDialog();
            exportDialog.Filter = "Portable Document File (*.pdf)|*.pdf";

            if (exportDialog.ShowDialog() == true)
            {
                Document doc = new Document(PageSize.A4.Rotate(), 1, 1, 1, 1);
                PdfPTable pdftable = new PdfPTable(DataTable.Columns.Count);
                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(exportDialog.FileName, FileMode.Create));
                doc.Open();


                //Report header
                string strHeader = "Table Exported";
                BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fntHead = new Font(bfntHead, 16, 1, BaseColor.GRAY);
                Paragraph prgHeading = new Paragraph();
                prgHeading.Alignment = Element.ALIGN_CENTER;
                prgHeading.Add(new Chunk(strHeader.ToUpper(), fntHead));
                doc.Add(prgHeading);

                Paragraph prgElements = new Paragraph();
                BaseFont btnElements = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font ftnElements = new Font(btnElements, 8, 2, BaseColor.GRAY);
                prgElements.Alignment = Element.ALIGN_RIGHT;
                prgElements.Add(new Chunk("\nDate :" + DateTime.Now.ToShortDateString(), ftnElements));
                doc.Add(prgElements);

                Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 0.0F)));
                doc.Add(p);
                doc.Add(new Chunk("\n", fntHead));

                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);

                Font text = new Font(bf, 11, Font.NORMAL);
                for (int j = 0; j < DataTable.Columns.Count; j++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(DataTable.Columns[j].ToString(), text));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    pdftable.AddCell(cell);
                }

                foreach (DataRow Row in DataTable.Rows)
                {
                    foreach (object Item in Row.ItemArray)
                    {
                        pdftable.AddCell(Item.ToString());
                    }
                }

                doc.Add(pdftable);
                doc.Close();
                MessageBox.Show("PDF Exporter Successfully");

            }
        }


        /// <summary>
        /// Exports to csv
        /// </summary>
        /// <param name="datatable">This is determined in the export button click, checked if allselect is checked or not. Options are MainDataTable, or SelectedDataTable</param>
        private void ExportCSVTable(DataTable datatable)
        {

            try

            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Comma Seperate Values (*.csv)|*.csv";
                string[] outputCsv = new string[datatable.Rows.Count + 1];

                if (sfd.ShowDialog() == true)
                {
                    CSVLibraryAK.CSVLibraryAK.Export(sfd.FileName, datatable);
                    MessageBox.Show("CSV Exporter Successfully");

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);


            }
        }

        /// <summary>
        /// If the select check is checked, then select whole table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCheck_Checked(object sender, RoutedEventArgs e)
        {
            MainTable.SelectAll();
        }


        /// <summary>
        /// If the select check is unselected then unselect.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            MainTable.UnselectAll();
        }

        /// <summary>
        /// Add items in ColumnSizeComboBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColumnSizeComboBox_Initialized(object sender, EventArgs e)
        {
            ColumnSizeComboBox.Items.Add("1");
            ColumnSizeComboBox.Items.Add("2");
            ColumnSizeComboBox.Items.Add("3");
            ColumnSizeComboBox.Items.Add("default");


        }

        /// <summary>
        /// Based on ColumSizeComboBox selection, then change the table height and font size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColumnSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedColumnSize = ColumnSizeComboBox.SelectedIndex + 1;
            switch (selectedColumnSize)
            {
                case 1:
                    MainTable.RowHeight = 15;
                    MainTable.FontSize = 11;
                    break;
                case 2:
                    MainTable.RowHeight = 20;
                    MainTable.FontSize = 15;
                    break;
                case 3:
                    MainTable.RowHeight = 30;
                    MainTable.FontSize = 17;
                    break;
                default:
                    MainTable.RowHeight = 21;
                    MainTable.FontSize = 14;
                    break;
            }
        }

    }
}
