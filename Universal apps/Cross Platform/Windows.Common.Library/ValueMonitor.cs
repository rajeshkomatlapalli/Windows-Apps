using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackgroungWorkerHelper
{
    /// <summary>
    /// For the Value monitor <b>ValueChanged</b> event.
    /// </summary>
    /// <typeparam name="ValueType"> The type of the value in ValueMonitor class </typeparam>
    /// <param name="oldValue"> The old value, that has been overwritten </param>
    /// <param name="newValue"> The new value, that has been set </param>
    public delegate void ValueChangedDelegate<ValueType>(ValueType oldValue, ValueType newValue);

    public interface IValueMonitor<ValueType>
    {
        ValueType Value { get; }
        event ValueChangedDelegate<ValueType> ValueChanged;
    }

    /// <summary>
    /// Monitors the value of any variable. 
    /// If the value changes by means of <b>set</b> function - the event is rised.
    /// If set function is called with the same value - then we still quiet.
    /// </summary>
    /// <typeparam name="ValueType"></typeparam>
    public class ValueMonitor<ValueType> : IValueMonitor<ValueType>
    {
        private ValueType aValue = default(ValueType);
        private IEqualityComparer<ValueType> comparer = null;

        /// <summary>
        /// Creates an instance of the value monitor with the operator== function as comparison function.
        /// </summary>
        /// <param name="initialValue"> The initial value of the variable. </param>
        public ValueMonitor(ValueType initialValue)
        {
            aValue = initialValue;
        }

        /// <summary>
        /// Creates an instance of ValueMonitor with the given comparer.
        /// </summary>
        /// <param name="initialValue">  The initial value of the variable.  </param>
        /// <param name="comparator"> The comparer object </param>
        public ValueMonitor(ValueType initialValue, IEqualityComparer<ValueType> comparator)
        {
            aValue = initialValue;
            comparer = comparator;
        }

        /// <summary>
        /// Gets or sets the value of the variable.
        /// </summary>
        public ValueType Value
        {
            get { return aValue; }
            set
            {
                bool areEqual = false;
                if (comparer == null)
                    areEqual = (aValue.Equals(value));
                else areEqual = comparer.Equals(aValue, value);

                if (areEqual == true) return;
                ValueType oldValue = aValue; // remember previous for the event rising
                aValue = value;
                if (ValueChanged != null)
                    ValueChanged(oldValue, aValue);
            }
        }

        /// <summary>
        /// "Quietly" sets the connection status.
        /// </summary>
        /// <param name="newValue"></param>
        public void SetQuietly(ValueType newValue)
        {
            aValue = newValue;
        }

        /// <summary>
        /// Raised, whenever the value REALLY changed
        /// </summary>
        public event ValueChangedDelegate<ValueType> ValueChanged;
    }
}
