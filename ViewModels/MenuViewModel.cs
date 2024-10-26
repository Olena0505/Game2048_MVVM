using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2048.Commands;
using _2048.ViewModels.Base;

namespace _2048.ViewModels
{
    public class MenuViewModel : ViewModel
    {
        public NavigationCommand NavigateToGamePage
        {
            get => new(NavigateToPage, new Uri("Views/Pages/GamePage.xaml", UriKind.RelativeOrAbsolute)); 
        }
        public NavigationCommand NavigateToStatisticsPage
        {
            get => new(NavigateToPage, new Uri("Views/Pages/StatisticsPage.xaml", UriKind.RelativeOrAbsolute));
        }
        public static RelayCommand QuitCommand
        {
            get => new(Quit);
        }
    }
}
