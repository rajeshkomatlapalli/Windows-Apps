using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace OnlineVideosCardGame
{
    public abstract class ViewModelBase : INotifyPropertyChanged, ICleanup, IDisposable
    {
        private static bool? _isInDesignMode;

        public event PropertyChangedEventHandler PropertyChanged;

        protected ViewModelBase()
            : this(null)
        {
        }

        protected ViewModelBase(IMessenger messenger)
        {
            this.MessengerInstance = messenger;
        }

        protected virtual void Broadcast<T>(T oldValue, T newValue, string propertyName)
        {
            PropertyChangedMessage_1<T> message = new PropertyChangedMessage_1<T>(this, oldValue, newValue, propertyName);
            if (this.MessengerInstance != null)
            {
                this.MessengerInstance.Send<PropertyChangedMessage_1<T>>(message);
            }
            else
            {
                Messenger.Default.Send<PropertyChangedMessage_1<T>>(message);
            }
        }

        public virtual void Cleanup()
        {
            Messenger.Default.Unregister(this);
        }

        public void Dispose()
        {
            //this.Dispose(true);
        }

        [Obsolete("This interface will be removed from ViewModelBase in a future version, use ICleanup.Cleanup instead.")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Cleanup();
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "This cannot be an event")]
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "This cannot be an event")]
        protected virtual void RaisePropertyChanged<T>(string propertyName, T oldValue, T newValue, bool broadcast)
        {
            this.RaisePropertyChanged(propertyName);
            if (broadcast)
            {
                this.Broadcast<T>(oldValue, newValue, propertyName);
            }
        }

        [Conditional("DEBUG"), DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            //if (base.GetType().GetProperty(propertyName) == null)
            if (base.GetType().GetRuntimeProperty(propertyName) == null)
            {
                throw new ArgumentException("Property not found", propertyName);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Non static member needed for data binding")]
        public bool IsInDesignMode
        {
            get
            {
                return IsInDesignModeStatic;
            }
        }

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "The security risk here is neglectible.")]
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
                    //_isInDesignMode = new bool?(DesignerProperties.IsInDesignTool);
                    _isInDesignMode = new bool?(Windows.ApplicationModel.DesignMode.DesignModeEnabled);
                }
                return _isInDesignMode.Value;
            }
        }

        protected IMessenger MessengerInstance { get; set; }
    }
}
