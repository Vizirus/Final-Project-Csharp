using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FinalProject.Models;
using Microsoft.Win32;

namespace FinalProject
{
    /// <summary>
    /// Interaction logic for AddContextToPipes.xaml
    /// </summary>
    public partial class AddContextToPipes : Window
    {
        private ObservableCollection<FileViewModel> fileViewModels;
        private ObservableCollection<FileViewModel> fileViewModelsB;
        private List<string> searchedWordsList;
        private List<string> searchedWordsListB;
        private string defaultPath = Environment.ProcessPath;
        private MainWindow mainWindow;  
        public AddContextToPipes(MainWindow mainWindow)
        {
            InitializeComponent();
            defaultPath = String.Empty;
            searchedWordsList = new List<string>();
            fileViewModels = new ObservableCollection<FileViewModel>();
            fileViewModelsB = new ObservableCollection<FileViewModel>();
            searchedWordsListB = new List<string>();
            AgentAFileList.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = fileViewModels
            });
            AgentBFileList.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = fileViewModelsB
            });
            this.mainWindow = mainWindow;
        }
        private void UpdateList(string param)
        {
            var txtFiles = System.IO.Directory.GetFiles(defaultPath, "*.txt");
            if (txtFiles.Any())
            {
                var newItem = new FileViewModel();
                foreach (var file in txtFiles)
                {
                    newItem.FileName = System.IO.Path.GetFileName(file);
                    newItem.FilePath = file;
                    if(param == "A")
                    {
                        if(fileViewModels.Any(f => f.FileName == newItem.FileName))
                        {
                            MessageBox.Show($"File '{newItem.FileName}' already exists in the list.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue; // Skip adding this file if it already exists
                        }
                        fileViewModels.Add(newItem);
                    }
                        
                    else if (param == "B")
                    {
                        if (fileViewModelsB.Any(f => f.FileName == newItem.FileName))
                        {
                            MessageBox.Show($"File '{newItem.FileName}' already exists in the list.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue; // Skip adding this file if it already exists
                        }
                        fileViewModelsB.Add(newItem);
                    }
                    newItem = new FileViewModel(); // Reset for the next item
                }
            }
            else
            {
                MessageBox.Show("No text files found in the specified directory.", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            var button = sender as Button;
            var buttonTag = button?.Tag as string;
            if(buttonTag == "A")
            {
                if (string.IsNullOrWhiteSpace(AgentAWordToSearch.Text))
                {
                    MessageBox.Show("Please enter a file name to search.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if(!searchedWordsList.Contains(AgentAWordToSearch.Text))
                {
                    searchedWordsList.Add(AgentAWordToSearch.Text);
                    ResultBlock.Text = string.Join(", ", searchedWordsList);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(AgentBWordToSearch.Text))
                {
                    MessageBox.Show("Please enter a file name to search.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!searchedWordsListB.Contains(AgentBWordToSearch.Text))
                {
                    searchedWordsListB.Add(AgentBWordToSearch.Text);
                    ResultBlockB.Text = string.Join(", ", searchedWordsListB);
                }        
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Title = "Select Directory";
            openFolderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var button = sender as Button;
            var buttonTag = button?.Tag as string;
            if (openFolderDialog.ShowDialog() == true)
            {
                defaultPath = openFolderDialog.FolderName;
                UpdateList(buttonTag);
            }
            else
            {
                MessageBox.Show("No directory selected.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select Text File";
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var fileName in openFileDialog.FileNames)
                {
                    var newItem = new FileViewModel
                    {
                        FileName = System.IO.Path.GetFileName(fileName),
                        FilePath = fileName
                    };
                    var button = sender as Button;
                    var buttonTag = button?.Tag as string;
                    if (buttonTag == "A")
                    {
                        if (fileViewModels.Any(f => f.FileName == newItem.FileName))
                        {
                            MessageBox.Show($"File '{newItem.FileName}' already exists in the list.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue; // Skip adding this file if it already exists
                        }
                        fileViewModels.Add(newItem);
                    }

                    else if (buttonTag == "B")
                    {
                        if (fileViewModelsB.Any(f => f.FileName == newItem.FileName))
                        {
                            MessageBox.Show($"File '{newItem.FileName}' already exists in the list.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue; // Skip adding this file if it already exists
                        }
                        fileViewModelsB.Add(newItem);
                    }
                }
            }
            else
            {
                MessageBox.Show("No file selected.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var buttonTag = button?.Tag as string;
            if(buttonTag == "A")
                mainWindow.SetContext(buttonTag, searchedWordsList, fileViewModels.ToDictionary(f => f.FileName, f => f.FilePath));
            else if (buttonTag == "B")
                mainWindow.SetContext(buttonTag, searchedWordsListB, fileViewModelsB.ToDictionary(f => f.FileName, f => f.FilePath));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var buttonTag = button?.Tag as string;
            if(buttonTag == "A")
            {
                var SelectedItem = AgentAFileList.SelectedItem as FileViewModel;
                if (SelectedItem != null)
                {
                    fileViewModels.Remove(SelectedItem);
                }
                else
                {
                    MessageBox.Show("Please select a file to remove.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else if (buttonTag == "B")
            {
                var SelectedItem = AgentBFileList.SelectedItem as FileViewModel;
                if (SelectedItem != null)
                {
                    fileViewModelsB.Remove(SelectedItem);
                }
                else
                {
                    MessageBox.Show("Please select a file to remove.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var buttonTag = button?.Tag as string;
            if (buttonTag == "A")
            {
                if (searchedWordsList.Contains(AgentAWordToSearch.Text))
                {
                    searchedWordsList.Remove(AgentAWordToSearch.Text);
                    ResultBlock.Text = string.Join(", ", searchedWordsList);
                }
                else
                {
                    MessageBox.Show("Please select a word to remove.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else if (buttonTag == "B")
            {
                if (searchedWordsListB.Contains(AgentBWordToSearch.Text))
                {
                    searchedWordsListB.Remove(AgentBWordToSearch.Text);
                    ResultBlock.Text = string.Join(", ", searchedWordsListB);
                }
                else
                {
                    MessageBox.Show("Please select a word to remove.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var menuItem = mainWindow.Menu.Items[1] as MenuItem;
            menuItem.IsEnabled = true; // Re-enable the menu item when the window is closed
        }
    }
}
