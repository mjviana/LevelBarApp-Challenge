// <copyright file="MainWindow.xaml.cs" company="VIBES.technology">
// Copyright (c) VIBES.technology. All rights reserved.
// </copyright>

namespace LevelBarApp.Views
{
    using LevelBarApp.ViewModels;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();

            //// Retrieve the ViewModel instance from App.xaml resources
            //var locator = (ViewModelLocator)Application.Current.Resources["Locator"];
            //DataContext = locator.Main;
            InitializeComponent();

            //AddLevelBarControllers(4);
        }

        private void AddLevelBarControllers(int numberOfChannels)
        {
            List<ManualLevelBarControl> levelBars = new List<ManualLevelBarControl>();

            for (int i = 0; i < numberOfChannels; i++)
            {
                levelBars.Add(new ManualLevelBarControl());
            }

            // Create a WrapPanel to contain the custom controls
            WrapPanel wrapPanel = new WrapPanel();

            wrapPanel.Orientation = Orientation.Horizontal;
            // Set spacing between the custom controls (in pixels)
            wrapPanel.Margin = new Thickness(2);

            foreach (var item in levelBars)
            {
                wrapPanel.Children.Add(item);
            }

            mainGrid.Children.Add(wrapPanel);

            // Calculate the required window size based on the number and size of controls
            double requiredWidth = CalculateRequiredWidth(wrapPanel);
            double requiredHeight = CalculateRequiredHeight(wrapPanel);

            // Set the window's size
            this.Width = requiredWidth;
            this.Height = requiredHeight;
        }

        private double CalculateRequiredWidth(WrapPanel wrapPanel)
        {
            double maxWidth = 0;

            foreach (ManualLevelBarControl customControl in wrapPanel.Children)
            {
                // Calculate the total width based on the custom controls' widths
                double controlWidth = customControl.Width; // Adjust this based on your control's actual size
                maxWidth += controlWidth;
            }

            return maxWidth;
        }

        private double CalculateRequiredHeight(WrapPanel wrapPanel)
        {
            double maxHeight = 0;

            foreach (ManualLevelBarControl customControl in wrapPanel.Children)
            {
                // Calculate the total height based on the custom controls' heights
                double controlHeight = customControl.Height; // Adjust this based on your control's actual size
                maxHeight += controlHeight;
            }

            return maxHeight;
        }

    }
}
