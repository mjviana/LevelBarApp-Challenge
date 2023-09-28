// <copyright file="LevelBarGenerator.cs" company="VIBES.technology">
// Copyright (c) VIBES.technology. All rights reserved.
// </copyright>

namespace LevelBarGeneration
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// LevelBarGenerator
    /// </summary>
    public class LevelBarGenerator : ILevelBarGenerator
    {
        // Fields

        private GeneratorState state = GeneratorState.Stopped;

        // Constructor

        /// <summary>
        /// Prevents a default instance of the <see cref="LevelBarGenerator"/> class from being created.
        /// </summary>
        public LevelBarGenerator()
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }

        // Events

        /// <summary>
        /// Occurs when [channel added].
        /// </summary>
        public event EventHandler<ChannelChangedEventArgs> ChannelAdded;

        /// <summary>
        /// Occurs when [channel removed].
        /// </summary>
        public event EventHandler<ChannelChangedEventArgs> ChannelRemoved;

        /// <summary>
        /// Occurs when [channel data received].
        /// </summary>
        public event EventHandler<ChannelDataEventArgs> ChannelLevelDataReceived;

        /// <summary>
        /// Occurs when [state changed].
        /// </summary>
        public event EventHandler<GeneratorStateChangedEventArgs> GeneratorStateChanged;

        // Properties

        CancellationTokenSource cancelTokenSource;
        CancellationToken token;

        public GeneratorState State => state;

        // Methods

        /// <summary>
        /// Connects this instance.
        /// </summary>
        /// <returns>Connect Task</returns>
        public async Task Connect()
        {

            if (state == GeneratorState.Running)
            {
                Console.WriteLine("Generator is already connected");
                return;
            }

            // What properties are used
            int channelBlockSize = 512;
            int samplingRate = 16384;
            double samplingTime = 1.0d;

            // Setup the channels
            int numberOfChannels = RegisterChannels();

            // Setup and fire the data generator
            await SetupDataGenerator(channelBlockSize, samplingRate, samplingTime, numberOfChannels);

            if (token.IsCancellationRequested)
            {
                ResetCancellationToken();
            }
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        /// <returns>Disconnect Task</returns>
        public async Task Disconnect()
        {
            if (state == GeneratorState.Stopped)
            {
                Console.WriteLine("Generator is already stopped");
                return;
            }

            cancelTokenSource.Cancel();

            DeregisterChannels();

            state = GeneratorState.Stopped;
            GeneratorStateChanged?.Invoke(this, new GeneratorStateChangedEventArgs { State = GeneratorState.Stopped });

        }

        /// <summary>
        /// Receives the level data.
        /// </summary>
        /// <param name="channelIds">The channel ids.</param>
        /// <param name="levels">The levels.</param>
        public void ReceiveLevelData(int[] channelIds, float[] levels)
        {
            ChannelLevelDataReceived?.Invoke(this, new ChannelDataEventArgs { ChannelIds = channelIds, Levels = levels });
        }

        /// <summary>
        /// Resets Cancellation token so that can be used again.
        /// </summary>
        private void ResetCancellationToken()
        {
            Console.WriteLine("Generator was disconnected");
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }

        private async Task SetupDataGenerator(int channelBlockSize, int samplingRate, double samplingTime, int numberOfChannels)
        {
            DataThroughputJob.SetupJob(samplingRate, channelBlockSize, samplingTime, numberOfChannels);
            var job = new DataThroughputJob();
            var interval = TimeSpan.FromMilliseconds((double)(channelBlockSize / 8d) / samplingRate * 1000d);

            try
            {
                while (!token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();

                    if (state != GeneratorState.Running && !token.IsCancellationRequested)
                    {
                        state = GeneratorState.Running;
                        GeneratorStateChanged?.Invoke(this, new GeneratorStateChangedEventArgs { State = GeneratorState.Running });
                    }

                    await Task.Run(async () => await job.Execute(this));

                    await Task.Delay(interval);

                }
            }
            catch (OperationCanceledException)
            {
                cancelTokenSource.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong!\n Details: {ex.Message}");
            }
        }

        private int RegisterChannels()
        {
            int numberOfChannels = 75;
            for (int i = 0; i < numberOfChannels; ++i)
            {
                ChannelAdded?.Invoke(this, new ChannelChangedEventArgs { ChannelId = i });
            }

            return numberOfChannels;
        }

        private void DeregisterChannels()
        {
            int numberOfChannels = 75;
            for (int i = 0; i < numberOfChannels; ++i)
            {
                ChannelRemoved?.Invoke(this, new ChannelChangedEventArgs { ChannelId = i });
            }
        }


    }
}
