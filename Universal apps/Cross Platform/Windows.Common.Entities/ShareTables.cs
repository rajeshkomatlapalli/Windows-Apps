using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
  [SQLite.Table("ShareTables")]
    public class ShareTables
    {
         [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID
        {
            get;
            set;
        }
         [SQLite.Column("TableName")]
        public string TableName
        {
            get;
            set;
        }

         [SQLite.Column("Type")]
        public string Type
        {
            get;
            set;
        }

      [SQLite.Column("WhereCondition")]
        public string WhereCondition
        {
            get;
            set;
        }
        [SQLite.Column("TableOrder")]
        public int TableOrder
        {
            get;
            set;
        }
       
          [SQLite.Column("DependentTable")]
        public string DependentTable
        {
            get;
            set;
        }
         [SQLite.Column("DependentWhereCondition")]
        public string DependentWhereCondition
        {
            get;
            set;
        }
    }
}
