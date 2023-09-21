using LevelBarApp.ViewModels;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LevelBarApp.Views
{
    /// <summary>
    /// Interaction logic for LevelBarControl.xaml
    /// </summary>
    public partial class LevelBarControl : UserControl
    {
        private Timer _timer;
        private LevelBarViewModel _levelBarViewModel;

        /// <summary>
        /// Interaction logic for LevelBarControl.xaml
        /// </summary>
        public LevelBarControl()
        {
            InitializeComponent();

            Loaded += LevelBarControl_Loaded;
        }

        private void LevelBarControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Access the DataContext after it's set, to ensure that _levelBarViewModel is not null 
            _levelBarViewModel = (LevelBarViewModel)this.DataContext;
            SetPeakHoldRefresh();
        }

        /// <summary>
        /// Sets a refresh for Peak Hold value and it's background color
        /// </summary>
        public void SetPeakHoldRefresh()
        {
            _timer = new Timer();
            _timer.Elapsed += refresh_PeakHold;
            _timer.Interval = 2 * 1000; // 2 seconds
            _timer.Start();
        }

        private void refresh_PeakHold(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                var levelBarMaxHeight = levelBarContainer.Height;
                var levelBarHeight = levelBar.Height;

                SetupPeakHoldBackgroundColor(levelBarMaxHeight, levelBarHeight);

                var linearLevelValue = _levelBarViewModel.Level;
                // convert Linear value to decibel
                peakValue.Text = (20 * Math.Log10(linearLevelValue)).ToString();
            });
        }

        private void SetupPeakHoldBackgroundColor(double levelBarMaxHeight, double levelBarHeight)
        {
            if (levelBarHeight >= levelBarMaxHeight * 0.75)
            {
                peakValue.Background = Brushes.Red;
            }
            else if (levelBarHeight >= levelBarMaxHeight * 0.5)
            {
                peakValue.Background = Brushes.Orange;
            }
            else if (levelBarHeight >= levelBarMaxHeight * 0.25)
            {
                peakValue.Background = Brushes.Yellow;
            }
            else if (levelBarHeight >= levelBarMaxHeight * 0.1)
            {
                peakValue.Background = Brushes.YellowGreen;
            }
            else if (levelBarHeight > 0)
            {
                peakValue.Background = Brushes.Green;
            }
        }
    }
}
