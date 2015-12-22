using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideosCardGame
{
    public class GenericMessage_1<T> : MessageBase
    {
       public GenericMessage_1(T content)
        {
            this.Content = content;
        }

        public GenericMessage_1(object sender, T content)
            : base(sender)
        {
            this.Content = content;
        }

        public GenericMessage_1(object sender, object target, T content)
            : base(sender, target)
        {
            this.Content = content;
        }

        public T Content
        {
            [CompilerGenerated]
            get
            {
                return this.Content;
            }
            [CompilerGenerated]
            protected set
            {
                this.Content = value;
            }
        }
    }
}
