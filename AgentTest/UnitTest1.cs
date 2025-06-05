using System;
using Agent;
using Xunit;
namespace AgentTest
{
    public class UnitTest1
    {
        private void createTheFile(string filename)
        {
            using (var writer = new System.IO.StreamWriter(filename))
            {
                writer.WriteLine("This is a test file for the DefaultAgent.");
                writer.WriteLine("It contains some sample test test text to be processed by the agent.");
                writer.Close();
            }
        }
        [Fact]
        public async void Test1()
        {
            createTheFile("testFile.txt");
            DefaultAgent agent = new DefaultAgent("testFile.txt");
            await agent.ParseFileAsync();

            //Try to find all 'text' word in the given test
            agent.CalculateWordFrequency("text");
            var result = agent.GetContentByToken("text");

            Assert.Equal(1, result.Item2);
        }
        [Fact]
        public async void TestOneMoreFile()
        {
            createTheFile("testFile2.txt");
            DefaultAgent agent = new DefaultAgent("testFile2.txt");
            await agent.ParseFileAsync();

            //Try to find all 'test' word in the given test
            agent.CalculateWordFrequency("test");
            var result = agent.GetContentByToken("test");

            Assert.Equal(3, result.Item2);
        }
        [Fact]
        public async void DynamicChange()
        {
            DefaultAgent agent = new DefaultAgent("testFile.txt");
            await agent.ParseFileAsync();

            //Try to find all 'text' word in the given test
            agent.CalculateWordFrequency("text");
            var result = agent.GetContentByToken("text");

            Assert.Equal(1, result.Item2);

            //Try to find 'test' word in a second file
            agent.ChangeFilePath("testFile2.txt");
            await agent.ParseFileAsync();
            agent.CalculateWordFrequency("test");

            result = agent.GetContentByToken("test");
            Assert.Equal(3, result.Item2);
        }
        [Fact]
        public async void ReadFromLorenIpsum()
        {
            DefaultAgent agent = new DefaultAgent("lorenI.txt");
            await agent.ParseFileAsync();
            agent.CalculateWordFrequency("Lorem");
            var result = agent.GetContentByToken("Lorem");

            Assert.Equal(1, result.Item2);
        }
        [Fact]
        public async void ReadZero()
        {
            DefaultAgent agent = new DefaultAgent("lorenI.txt");
            await agent.ParseFileAsync();
            agent.CalculateWordFrequency("UnknowWordWhich");
            var result = agent.GetContentByToken("UnknowWordWhich");

            Assert.Equal(0, result.Item2);
        }
        [Fact]
        public async void AccessWrongToken()
        {
            DefaultAgent agent = new DefaultAgent("lorenI.txt");
            await agent.ParseFileAsync();
            agent.CalculateWordFrequency("Lorem");
            var result = agent.GetContentByToken("UnknowWordWhich");

            Assert.Equal(0, result.Item2);
            Assert.Equal(string.Empty, result.Item1);
        }
    }
}
