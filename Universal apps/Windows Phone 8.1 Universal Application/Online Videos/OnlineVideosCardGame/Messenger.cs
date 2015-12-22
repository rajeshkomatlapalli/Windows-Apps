using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace OnlineVideosCardGame
{
    public class Messenger : IMessenger
    {
        private static Messenger _defaultInstance;
        private Dictionary<Type, List<WeakActionAndToken>> _recipientsOfSubclassesAction;
        private Dictionary<Type, List<WeakActionAndToken>> _recipientsStrictAction;

        private void Cleanup()
        {
            CleanupList(this._recipientsOfSubclassesAction);
            CleanupList(this._recipientsStrictAction);
        }

        private static void CleanupList(IDictionary<Type, List<WeakActionAndToken>> lists)
        {
            if (lists != null)
            {
                List<Type> list = new List<Type>();
                foreach (KeyValuePair<Type, List<WeakActionAndToken>> pair in lists)
                {
                    List<WeakActionAndToken> list2 = new List<WeakActionAndToken>();
                    foreach (WeakActionAndToken token in pair.Value)
                    {
                        if (!((token.Action != null) && token.Action.IsAlive))
                        {
                            list2.Add(token);
                        }
                    }
                    foreach (WeakActionAndToken token2 in list2)
                    {
                        pair.Value.Remove(token2);
                    }
                    if (pair.Value.Count == 0)
                    {
                        list.Add(pair.Key);
                    }
                }
                foreach (Type type in list)
                {
                    lists.Remove(type);
                }
            }
        }

        private static bool Implements(Type instanceType, Type interfaceType)
        {
            if ((interfaceType != null) && (instanceType != null))
            {
                IEnumerable<Type> interfaces = instanceType.GetType().GetTypeInfo().ImplementedInterfaces;
                foreach (Type type in interfaces)
                {
                    if (type == interfaceType)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void OverrideDefault(Messenger newMessenger)
        {
            _defaultInstance = newMessenger;
        }

        public virtual void Register<TMessage>(object recipient, Action<TMessage> action)
        {
            this.Register<TMessage>(recipient, null, false, action);
        }

        public virtual void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Action<TMessage> action)
        {
            this.Register<TMessage>(recipient, null, receiveDerivedMessagesToo, action);
        }

        public virtual void Register<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            this.Register<TMessage>(recipient, token, false, action);
        }

        public virtual void Register<TMessage>(object recipient, object token, bool receiveDerivedMessagesToo, Action<TMessage> action)
        {
            Dictionary<Type, List<WeakActionAndToken>> dictionary;
            List<WeakActionAndToken> list;
            Type key = typeof(TMessage);
            if (receiveDerivedMessagesToo)
            {
                if (this._recipientsOfSubclassesAction == null)
                {
                    this._recipientsOfSubclassesAction = new Dictionary<Type, List<WeakActionAndToken>>();
                }
                dictionary = this._recipientsOfSubclassesAction;
            }
            else
            {
                if (this._recipientsStrictAction == null)
                {
                    this._recipientsStrictAction = new Dictionary<Type, List<WeakActionAndToken>>();
                }
                dictionary = this._recipientsStrictAction;
            }
            if (!dictionary.ContainsKey(key))
            {
                list = new List<WeakActionAndToken>();
                dictionary.Add(key, list);
            }
            else
            {
                list = dictionary[key];
            }
            WeakAction_1<TMessage> action2 = new WeakAction_1<TMessage>(recipient, action);
            WeakActionAndToken token3 = new WeakActionAndToken
            {
                Action = action2,
                Token = token
            };
            WeakActionAndToken item = token3;
            list.Add(item);
            this.Cleanup();
        }

        public static void Reset()
        {
            _defaultInstance = null;
        }

        public virtual void Send<TMessage>(TMessage message)
        {
            this.SendToTargetOrType<TMessage>(message, null, null);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This syntax is more convenient than other alternatives.")]
        public virtual void Send<TMessage, TTarget>(TMessage message)
        {
            this.SendToTargetOrType<TMessage>(message, typeof(TTarget), null);
        }

        public virtual void Send<TMessage>(TMessage message, object token)
        {
            this.SendToTargetOrType<TMessage>(message, null, token);
        }

        private static void SendToList<TMessage>(TMessage message, IEnumerable<WeakActionAndToken> list, Type messageTargetType, object token)
        {
            if (list != null)
            {
                List<WeakActionAndToken> list2 = list.Take<WeakActionAndToken>(list.Count<WeakActionAndToken>()).ToList<WeakActionAndToken>();
                foreach (WeakActionAndToken token2 in list2)
                {
                    IExecuteWithObject action = token2.Action as IExecuteWithObject;
                    if (((((action != null) && token2.Action.IsAlive) && (token2.Action.Target != null)) && (((messageTargetType == null) || (token2.Action.Target.GetType() == messageTargetType)) || Implements(token2.Action.Target.GetType(), messageTargetType))) && (((token2.Token == null) && (token == null)) || ((token2.Token != null) && token2.Token.Equals(token))))
                    {
                        action.ExecuteWithObject(message);
                    }
                }
            }
        }

        private void SendToTargetOrType<TMessage>(TMessage message, Type messageTargetType, object token)
        {
            List<WeakActionAndToken> list2;
            Type instanceType = typeof(TMessage);
            if (this._recipientsOfSubclassesAction != null)
            {
                List<Type> list = this._recipientsOfSubclassesAction.Keys.Take<Type>(this._recipientsOfSubclassesAction.Count<KeyValuePair<Type, List<WeakActionAndToken>>>()).ToList<Type>();
                foreach (Type type2 in list)
                {
                    list2 = null;
                    if (((instanceType == type2) || instanceType.GetType().GetTypeInfo().IsSubclassOf(type2)) || Implements(instanceType, type2))
                    {
                        list2 = this._recipientsOfSubclassesAction[type2];
                    }
                    SendToList<TMessage>(message, list2, messageTargetType, token);
                }
            }
            if ((this._recipientsStrictAction != null) && this._recipientsStrictAction.ContainsKey(instanceType))
            {
                list2 = this._recipientsStrictAction[instanceType];
                SendToList<TMessage>(message, list2, messageTargetType, token);
            }
            this.Cleanup();
        }

        public virtual void Unregister(object recipient)
        {
            UnregisterFromLists(recipient, this._recipientsOfSubclassesAction);
            UnregisterFromLists(recipient, this._recipientsStrictAction);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "The type parameter TMessage identifies the message type that the recipient wants to unregister for.")]
        public virtual void Unregister<TMessage>(object recipient)
        {
            this.Unregister<TMessage>(recipient, null);
        }

        public virtual void Unregister<TMessage>(object recipient, Action<TMessage> action)
        {
            UnregisterFromLists<TMessage>(recipient, action, this._recipientsStrictAction);
            UnregisterFromLists<TMessage>(recipient, action, this._recipientsOfSubclassesAction);
            this.Cleanup();
        }

        private static void UnregisterFromLists(object recipient, Dictionary<Type, List<WeakActionAndToken>> lists)
        {
            if (((recipient != null) && (lists != null)) && (lists.Count != 0))
            {
                lock (lists)
                {
                    foreach (Type type in lists.Keys)
                    {
                        foreach (WeakActionAndToken token in lists[type])
                        {
                            WeakAction action = token.Action;
                            if ((action != null) && (recipient == action.Target))
                            {
                                action.MarkForDeletion();
                            }
                        }
                    }
                }
            }
        }

        private static void UnregisterFromLists<TMessage>(object recipient, Action<TMessage> action, Dictionary<Type, List<WeakActionAndToken>> lists)
        {
            Type key = typeof(TMessage);
            if ((((recipient != null) && (lists != null)) && (lists.Count != 0)) && lists.ContainsKey(key))
            {
                lock (lists)
                {
                    foreach (WeakActionAndToken token in lists[key])
                    {
                        WeakAction_1<TMessage> action2 = token.Action as WeakAction_1<TMessage>;
                        if (((action2 != null) && (recipient == action2.Target)) && ((action == null)))
                        {
                            token.Action.MarkForDeletion();
                        }
                    }
                }
            }
        }

        public static Messenger Default
        {
            get
            {
                if (_defaultInstance == null)
                {
                    _defaultInstance = new Messenger();
                }
                return _defaultInstance;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WeakActionAndToken
        {
            public WeakAction Action;
            public object Token;
        }
    }
}
