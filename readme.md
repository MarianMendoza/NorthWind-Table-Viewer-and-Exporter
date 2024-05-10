![NorthWingLogo](/NW%20Table%20Viewer/Resources/logo.png)


# Northwind Table Viewer and Exporter

The Northwind Table Viewer and Exporter is a desktop application that allows you to view the Customers, Employees, and Orders tables within the Northwind database. This application will allow you to change between table views, search ID and names, filter columns, and change rows sizes for comfort. It also allows user to select rows, and export those rows, in .pdf, and .csv, alternatively selecting the whole table and exporting that in 2 clicks. The main focus of this application is to implement a simplistic and enhance user experience. This was done by implementing a minimalistic style. To get started users, must create an account. Check out below to learn more.

This desktop application is similar to my workplace project. This application was deployed in the team and used to search data in massive databases.

## How To Install
Simply download the github folder, run the solution in visual studio. Beware you do need some packages in order to use this. When launched simply sign up and register. Login to access the table viewer .Read below to find out more.  

Alternatively you can also publish this, when you open this in Visual Studio, simply go into properties and press publish. This will supply you with an installation file, making a .exe, on your desktop.

## System Requirements
* Windows
* .NET Framework 3.0 or higher
* Visual Studio 2022 or higher
* NuGet Extension

## Packages Used

* CommunityToolkit.WinUI.UI.Controls
* CSVLibraryAK
* EntityFramework
* iTextSharp
* Microsoft.Bcl.AsyncInterfaces
* System.Buffers
* System.ComponentModel.Annotations
* System.Memory
* System.Numerics.Vectors
* System.Runtime.CompilerServices.Unsafe
* System.Threading.Tasks.Extensions

## Usage Overview
[![](https://markdown-videos-api.jorgenkh.no/youtube/2Ibml2FGJUI)](https://youtu.be/2Ibml2FGJUI)]


Listed features within the desktop application

* Register
* Login
* Cell Sort
* Table View Change
* Search (Order Id, Employee ID, CustomerID, FirstName, LastName)
* Hide/Show Columns
* Columns Density Change
* Export PDF
* Export CSV
* Export Whole Table
* Export Selected Rows
* Theme change (Light/Dark Mode)


#### Register

Once the application is launched, if you have an account enter your credentials and sign in. Otherwise, simply press the SIGN UP button below. This will bring you to the registration page. Fill in your desired username and password. Press signup and if a message box appears your account has been made successfully.

#### Login

After registration, go back to the login page, enter your credentials and you should be able to make 

#### Cell Sort

Simply click on the column header, and the column with sort in ascending order. 

#### Table View Change

Select the table view by clicking on the column box that is left to the search bar. You will be presented a drop down menu, with 3 options, Customers, Employees and Orders. Click one to switch between table views.

#### Search

Simply type in the search bar to search for ID. The searchable columns are CustomerId, EmployeeId, OrderId, and FirstName, LastName, and Company Name. This is only searchable by these columns as complexity for searching all cells is quite high. This was mainly done to make improve the complexity as it was done through a linear search. Since this is similar to my workplace project, I implemented it the same way as the NorthWind Application because the CSO Viewer would be pointing at huge vasts of data. 


#### Hide/Show Columns

To the left of the search bar the combo box provides the option to hide/show columns. By default some columns are hidden. This can be changed by checking the box. Similarly to hide a column simply uncheck the box. This selection of hidden / shown columns will correspond in both tables.

#### Columns Density Change

Next to the hide/show columns combo box. There is a button with the image of columns. A dropdown will provide you 4 different settings. These allow the user to change the row size, and font size. This provides a more condensed or sparse look. The options are as listed, 1,2,3 and default. 1 is the most compact, allowing users to view more rows, while still being able to read the font, 2 is in between not compact nor sparse, and lastly 3, which is the most sparse option. This will increase font size. Then the option default allows you to return back to what size the view originally was. 

#### Export .PDF

To export .PDF simply check the .pdf checkbox located in between the two tables. Export selection of rows or whole table.

#### Export .CSV
To export .CSV simply check the .csv checkbox located in between the two tables. Export selection or rows or whole table.

#### Export Whole Table

To export the whole table, there is a checkbox located next to the format options. If you click this it will select the whole table. Pick your desired format by checking the boxes, and then press the export button. This will open a file dialog.

#### Export Selected Rows

Select the rows, make sure that whole table selection is unchecked. Pick your desired format, and then press the export button. This will open a file dialog, and the selected rows should be exported.


#### Change Themes 

To change the theme into dark/light mode, toggle the switch on the bottom right corner of your window. This will immediately change themes. By default it is set to light mode. For better comfort switch to dark mode.

## Conclusion

This application is to view data in the dummy database Northwind. 




