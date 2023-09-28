using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            generatorInstance.GeneratorStateChanged += GeneratorInstance_GeneratorStateChangedToRunning;

            var connectAction = new Action(async () => await generatorInstance.Connect());

            connectAction.Invoke();

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
    }
}