using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideosCardGame
{
   public class PropertyChangedMessage_1<T> : PropertyChangedMessageBase
    {
        public PropertyChangedMessage_1(T oldValue, T newValue, string propertyName)
            : base(propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public PropertyChangedMessage_1(object sender, T oldValue, T newValue, string propertyName)
            : base(sender, propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public PropertyChangedMessage_1(object sender, object target, T oldValue, T newValue, string propertyName)
            : base(sender, target, propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public T NewValue
        {

            get
            {
                return this.NewValue;
            }

            private set
            {
                this.NewValue = value;
            }
        }

        public T OldValue
        {
            get
            {
                return this.OldValue;
            }

            private set
            {
                this.OldValue = value;
            }
        }
    }
}
