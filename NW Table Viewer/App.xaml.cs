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


/// <summary>
/// This is how themes are changed, looking in resource dictionary.
/// </summary>
/// <param name="theme"> In the Themes folder. These are resource dictionaries.</param>
        public static void ChangeTheme(Uri theme)
        {
            ResourceDictionary Theme = new ResourceDictionary() { Source = theme };
            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(Theme);
        }
    }

}
