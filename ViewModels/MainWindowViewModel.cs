﻿// <copyright file="MainWindowViewModel.cs" company="VIBES.technology">
// Copyright (c) VIBES.technology. All rights reserved.
// </copyright>

namespace LevelBarApp.ViewModels
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using LevelBarGeneration;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// MainWindowViewModel
    /// </summary>
    /// <seealso cref="ViewModelBase" />
    public class MainWindowViewModel : ViewModelBase
    {
        // Fields
        private readonly LevelBarGenerator levelBarGenerator;
        private RelayCommand connectToGeneratorCommand;
        private RelayCommand disconnectToGeneratorCommand;

        // Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            levelBarGenerator = LevelBarGenerator.Instance;

            levelBarGenerator.GeneratorStateChanged += LevelBarGenerator_GeneratorStateChanged;
            levelBarGenerator.ChannelAdded += LevelBarGenerator_ChannelAdded;
            levelBarGenerator.ChannelLevelDataReceived += LevelBarGenerator_ChannelDataReceived;
            levelBarGenerator.ChannelRemoved += LevelBarGenerator_ChannelRemoved;
        }

        // Properties

        /// <summary>
        /// Gets or sets the level bars, one for each channel.
        /// </summary>
        /// <value>
        /// The level bars.
        /// </value>
        public ObservableCollection<LevelBarViewModel> LevelBars { get; set; } = new ObservableCollection<LevelBarViewModel>();

        /// <summary>
        /// Gets the command to connect the generator
        /// </summary>
        /// <value>
        /// The connect generator.
        /// </value>
        public RelayCommand ConnectGeneratorCommand => connectToGeneratorCommand ?? (connectToGeneratorCommand = new RelayCommand(new System.Action(async () => await levelBarGenerator.Connect())));

        /// <summary>
        /// Gets the command to disconnect the generator
        /// </summary>
        /// <value>
        /// The disconnect generator.
        /// </value>
        public RelayCommand DisconnectGeneratorCommand => disconnectToGeneratorCommand ?? (disconnectToGeneratorCommand = new RelayCommand(new System.Action(async () => await levelBarGenerator.Disconnect())));

        // Methods
        private void LevelBarGenerator_ChannelAdded(object sender, ChannelChangedEventArgs e)
        {
            System.Console.WriteLine($"Adding channel {e.ChannelId}...");
            // Generate a LevelBarViewModel
            LevelBars.Add(new LevelBarViewModel { Id = e.ChannelId });
        }

        private void LevelBarGenerator_ChannelRemoved(object sender, ChannelChangedEventArgs e)
        {
            if (LevelBars.Any())
            {
                System.Console.WriteLine($"Removing channel...{e.ChannelId}");

                // Remove the corresponding LevelBarViewModel
                var levelBarToRemove = LevelBars.Where(l => l.Id == e.ChannelId).FirstOrDefault();
                if (levelBarToRemove != null)
                    LevelBars.Remove(levelBarToRemove);
            }
            else
            {
                System.Console.WriteLine("No connected channels!");
            }
        }

        private void LevelBarGenerator_GeneratorStateChanged(object sender, GeneratorStateChangedEventArgs e)
        {
            if (e.State == GeneratorState.Running)
            {
                ConnectGeneratorCommand?.Execute(this);
            }
            else
            {
                System.Console.WriteLine("Channel disconneted!");
            }
        }

        private void LevelBarGenerator_ChannelDataReceived(object sender, ChannelDataEventArgs e)
        {
            for (int i = 0; i < e.ChannelIds.Length; i++)
            {
                var currentLevelBar = LevelBars.Where(l => l.Id == e.ChannelIds[i]).FirstOrDefault();
                if (currentLevelBar != null)
                    currentLevelBar.Level = e.Levels[i];
            }
        }
    }
}
