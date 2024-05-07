using System.Configuration;
using System.Data;
using System.Windows;

namespace NW_Table_Viewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    internal class AppThemes
    {
        public static void ChangeTheme(Uri theme)
        {
            ResourceDictionary Theme = new ResourceDictionary() { Source = theme };
            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(Theme);
        }
    }

}
