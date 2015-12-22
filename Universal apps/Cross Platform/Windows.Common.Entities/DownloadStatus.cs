using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
    public  class DownloadStatus : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }


        #endregion
        private int _StatusId;
        [SQLite.Ignore]
        public int StatusId 
        {
            get { return _StatusId; }
            set { _StatusId = value; }
        }
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        private string _Thumb;
        public string ThumbNail
        {
            get { return _Thumb; }
            set { _Thumb = value; }
        }
        private string _TransferStatus;
        [SQLite.Ignore]
        public string TransferStatus
        {
            get { return _TransferStatus; }
            set { _TransferStatus = value; }
        }
        private double _BytesRecieved;
        [SQLite.Ignore]
        public double BytesRecieved
        {
            get { return this._BytesRecieved; }

            set
            {
                if (value != this._BytesRecieved)
                {
                    this._BytesRecieved = value;
                    RaisePropertyChanged();
                }
            }
        }
        private string _TotalBytesToRecieve;
        [SQLite.Ignore]
        public string TotalBytesToRecieve
        {
            get { return this._TotalBytesToRecieve; }

            set
            {
                if (value != this._TotalBytesToRecieve)
                {
                    this._TotalBytesToRecieve = value;
                    RaisePropertyChanged();
                }
            }
        }
        private double _ChapterProgressPosition;
        public double ChapterProgressPosition
        {
            get { return _ChapterProgressPosition; }

            set
            {
                if (value !=_ChapterProgressPosition)
                {
                    _ChapterProgressPosition = value;
                    RaisePropertyChanged();
                }
            }
        }
        private string _RequestUri;
        public string RequestUri
        {
            get { return _RequestUri; }
            set { _RequestUri = value; }
        }
        private string _FolderStatus;
        [SQLite.Ignore]
        public string FolderStatus
        {
            get { return _FolderStatus; }
            set { _FolderStatus = value; }
        }
        private string _FileSize;
        public string FileSize
        {
            get { return _FileSize; }
            set { _FileSize = value; }
        }
        private string _FolderName;
        public string FolderName
        {
            get { return _FolderName; }
            set { _FolderName = value; }
        }

        private string _DownStatus;
        [SQLite.Ignore]
        public string Downsc
        {
            get { return _DownStatus; }
            set { _DownStatus = value; }
        }
    }
}
