using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideosCardGame
{
    public interface IMessenger
    {
        void Register<TMessage>(object recipient, Action<TMessage> action);
        void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Action<TMessage> action);
        void Send<TMessage>(TMessage message);
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This syntax is more convenient than other alternatives.")]
        void Send<TMessage, TTarget>(TMessage message);
        void Unregister(object recipient);
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This syntax is more convenient than other alternatives.")]
        void Unregister<TMessage>(object recipient);
        void Unregister<TMessage>(object recipient, Action<TMessage> action);
    }
}
