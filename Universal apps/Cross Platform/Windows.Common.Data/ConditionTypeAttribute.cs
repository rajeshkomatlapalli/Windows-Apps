using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;

namespace OnlineVideos.Data
{
    public class ConditionTypeAttribute : Attribute
    {
        public ConditionTypeAttribute(string coloumnname,string condition)
            {
                theCondition.Add(condition,coloumnname);
            }

           
        protected Dictionary<string,string> theCondition=new Dictionary<string,string>();


        public Dictionary<string, string> Condition
            {
                get { return theCondition; }
                set { theCondition = value; }
            }

    }
}
