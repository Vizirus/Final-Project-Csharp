using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AddContextToPipes addContextToPipes { get; set; }
        private List<string> searchedWordsListA;
        private Dictionary<string, string> filePathsDictionaryA;
        private List<string> searchedWordsListB;
        private Dictionary<string, string> filePathsDictionaryB;
        public MainWindow()
        {
            InitializeComponent();
            searchedWordsListA = new List<string>();
            filePathsDictionaryA = new Dictionary<string, string>();
            searchedWordsListB = new List<string>();
            filePathsDictionaryB = new Dictionary<string, string>();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            addContextToPipes = new AddContextToPipes(this);
            addContextToPipes.Show();
            var menu = sender as MenuItem;
            menu.IsEnabled = false; // Disable the menu item after opening the window
        }
        internal void SetContext(string contextChar, List<string> swL, Dictionary<string, string> keyValuePairs)
        {
            if(contextChar == "A")
            {
                
                if (swL == null || keyValuePairs == null || swL.Count == 0 || keyValuePairs.Count == 0)
                {
                    MessageBox.Show("Invalid context data provided.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                searchedWordsListA = swL;
                filePathsDictionaryA = keyValuePairs;
                MessageBox.Show("Context A has been set successfully.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (contextChar == "B")
            {
                if (swL == null || keyValuePairs == null || swL.Count == 0 || keyValuePairs.Count == 0)
                {
                    MessageBox.Show("Invalid context data provided.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                searchedWordsListB = swL;
                filePathsDictionaryB = keyValuePairs;
                MessageBox.Show("Context B has been set successfully.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            StringBuilder sb = new StringBuilder(); 
            var button = sender as Button;
            var buttonTag = button?.Tag as string;
            if(buttonTag == "A" && (searchedWordsListA.Count() == 0 || filePathsDictionaryA.Count() == 0))
            {
                MessageBox.Show("Contexts A are not set. Please set them before proceeding.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if(buttonTag == "B" &&(searchedWordsListB.Count() == 0 || filePathsDictionaryB.Count() == 0))
            {
                MessageBox.Show("Contexts B are not set. Please set them before proceeding.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                if(buttonTag == "A")
                {
                    var proccess = Process.Start(new ProcessStartInfo
                    {
                        FileName = "D:\\Artem\\Файли\\Унік\\2 semest\\OOP\\C#\\Final project\\MasterApplication\\FileAgent\\bin\\Debug\\net9.0\\FileAgent.exe",
                        CreateNoWindow = false,
                        UseShellExecute = true
                    });
                }
                else
                {
                    var proccess = Process.Start(new ProcessStartInfo
                    {
                        FileName = "D:\\Artem\\Файли\\Унік\\2 semest\\OOP\\C#\\Final project\\MasterApplication\\FileAgent_2\\bin\\Debug\\net9.0\\FileAgent_2.exe",
                        CreateNoWindow = false,
                        UseShellExecute = true
                    });
                }

                Task.Run(() =>
                    {
                        var pipeName = (buttonTag == "A") ? "FileAgentPipe" : "FileAgentPipeB";
                        sb.AppendLine("Agent have been launched.");
                        using var pipeClient = new NamedPipeServerStream(pipeName, PipeDirection.InOut);
                        sb.AppendLine("Waiting for connection...");
                        pipeClient.WaitForConnection();
                        sb.AppendLine("Connection established.");

                        using var reader = new StreamReader(pipeClient);
                        using var writer = new StreamWriter(pipeClient) { AutoFlush = true };


                        if (buttonTag == "A")
                        {
                            writer.WriteLine("w");
                            foreach (var i in searchedWordsListA)
                            {
                                writer.WriteLine(i);
                            }
                            writer.WriteLine("dc");
                            foreach (var kvp in filePathsDictionaryA)
                            {
                                writer.WriteLine($"{kvp.Key};{kvp.Value}");
                            }
                            writer.WriteLine("end");
                        }
                        else if (buttonTag == "B")
                        {
                            writer.WriteLine("w");
                            foreach (var i in searchedWordsListB)
                            {
                                writer.WriteLine(i);
                            }
                            writer.WriteLine("dc");
                            foreach (var kvp in filePathsDictionaryB)
                            {
                                writer.WriteLine($"{kvp.Key};{kvp.Value}");
                            }
                            writer.WriteLine("end");
                        }
                        string? readResult = reader?.ReadLine();
                        while (readResult != "exit")
                        {
                            if (readResult == "ok")
                            {
                                readResult = reader.ReadLine();
                                sb.AppendLine(readResult);
                            }
                            else if (readResult == "er")
                            {
                                sb.AppendLine("An error occurred while processing the request.");
                            }
                            readResult = reader?.ReadLine();
                        }
                        writer.Flush();
                        writer.Close();
                        reader.Close();
                        sb.AppendLine("Processing completed.");
                        pipeClient.Close();
                        Dispatcher.Invoke(() =>
                        {
                            if (buttonTag == "A")
                            {
                                AgentAName.Text = sb.ToString();
                            }
                            else if (buttonTag == "B")
                            {
                                AgentBName.Text = sb.ToString();
                            }
                        });
                    });
            }
        }
    }
}