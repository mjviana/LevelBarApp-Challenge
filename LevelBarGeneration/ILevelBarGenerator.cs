// <copyright file="LevelBarGenerator.cs" company="VIBES.technology">
// Copyright (c) VIBES.technology. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;

namespace LevelBarGeneration
{
    public interface ILevelBarGenerator
    {
        event EventHandler<ChannelChangedEventArgs> ChannelAdded;
        event EventHandler<ChannelDataEventArgs> ChannelLevelDataReceived;
        event EventHandler<ChannelChangedEventArgs> ChannelRemoved;
        event EventHandler<GeneratorStateChangedEventArgs> GeneratorStateChanged;

        void ReceiveLevelData(int[] channelIds, float[] levels);

        Task Connect();
        Task Disconnect();
    }
}