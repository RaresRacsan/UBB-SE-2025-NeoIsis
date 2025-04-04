using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI;
using NeoIsisJob.ViewModels.Rankings;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NeoIsisJob.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RankingPage : Page
    {
        // I am sorry for whoever has to work on this code in the future. Know that the failures of this code 
        // are not by design, but a victim of time constraint, frustration and endless crashing.
        private readonly RankingsViewModel _rankingsViewModel;
        public RankingPage()
        {
            this.InitializeComponent();
            this._rankingsViewModel = new RankingsViewModel();
            this.LoadRankings();
            this.LoadMuscleGroupColors();
        }

        public void GoToMainPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        public void GoToWorkoutPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WorkoutPage));
        }

        public void GoToCalendarPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CalendarPage));
        }

        public void GoToClassPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ClassPage));
        }

        public void GoToRankingPage_Tap(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(RankingPage));
        }

        private void Page_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is not Microsoft.UI.Xaml.Shapes.Path)
            {
                LoadRankings();
            }
        }

        private void Chest_Clicked(object sender, RoutedEventArgs e)
        {
            var ranking = this._rankingsViewModel.GetRankingByMGID(1);
            if (ranking != null)
            {
                this.LoadMuscleGroupPanel(ranking.Rank, "Chest");
            }
        }

        private void Legs_Clicked(object sender, RoutedEventArgs e)
        {
            var ranking = this._rankingsViewModel.GetRankingByMGID(2);
            if (ranking != null)
            {
                this.LoadMuscleGroupPanel(ranking.Rank, "Legs");
            }
        }

        private void Arms_Clicked(object sender, RoutedEventArgs e)
        {
            var ranking = this._rankingsViewModel.GetRankingByMGID(3);
            if (ranking != null)
            {
                this.LoadMuscleGroupPanel(ranking.Rank, "Arms");
            }
        }

        private void Abs_Clicked(object sender, RoutedEventArgs e)
        {
            var ranking = this._rankingsViewModel.GetRankingByMGID(4);
            if (ranking != null)
            {
                this.LoadMuscleGroupPanel(ranking.Rank, "Abs");
            }
        }

        private void Back_Clicked(object sender, RoutedEventArgs e)
        {
            var ranking = this._rankingsViewModel.GetRankingByMGID(5);
            if (ranking != null)
            {
                this.LoadMuscleGroupPanel(ranking.Rank, "Back");
            }
        }

        private void LoadMuscleGroupPanel(int rank, String muscleGroup)
        {
            var rankingPanel = FindName("RankingPanel") as StackPanel;
            if (rankingPanel != null)
            {
                rankingPanel.Children.Clear();

                SolidColorBrush rankcolor = this._rankingsViewModel.GetRankColor(rank);

                rankingPanel.Children.Add(this.CreateMuscleGroupPanel(this._rankingsViewModel.GetRankIcon(rank), muscleGroup, rank, rankcolor));

            }
        }

        private void LoadMuscleGroupColors()
        {
            this.LoadChestColor();
            this.LoadLegsColor();
            this.LoadArmsColor();
            this.LoadAbsColor();
            this.LoadBackColor();
        }

        private void LoadChestColor()
        {
            var chestSVG = FindName("Chest") as Microsoft.UI.Xaml.Shapes.Path;

            var ranking = this._rankingsViewModel.GetRankingByMGID(1); // chest has id 1 in db
            if (chestSVG != null && ranking != null)
            {
               chestSVG.Fill = this._rankingsViewModel.GetRankColor(ranking.Rank);
            }
        }

        private void LoadLegsColor()
        {
            var legsSVG = FindName("Legs") as Microsoft.UI.Xaml.Shapes.Path;

            var ranking = this._rankingsViewModel.GetRankingByMGID(2); // legs has id 2 in db
            if (legsSVG != null && ranking != null)
            {
                legsSVG.Fill = this._rankingsViewModel.GetRankColor(ranking.Rank);
            }
        }

        private void LoadArmsColor()
        {
            var armsSVG = FindName("Arms") as Microsoft.UI.Xaml.Shapes.Path;

            var ranking = this._rankingsViewModel.GetRankingByMGID(3); // arms has id 3 in db
            if (armsSVG != null && ranking != null)
            {
                armsSVG.Fill = this._rankingsViewModel.GetRankColor(ranking.Rank);
            }
        }

        private void LoadAbsColor()
        {
            var absSVG = FindName("Abs") as Microsoft.UI.Xaml.Shapes.Path;

            var ranking = this._rankingsViewModel.GetRankingByMGID(4); // abs has id 4 in db
            if (absSVG != null && ranking != null)
            {
                absSVG.Fill = this._rankingsViewModel.GetRankColor(ranking.Rank);
            }
        }

        private void LoadBackColor()
        {
            var backSVG = FindName("Back") as Microsoft.UI.Xaml.Shapes.Path;

            var ranking = this._rankingsViewModel.GetRankingByMGID(5); // back has id 5 in db
            if (backSVG != null && ranking != null)
            {
                backSVG.Fill = this._rankingsViewModel.GetRankColor(ranking.Rank);
            }
        }

        private void LoadRankings()
        {
            var rankingPanel = FindName("RankingPanel") as StackPanel;

            if (rankingPanel != null)
            {
                rankingPanel.Children.Clear();

                rankingPanel.Children.Add(new TextBlock { Text = "All Rankings Explained:", FontSize = 25, FontWeight = FontWeights.Bold  });

                rankingPanel.Children.Add(CreateRankItem("/Assets/Ranks/Rank8.png", "Challenger", "9500", "10000", Colors.Aquamarine));
                rankingPanel.Children.Add(CreateRankItem("/Assets/Ranks/Rank7.png", "Grandmaster", "8500", "9500", Colors.OrangeRed));
                rankingPanel.Children.Add(CreateRankItem("/Assets/Ranks/Rank6.png", "Master", "7000", "8500", Colors.DarkViolet));
                rankingPanel.Children.Add(CreateRankItem("/Assets/Ranks/Rank5.png", "Elite", "5000", "7000", Colors.DarkGreen));
                rankingPanel.Children.Add(CreateRankItem("/Assets/Ranks/Rank4.png", "Gold", "3500", "5000", Colors.Gold));
                rankingPanel.Children.Add(CreateRankItem("/Assets/Ranks/Rank3.png", "Silver", "2250", "3500", Colors.Silver));
                rankingPanel.Children.Add(CreateRankItem("/Assets/Ranks/Rank2.png", "Bronze", "1000", "2250", Colors.SandyBrown));
                rankingPanel.Children.Add(CreateRankItem("/Assets/Ranks/Rank1.png", "Beginner", "0", "1000", Colors.DimGray));
            }
        }

        private StackPanel CreateMuscleGroupPanel(string imagePath, string muscleGroup, int rank, SolidColorBrush color)
        {
            StackPanel stackPanel = new StackPanel();
            StackPanel rowStackPanel = new StackPanel { Orientation = Orientation.Horizontal };

            Image rankImage = new Image { Source = new BitmapImage(new Uri(this.BaseUri, imagePath)), Width = 150, Height = 150 };
            TextBlock muscleGroupName = new TextBlock { Text = muscleGroup, FontSize = 25, Foreground = color, Margin = new Thickness(20, 60, 0, 10), FontWeight = FontWeights.Bold };
            ProgressBar progressBar = new ProgressBar { Value = rank, Minimum = this._rankingsViewModel.GetRankLowerBound(rank), Maximum = this._rankingsViewModel.GetRankUpperBound(rank), Foreground = color};
            TextBlock nextRankBlock = null;
            if (this._rankingsViewModel.GetRankUpperBound(rank) == 10000)
            {
                nextRankBlock = new TextBlock { Text = "You require " + (this._rankingsViewModel.GetRankUpperBound(rank) - rank).ToString() + " points to reach the end of this ranking!" };
            }
            else
            {
                nextRankBlock = new TextBlock { Text = "You require " + (this._rankingsViewModel.GetRankUpperBound(rank) - rank).ToString() + " points to reach the next ranking!" };
            }
            rowStackPanel.Children.Add(rankImage);
            rowStackPanel.Children.Add(muscleGroupName);
            
            stackPanel.Children.Add(rowStackPanel);
            stackPanel.Children.Add(progressBar);
            stackPanel.Children.Add(nextRankBlock);
            
            return stackPanel;
        }

        private StackPanel CreateRankItem(string imagePath, string rankName, string minScore, string maxScore, Windows.UI.Color color)
        {
            StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal };

            Image rankImage = new Image { Source = new BitmapImage(new Uri(this.BaseUri, imagePath)), Width = 50, Height = 50 };
            TextBlock rankText = new TextBlock { Text = rankName, Foreground = new SolidColorBrush(color), Width = 150, Margin = new Thickness(10, 15, 0, 0), FontWeight = FontWeights.Bold };
            TextBlock minText = new TextBlock { Text = minScore, Foreground = new SolidColorBrush(color), TextAlignment = TextAlignment.Center, Width = 50, Margin = new Thickness(25, 15, 0, 0), FontWeight= FontWeights.Bold};
            TextBlock dashText = new TextBlock { Text = "-", Width = 10, Margin = new Thickness(15, 15, 0, 0) };
            TextBlock maxText = new TextBlock { Text = maxScore, Foreground = new SolidColorBrush(color), TextAlignment = TextAlignment.Center, Width = 50, Margin = new Thickness(15, 15, 0, 0), FontWeight = FontWeights.Bold };

            stackPanel.Children.Add(rankImage);
            stackPanel.Children.Add(rankText);
            stackPanel.Children.Add(minText);
            stackPanel.Children.Add(dashText);
            stackPanel.Children.Add(maxText);

            return stackPanel;
        }

        private void Abs_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }
    }
}
