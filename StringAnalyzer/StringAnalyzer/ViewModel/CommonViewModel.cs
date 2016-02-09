using System;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using StringAnalyzer.WindowFactories;

namespace StringAnalyzer.ViewModel
{
    public class CommonViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public CommonViewModel()
        {
            this.OpenTextfileCommand = new RelayCommand(this.OnExecuteOpenTextfileCommand);
            this.SaveTextfileCommand = new RelayCommand(this.OnExecuteSaveTextfileCommand);
            this.AboutCommand = new RelayCommand(this.OnExecuteAboutCommand);

            if (this.IsInDesignMode)
            {
                this.Text = "CATTATTAGGA";
            }
            else
            {
                // Code runs "for real"
            }
        }

        #region Fields

        private string _text;
        private bool _ignoreLinebreaks = true;
        private bool _ignoreTabs = true;
        private bool _ignoreSpaces;

        #endregion

        #region Properties

        public string Text
        {
            get { return this._text; }
            set
            {
                if (this._text == value) return;

                this._text = value;
                this.RaisePropertyChanged();
            }
        }
        public bool IgnoreLinebreaks
        {
            get { return this._ignoreLinebreaks; }
            set
            {
                if (this._ignoreLinebreaks == value) return;

                this._ignoreLinebreaks = value;
                this.RaisePropertyChanged();
            }
        }
        public bool IgnoreTabs
        {
            get { return this._ignoreTabs; }
            set
            {
                if (this._ignoreTabs == value) return;

                this._ignoreTabs = value;
                this.RaisePropertyChanged();
            }
        }
        public bool IgnoreSpaces
        {
            get { return this._ignoreSpaces; }
            set
            {
                if (this._ignoreSpaces == value) return;

                this._ignoreSpaces = value;
                this.RaisePropertyChanged();
            }
        }

        #region Commands

        public ICommand OpenTextfileCommand { get; }
        public ICommand SaveTextfileCommand { get; }
        public ICommand AboutCommand { get; }

        #endregion

        #endregion

        private void OnExecuteOpenTextfileCommand()
        {
            var instance = this.OpenFileDialogFactory.GetInstance();
            instance.Filter = "Textdateien (*.txt)|*.txt|Alle Dateien|*.*";
            if (instance.ShowDialog() != true) return;

            try
            {
                this.Text = File.ReadAllText(instance.FileName);
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException || ex is SecurityException)
            {
                this.MessageBoxProvider.Show("Fehler", "Beim �ffnen der Textdatei ist ein Fehler aufgetreten.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnExecuteSaveTextfileCommand()
        {
            var instance = this.SaveFileDialogFactory.GetInstance();
            instance.Filter = "Textdateien (*.txt)|*.txt|Alle Dateien|*.*";
            if (instance.ShowDialog() != true) return;

            try
            {
                File.WriteAllText(instance.FileName, this.Text);
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException || ex is SecurityException)
            {
                this.MessageBoxProvider.Show("Fehler", "Beim speichern der Textdatei ist ein Fehler aufgetreten.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnExecuteAboutCommand()
        {
            var instance = this.AboutWindowFactory.GetInstance();
            instance.ShowDialog();
        }

        public IModalDialogFactory<IModalDialog<bool?>, bool?> AboutWindowFactory { get; set; }
        public IModalFileDialogFactory SaveFileDialogFactory { get; set; }
        public IModalFileDialogFactory OpenFileDialogFactory { get; set; }
        public IMessageBoxProvider MessageBoxProvider { get; set; }
    }
}