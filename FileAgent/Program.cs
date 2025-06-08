using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Reflection.Metadata;
using System.Text;
using Agent;
namespace FileAgent
{
    internal class Program
    {
        
        static async Task Main(string[] args)
        {
            Process process = Process.GetCurrentProcess();
            process.ProcessorAffinity = (IntPtr)(1 << 2);
            StringBuilder stringBuilder = new StringBuilder();
            List<string> searchedWordsListA = new List<string>();
            Dictionary<string, string> filePathsDictionaryA = new Dictionary<string, string>();
            Console.WriteLine("Welcome to file agent A application manger! ");
            DefaultAgent agent = new DefaultAgent();
            await Task.Run(async () =>
            {
                var pipeClient = new NamedPipeClientStream(".", "FileAgentPipe", PipeDirection.InOut);
                Console.WriteLine("Connecting to the pipe...");
                await pipeClient.ConnectAsync();
                Console.WriteLine("Connected to the pipe.");
                using var reader = new StreamReader(pipeClient) ;
                using var writer = new StreamWriter(pipeClient) { AutoFlush = true };
                string? readedResult = reader.ReadLine();
                if (readedResult == "w")
                {
                    readedResult = reader.ReadLine();
                    while (readedResult != "dc")
                    {
                        if (!string.IsNullOrEmpty(readedResult))
                        {
                            searchedWordsListA.Add(readedResult);
                        }
                        readedResult = reader.ReadLine();
                    }
                    Console.WriteLine("Words to search: " + string.Join(", ", searchedWordsListA));
                    string[] splittedResult = new string[2];
                    readedResult = reader.ReadLine();
                    while (readedResult != "end")
                    {
                        splittedResult = readedResult.Split(';');
                        filePathsDictionaryA.Add(splittedResult[0], splittedResult[1]);
                        readedResult = reader.ReadLine();
                    }
                    Console.WriteLine("File paths and their descriptions: ");

                    //The actual parsing
                    foreach (var kvp in filePathsDictionaryA)
                    {
                        try
                        {
                            agent.ChangeFilePath(kvp.Value);
                            await agent.ParseFileAsync();
                            foreach (var word in searchedWordsListA)
                            {
                                agent.CalculateWordFrequency(word);
                                int countInFile = agent.ReturnCount(word);
                                var content = agent.GetContentByToken(word);
                                writer.WriteLine("ok");
                                writer.WriteLine($"File: {kvp.Key}, Word: {content.Item1}, Count: {countInFile}. Sum count for the word: {content.Item2}\n");
                            }
                            Console.WriteLine("Data has been sent to the master application successfully!");
                        }
                        catch (Exception ex)
                        {
                            writer.WriteLine("er");
                            Console.WriteLine("Error Occured while running the application! " + ex.Message);
                        }
                    }
                    writer.WriteLine("exit");
                    writer.Flush();
                    writer.Close();
                    reader.Close();
                    pipeClient.Close();
                }
                
                Console.WriteLine("Process finished.");
            });
        }
    }
}
