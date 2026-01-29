using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace NotePad
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        string NotYolu;
        bool IsCreateContentVisible = false;
        private DispatcherTimer _autoSaveTimer;
        private bool _isLoadingNote = false;
        private string _lastSavedContent = "";



        public MainWindow()
        {
            InitializeComponent();

            // AppData\Roaming\Notepad
            string appDataPath = Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData);

            NotYolu = System.IO.Path.Combine(appDataPath, "NotePad", "Notes");

            // Klasör yoksa oluştur
            Directory.CreateDirectory(NotYolu);

            ShowNotes();
            InitializeAutoSaveTimer();


        }
        private void InitializeAutoSaveTimer()
        {
            _autoSaveTimer = new DispatcherTimer();
            _autoSaveTimer.Interval = TimeSpan.FromSeconds(2);
            _autoSaveTimer.Tick += AutoSaveTimer_Tick;
        }
        private void TextArea1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isLoadingNote)
                return;

            if (NoteList.SelectedItem == null)
                return;

            SetStatus(
                "Changes detected, will be saved...",
                Brushes.Orange,
                "●",
                "Autosave pending");

            _autoSaveTimer.Stop();   
            _autoSaveTimer.Start(); 
        }
        private async void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            _autoSaveTimer.Stop(); 
            SetStatus(
                "Autosaving...",
                Brushes.DodgerBlue,
                "●",
                "Autosave in progress");
            await SaveMethod();
        }
        public void ShowNotes()
        {
            NoteList.Items.Clear();
            string[] Files = Directory.GetFiles(NotYolu,"*.txt");
            foreach (string File in Files)
            {
                NoteList.Items.Add(
                    System.IO.Path.GetFileNameWithoutExtension(File)
                );
            }
        }
        private string GetFilePathFromNoteName(string noteName)
        {
            return System.IO.Path.Combine(NotYolu, noteName + ".txt");
        }

        private void ShowCreateContent_Click(object sender, RoutedEventArgs e)
        {
            ChangeCreateContentVisibility();
        }

        private void ChangeCreateContentVisibility()
        {
            if (IsCreateContentVisible)
            {
                CreateButton.Visibility = Visibility.Hidden;
                EntryNotePadNameBorder.Visibility = Visibility.Hidden;
                IsCreateContentVisible = false;
            }
            else
            {
                CreateButton.Visibility = Visibility.Visible;
                EntryNotePadNameBorder.Visibility = Visibility.Visible;
                IsCreateContentVisible = true;
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string Txt_Isim = NotePadNameCreate.Text.Trim();
            string dosyaYolu = System.IO.Path.Combine(NotYolu, Txt_Isim + ".txt");

            if (string.IsNullOrWhiteSpace(Txt_Isim))
            {
                MessageBox.Show(
                    "Note name cannot be empty.",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(dosyaYolu))
                {
                    writer.WriteLine("");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            ShowNotes();
            ChangeCreateContentVisibility();
        }

        private void NoteList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NoteList.SelectedItem == null)
                return;

            string NoteName = NoteList.SelectedItem.ToString();

            _isLoadingNote = true;          
            _autoSaveTimer.Stop();          

            TextArea1.Document.Blocks.Clear();

            try
            {
                using (StreamReader sr = new StreamReader(GetFilePathFromNoteName(NoteName)))
                {
                    TextArea1.AppendText(sr.ReadToEnd());
                }

                _lastSavedContent = ReadContent();

                SetFileInfo(NoteName);
                SetStatus("Note loaded", Brushes.LimeGreen, "✔");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                SetStatus("Failed to load note", Brushes.Red, "✖");
            }

            _isLoadingNote = false;         
        }

        public string ReadContent()
        {
            TextRange range = new TextRange(TextArea1.Document.ContentStart, TextArea1.Document.ContentEnd);
            return range.Text;
        }

        private async void SaveNote_Click(object sender, RoutedEventArgs e)
        {
            await SaveMethod();

        }
        public async Task SaveMethod()
        {
            if (NoteList.SelectedItem == null)
                return;

            string NoteName = NoteList.SelectedItem.ToString();
            string Content = ReadContent();

            if (Content == _lastSavedContent)
            {
                SetStatus(
                    "No changes",
                    Brushes.Gray,
                    "●",
                    "Autosave disabled");

                return;
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(GetFilePathFromNoteName(NoteName)))
                {
                    sw.Write(Content);
                }

                _lastSavedContent = Content;

                SetStatus(
                    "Saved",
                    Brushes.LimeGreen,
                    "✔",
                    "Autosave completed");
            }
            catch
            {
                SetStatus(
                    "Save Error!",
                    Brushes.Red,
                    "✖",
                    "Autosave disabled");
            }

            
            var saveButtonTemplate = SaveNote.Template;
            var iconTextBlock = (TextBlock)saveButtonTemplate.FindName("saveicon", SaveNote);
            if (iconTextBlock != null)
            {
                iconTextBlock.Text = "";
                await Task.Delay(250);
                iconTextBlock.Text = "\uE105";
            }
        }
        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            _autoSaveTimer.Stop();
            if (NoteList.SelectedItem == null)
                return;

            string NoteName = NoteList.SelectedItem.ToString();
            string DosyaYolu = GetFilePathFromNoteName(NoteName);

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete the note named \"{NoteName}\" ?",
                "Deletion Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                File.Delete(DosyaYolu);
                ShowNotes();
                TextArea1.Document.Blocks.Clear();
                SetFileInfo("");
                SetStatus("Note deleted", Brushes.OrangeRed, "✖", "Autosave disabled");

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error occurred while deleting the note.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }

        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string a = "";
            try
            {
                a = SearchBar.Text;
                for(int i =0; i < NoteList.Items.Count; i++)
                {
                    if (NoteList.Items[i].ToString().Contains(a))
                    {
                        NoteList.SelectedItem = NoteList.Items[i];break;
                    }
                }
            }
            catch (Exception ex) { }
        }
        private void SetStatus(
            string text,
            Brush color,
            string icon = "●",
            string autoSaveText = "Autosave enabled")
        {
            StatusText.Text = text;
            StatusIcon.Text = icon;
            StatusIcon.Foreground = color;
            AutoSaveText.Text = autoSaveText;
        }

        private void SetFileInfo(string noteName)
        {
            FileInfoText.Text = string.IsNullOrEmpty(noteName)
                ? "Note not selected"
                : noteName;
        }
    }
}
