using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2048.Commands;
using _2048.Data;
using _2048.Models;
using _2048.ViewModels.Base;

namespace _2048.ViewModels
{
    public class StatisticsViewModel : ViewModel
    {
        public static NavigationCommand NavigateToMenuPage
        {
            get => new(NavigateToPage, new Uri("Views/Pages/MenuPage.xaml", UriKind.RelativeOrAbsolute));
        }

        public static ObservableCollection<Player> StatisticsCollection { get => Statistics.Players; }
    }
}
