using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace LevelBarGeneration.Tests
{
    [TestClass()]
    public class LevelBarGeneratorTests
    {
        private bool dataReceived;

        [TestMethod()]
        public async Task ReceiveDataTest()
        {
            var generatorInstance = new LevelBarGenerator();

            generatorInstance.ChannelLevelDataReceived += GeneratorInstance_ChannelLevelDataReceived;

            var connectAction = new Action(async () => await generatorInstance.Connect());

            connectAction.Invoke();

            Assert.IsTrue(dataReceived);
        }

        private void GeneratorInstance_ChannelLevelDataReceived(object sender, ChannelDataEventArgs e)
        {
            dataReceived = e.Levels.Length > 0 && e.ChannelIds.Length > 0;
        }
    }
}