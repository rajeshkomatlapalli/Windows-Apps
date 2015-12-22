using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideosCardGame
{
    public abstract class PropertyChangedMessageBase : MessageBase
    {
        protected PropertyChangedMessageBase(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        protected PropertyChangedMessageBase(object sender, string propertyName)
            : base(sender)
        {
            this.PropertyName = propertyName;
        }

        protected PropertyChangedMessageBase(object sender, object target, string propertyName)
            : base(sender, target)
        {
            this.PropertyName = propertyName;
        }

        public string PropertyName { get; protected set; }
    }
}
