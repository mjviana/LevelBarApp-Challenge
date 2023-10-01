using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace LevelBarGeneration.Tests
{
    [TestClass()]
    public class LevelBarGeneratorTests
    {
        private bool dataReceived;
        private GeneratorState stateForConnectCase;
        private GeneratorState stateForDisconnectCase;

        [TestMethod()]
        public async Task ConnectTest()
        {
            var generatorInstance = new LevelBarGenerator();

            generatorInstance.GeneratorStateChanged += GeneratorInstance_GeneratorStateChangedToRunning;

            var connectAction = new Action(async () => await generatorInstance.Connect());

            connectAction.Invoke();

            Assert.IsTrue(stateForConnectCase == GeneratorState.Running);
        }

        [TestMethod()]
        public async Task DisconnectTest()
        {
            var generatorInstance = new LevelBarGenerator();

            generatorInstance.GeneratorStateChanged += GeneratorInstance_GeneratorStateChangedToStopped;

            var connectAction = new Action(async () => await generatorInstance.Connect());
            var disconnectAction = new Action(async () => await generatorInstance.Disconnect());

            connectAction.Invoke();
            disconnectAction.Invoke();

            Assert.IsTrue(stateForDisconnectCase == GeneratorState.Stopped);
        }

        [TestMethod()]
        public async Task ReceiveDataTest()
        {
            var generatorInstance = new LevelBarGenerator();

            generatorInstance.ChannelLevelDataReceived += GeneratorInstance_ChannelLevelDataReceived;

            var connectAction = new Action(async () => await generatorInstance.Connect());
            var disconnectAction = new Action(async () => await generatorInstance.Disconnect());

            connectAction.Invoke();
            disconnectAction.Invoke();

            Assert.IsTrue(dataReceived);
        }

        private void GeneratorInstance_GeneratorStateChangedToRunning(object sender, GeneratorStateChangedEventArgs e)
        {
            stateForConnectCase = e.State;
        }

        private void GeneratorInstance_GeneratorStateChangedToStopped(object sender, GeneratorStateChangedEventArgs e)
        {
            stateForDisconnectCase = e.State;
        }

        private void GeneratorInstance_ChannelLevelDataReceived(object sender, ChannelDataEventArgs e)
        {
            dataReceived = e.Levels.Length > 0 && e.ChannelIds.Length > 0;
        }

        #region Mock Generator

        [TestMethod]
        public void GeneratorStateChanged_Event_Should_Be_Raised_With_Running_State()
        {
            // Arrange
            var mockGenerator = new Mock<ILevelBarGenerator>();
            GeneratorStateChangedEventArgs capturedEventArgs = null;
            bool eventRaised = false;

            // Act
            mockGenerator.Object.GeneratorStateChanged += (sender, args) =>
            {
                capturedEventArgs = args;
                eventRaised = true;

            };

            mockGenerator.Raise(g => g.GeneratorStateChanged += null, new GeneratorStateChangedEventArgs { State = GeneratorState.Running });

            // Assert

            Assert.IsNotNull(capturedEventArgs);
            Assert.IsTrue(GeneratorState.Running == capturedEventArgs.State);
        }


        [TestMethod]
        public void GeneratorStateChanged_Event_Should_Be_Raised_With_Stopped_State()
        {
            // Arrange
            var mockGenerator = new Mock<ILevelBarGenerator>();
            GeneratorStateChangedEventArgs capturedEventArgs = null;
            bool eventRaised = false;
            int numberOfRaises = 0;

            // Act
            mockGenerator.Object.GeneratorStateChanged += (sender, args) =>
            {
                capturedEventArgs = args;
                eventRaised = true;
                numberOfRaises++;

            };

            // First we connect
            mockGenerator.Raise(g => g.GeneratorStateChanged += null, new GeneratorStateChangedEventArgs { State = GeneratorState.Running });

            // Then we disconnect
            mockGenerator.Raise(g => g.GeneratorStateChanged += null, new GeneratorStateChangedEventArgs { State = GeneratorState.Stopped });

            // Assert
            Assert.IsNotNull(capturedEventArgs);
            Assert.IsTrue(GeneratorState.Stopped == capturedEventArgs.State);
            Assert.IsTrue(numberOfRaises == 2);
        }

        [TestMethod]
        public void ChannelLevelDataReceived_Event_Should_Be_Raised()
        {

            // Arrange
            var mockGenerator = new Mock<ILevelBarGenerator>();
            ChannelDataEventArgs capturedEventArgs = null;
            bool eventRaised = false;
            var channelIds = new int[] { 1, 2, 3 };
            var levels = new float[] { 0.5f, 0.6f, 0.7f };

            // Act
            mockGenerator.Object.ChannelLevelDataReceived += (sender, args) =>
            {
                capturedEventArgs = args;
                eventRaised = true;
            };

            mockGenerator.Raise(g => g.ChannelLevelDataReceived += null, new ChannelDataEventArgs() { ChannelIds = channelIds, Levels = levels });


            // Assert
            Assert.IsNotNull(capturedEventArgs);
            Assert.IsTrue(eventRaised);
        }
        #endregion

    }
}