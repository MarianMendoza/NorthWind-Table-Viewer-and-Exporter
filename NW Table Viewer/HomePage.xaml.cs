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
                } else
                {
                    checkItem.IsChecked = true;
                }
                //checkitem.borderbursh = null;
                //checkitem.setresourcereference(backgroundproperty, "")\
                //checkitem.setresourcereference(foregroundproperty, "")

                checkItem.Content = col.Header;
                checkItem.Checked += CheckBox_Checked;
                checkItem.Unchecked += CheckBox_Unchecked;
                ColumnsVisibilityComboBox.Items.Add(checkItem);
            }
        }

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
    }
}
