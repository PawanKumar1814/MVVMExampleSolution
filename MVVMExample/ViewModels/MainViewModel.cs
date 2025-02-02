﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace MVVMExample.ViewModels
{
    ///<summary>
    /// Defines a service for logging messages.
    ///</summary>
    public interface ILogService
    {
        ///<summary>
        /// Logs a message to a specific destination.
        ///</summary>
        ///<param name="message">The message to log.</param>
        void LogMessage(string message);
    }

    ///<summary>
    /// Implementation of ILogService that logs messages to a store or console.
    ///</summary>
    public class LogService : ILogService
    {
        ///<summary>
        /// Logs a message to the console or a log store.
        ///</summary>
        ///<param name="message">The message to log.</param>
        public void LogMessage(string message)
        {
            // Implementation for logging to a store or console
        }
    }

    ///<summary>
    /// The ViewModel that serves as the data context for the MainWindow. It provides properties
    /// and commands that the view binds to and communicates changes via the INotifyPropertyChanged interface.
    ///</summary>
    ///<summary>
    /// ViewModel part of the MVVM architecture that manages application data, logs, and notifications.
    ///</summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _logDetails = "";
        private string _notificationMessage = "";
        private bool _notificationVisible = false;
        private readonly ILogService _logService;
        private DispatcherTimer _timer; // Timer to auto-hide notifications

        ///<summary>
        /// Initializes a new instance of the MainViewModel class with a specified log service.
        ///</summary>
        ///<param name="logService">The log service used for logging messages.</param>
        public MainViewModel(ILogService logService)
        {
            _logService = logService;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) }; // Notification shows for 5 seconds
            _timer.Tick += (sender, e) => { HideNotification(); };
        }

        ///<summary>
        /// Gets or sets the log details, which are displayed in the UI and updated upon changes.
        ///</summary>
        public string LogDetails
        {
            get => _logDetails;
            set
            {
                _logDetails = value;
                OnPropertyChanged();
            }
        }

        ///<summary>
        /// Gets or sets the message for the notification popup.
        ///</summary>
        public string NotificationMessage
        {
            get => _notificationMessage;
            set
            {
                _notificationMessage = value;
                OnPropertyChanged();
            }
        }

        ///<summary>
        /// Gets or sets the visibility of the notification popup.
        ///</summary>
        public bool NotificationVisible
        {
            get => _notificationVisible;
            set
            {
                _notificationVisible = value;
                OnPropertyChanged();
            }
        }

        ///<summary>
        /// Occurs when a property value changes.
        ///</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        ///<summary>
        /// Notifies listeners about property changes.
        ///</summary>
        ///<param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        ///<summary>
        /// Appends a message to the log details and logs it using the external log service.
        ///</summary>
        ///<param name="message">The message to log.</param>
        public void UpdateLogDetails(string message)
        {
            LogDetails += "\n" + message;
            _logService.LogMessage(message);
        }

        ///<summary>
        /// Displays a notification with a specified message.
        ///</summary>
        ///<param name="message">The message to display in the notification.</param>
        public void ShowNotification(string message)
        {
            NotificationMessage = message;
            NotificationVisible = true;
            _timer.Start();
        }

        ///<summary>
        /// Hides the notification popup and stops the auto-hide timer.
        ///</summary>
        private void HideNotification()
        {
            NotificationVisible = false;
            _timer.Stop();
        }
    }
}