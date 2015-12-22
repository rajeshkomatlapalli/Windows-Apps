using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideosCardGame
{
    public class WeakAction
    {
        private readonly System.Action _action;
        private WeakReference _reference;

        public WeakAction(object target, System.Action action)
        {
            this._reference = new WeakReference(target);
            this._action = action;
        }

        public void Execute()
        {
            if ((this._action != null) && this.IsAlive)
            {
                this._action();
            }
        }

        public void MarkForDeletion()
        {
            this._reference = null;
        }

        public System.Action Action
        {
            get
            {
                return this._action;
            }
        }

        public bool IsAlive
        {
            get
            {
                if (this._reference == null)
                {
                    return false;
                }
                return this._reference.IsAlive;
            }
        }

        public object Target
        {
            get
            {
                if (this._reference == null)
                {
                    return null;
                }
                return this._reference.Target;
            }
        }
    }
}
