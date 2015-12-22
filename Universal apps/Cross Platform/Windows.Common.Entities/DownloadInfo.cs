using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
    public class DownloadInfo
    {
        long m_ShowID;
        public long FolderID
        {
            get { return m_ShowID; }
            set { m_ShowID = value; }
        }

        string m_LinkUrl;
        public string LinkUrl
        {
            get { return m_LinkUrl; }
            set { m_LinkUrl = value; }
        }
        string m_thumb;
        public string ThumbNail
        {
            get { return m_thumb; }
            set { m_thumb = value; }
        }
        string m_Status;
        public string DownloadStatus
        {
            get { return m_Status; }
            set { m_Status = value; }
        }

        string m_title;
        public string title
        {
            get { return m_title; }
            set { m_title = value; }
        }
        string m_id;
        public string id
        {
            get { return m_id; }
            set { m_id = value; }
        }
        string m_DownLoadID;
        public string DownLoadID 
        {
            get { return m_DownLoadID; }
            set { m_DownLoadID = value; }
        }
    }
}
