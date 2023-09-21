
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LevelBarApp.Views
{
    /// <summary>
    /// Interaction logic for ManualLevelBarControl.xaml
    /// </summary>
    public partial class ManualLevelBarControl : UserControl
    {
        private GradientStop greenGradient, greenYellowGradient, yellowGradient, orangeRedGradient, redGradient;
        private double levelRange;
        private Timer _timer;

        /// <summary>
        /// A manual Level Bar control 
        /// </summary>
        public ManualLevelBarControl()
        {
            InitializeComponent();

            levelBar.MaxHeight = levelBarContainer.Height;
            levelBar.MinHeight = 0;

            levelRange = levelBarContainer.Height / 5;

            var levelRangeOffset = (levelRange) / levelBarContainer.Height;
            currentValue.Text = (levelBar.Height / levelBarContainer.Height).ToString("0.000");

            SetupLevelBarColors(levelRangeOffset);
            InitTimer();
        }

        private void increaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (levelBar.Height < levelBarContainer.Height)
            {
                levelBar.Height += 10;
                if (levelBar.Height > levelBarContainer.Height)
                    levelBar.Height = levelBarContainer.Height;
                UpdateLevelBar();
            }
        }


        private void decreaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (levelBar.Height > 0)
            {
                levelBar.Height -= 10;
                UpdateLevelBar(true);
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            levelBar.Height = 0;
            UpdateLevelBar();
        }

        private void UpdateLevelBar(bool valueDecreased = false)
        {
            currentValue.Text = (levelBar.Height / levelBarContainer.Height).ToString("0.000");

            if (levelBar.Height == 0)
            {
                brush.GradientStops.Clear();
            }
            else if (levelBar.Height > 0 && levelBar.Height <= levelRange)
            {
                System.Console.WriteLine("Should be green");

                if (!brush.GradientStops.Contains(greenGradient))
                {
                    System.Console.WriteLine("Adding green");
                    // Update the gradient stops for the green range
                    brush.GradientStops.Add(greenGradient);
                }

                if (valueDecreased && brush.GradientStops.Contains(greenGradient))
                {
                    if (brush.GradientStops.Contains(greenYellowGradient))
                        brush.GradientStops.Remove(greenYellowGradient);

                    if (levelBar.Height == 0)
                    {
                        System.Console.WriteLine("Removing green");
                        // Update the gradient stops for the green range
                        brush.GradientStops.Remove(greenGradient);
                    }
                }
            }
            else if (levelBar.Height > levelRange && levelBar.Height <= levelRange * 2)
            {
                System.Console.WriteLine("Should be green yellow");

                if (!brush.GradientStops.Contains(greenYellowGradient))
                {
                    System.Console.WriteLine("Adding green yellow");
                    // Update the gradient stops for the green yellow range
                    brush.GradientStops.Add(greenYellowGradient);
                }

                if (valueDecreased && brush.GradientStops.Contains(greenYellowGradient))
                {
                    if (brush.GradientStops.Contains(yellowGradient))
                        brush.GradientStops.Remove(yellowGradient);

                    if (levelBar.Height == levelRange)
                    {
                        System.Console.WriteLine("Removing green yellow");
                        brush.GradientStops.Remove(greenYellowGradient);
                    }
                }
            }
            else if (levelBar.Height > levelRange * 2 && levelBar.Height <= levelRange * 3)
            {
                System.Console.WriteLine("Should be yellow");

                if (!brush.GradientStops.Contains(yellowGradient))
                {
                    System.Console.WriteLine("Adding yellow");
                    // Update the gradient stops for the yellow range
                    brush.GradientStops.Add(yellowGradient);
                }

                if (valueDecreased && brush.GradientStops.Contains(yellowGradient))
                {
                    if (brush.GradientStops.Contains(orangeRedGradient))
                        brush.GradientStops.Remove(orangeRedGradient);

                    if (levelBar.Height == levelRange * 2)
                    {
                        System.Console.WriteLine("Removing yellow");
                        brush.GradientStops.Remove(yellowGradient);
                    }
                }
            }
            else if (levelBar.Height > levelRange * 3 && levelBar.Height <= levelRange * 4)
            {
                System.Console.WriteLine("Should be orange red");

                if (!brush.GradientStops.Contains(orangeRedGradient))
                {
                    System.Console.WriteLine("Adding orange red");
                    // Update the gradient stops for the orange red range
                    brush.GradientStops.Add(orangeRedGradient);
                }

                if (valueDecreased && brush.GradientStops.Contains(orangeRedGradient))
                {
                    if (brush.GradientStops.Contains(redGradient))
                        brush.GradientStops.Remove(redGradient);

                    if (levelBar.Height == levelRange * 3)
                    {
                        System.Console.WriteLine("Removing orange red");
                        brush.GradientStops.Remove(orangeRedGradient);
                    }
                }

            }
            else if (levelBar.Height > levelRange * 4 && levelBar.Height <= levelRange * 5)
            {
                System.Console.WriteLine("Should be red");

                if (!brush.GradientStops.Contains(redGradient))
                {
                    System.Console.WriteLine("Adding red");
                    brush.GradientStops.Add(redGradient);
                }

                if (valueDecreased && brush.GradientStops.Contains(redGradient) && levelBar.Height == levelRange * 4)
                {
                    System.Console.WriteLine("Removing red");
                    brush.GradientStops.Remove(redGradient);
                }


            }
            System.Console.WriteLine($"Max height: {levelBarContainer.Height}");
            System.Console.WriteLine($"LevelRange: {levelRange}");
            System.Console.WriteLine($"LevelBar heigh: {levelBar.Height}");

            levelBar.InvalidateVisual();

        }

        private void SetupLevelBarColors(double levelRangeOffset)
        {
            System.Console.WriteLine($" Level 1: {levelRangeOffset}");
            System.Console.WriteLine($"Level 2: {levelRangeOffset * 2}");
            System.Console.WriteLine($"Level 3: {levelRangeOffset * 3}");
            System.Console.WriteLine($"Level 4: {levelRangeOffset * 4}");
            System.Console.WriteLine($"Level 5: {levelRangeOffset * 5}");

            greenGradient = new GradientStop(Colors.Green, levelRangeOffset);
            greenYellowGradient = new GradientStop(Colors.GreenYellow, levelRangeOffset * 2);
            yellowGradient = new GradientStop(Colors.Yellow, levelRangeOffset * 3);
            orangeRedGradient = new GradientStop(Colors.Orange, levelRangeOffset * 4);
            redGradient = new GradientStop(Colors.Red, levelRangeOffset * 5);
        }

        /// <summary>
        /// Timer to refhresh the peak hold value
        /// </summary>
        public void InitTimer()
        {
            _timer = new Timer();
            _timer.Elapsed += timer_Elapsed;
            _timer.Interval = 5 * 1000; // 5 seconds
            _timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                peakValue.Text = (levelBar.Height / levelBarContainer.Height).ToString("0.000");

            });
        }
    }
}
