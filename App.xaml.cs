// <copyright file="App.xaml.cs" company="VIBES.technology">
// Copyright (c) VIBES.technology. All rights reserved.
// </copyright>

namespace LevelBarApp
{
    using LevelBarApp.ViewModels;
    using LevelBarApp.Views;
    using LevelBarGeneration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            ConfigureServices();
            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    ConfigureServices();

        //    var mainWindow = ServiceProvider.GetService<MainWindow>();
        //    mainWindow.Show();

        //    base.OnStartup(e);
        //}

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ILevelBarGenerator, LevelBarGenerator>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
