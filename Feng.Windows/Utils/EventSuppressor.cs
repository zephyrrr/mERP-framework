using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 取消Event事件
    /// </summary>
    public class EventSuppressor
    {
        Component _source;
        EventHandlerList _sourceEventHandlerList;
        Dictionary<object, Delegate[]> suppressedHandlers = new Dictionary<object, Delegate[]>();
        FieldInfo _headFI;
        Dictionary<object, Delegate[]> _handlers;
        PropertyInfo _sourceEventsInfo;
        Type _eventHandlerListType;
        Type _sourceType;

        public EventSuppressor(Component source)
        {
            if (source == null)
                throw new ArgumentNullException("control", "An instance of a control must be provided.");

            _source = source;
            _sourceType = _source.GetType();
            _sourceEventsInfo = _sourceType.GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
            _sourceEventHandlerList = (EventHandlerList)_sourceEventsInfo.GetValue(_source, null);
            _eventHandlerListType = _sourceEventHandlerList.GetType();
            _headFI = _eventHandlerListType.GetField("head", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        private void BuildList()
        {
            _handlers = new Dictionary<object, Delegate[]>();
            object head = _headFI.GetValue(_sourceEventHandlerList);
            if (head != null)
            {
                Type listEntryType = head.GetType();
                FieldInfo delegateFI = listEntryType.GetField("handler", BindingFlags.Instance | BindingFlags.NonPublic);
                FieldInfo keyFI = listEntryType.GetField("key", BindingFlags.Instance | BindingFlags.NonPublic);
                FieldInfo nextFI = listEntryType.GetField("next", BindingFlags.Instance | BindingFlags.NonPublic);
                BuildListWalk(head, delegateFI, keyFI, nextFI);
            }
        }

        private void BuildListWalk(object entry, FieldInfo delegateFI, FieldInfo keyFI, FieldInfo nextFI)
        {
            if (entry != null)
            {
                Delegate dele = (Delegate)delegateFI.GetValue(entry);
                object key = keyFI.GetValue(entry);
                object next = nextFI.GetValue(entry);

                Delegate[] listeners = dele.GetInvocationList();
                if (listeners != null && listeners.Length > 0)
                    _handlers.Add(key, listeners);

                if (next != null)
                {
                    BuildListWalk(next, delegateFI, keyFI, nextFI);
                }
            }
        }

        public void Resume()
        {
            Resume(null);
        }

        public void Resume(string pMethodName)
        {
            //if (_handlers == null)
            //    throw new ApplicationException("Events have not been suppressed.");

            Dictionary<object, Delegate[]> toRemove = new Dictionary<object, Delegate[]>();

            // goes through all handlers which have been suppressed.  If we are resuming,
            // all handlers, or if we find the matching handler, add it back to the
            // control's event handlers
            foreach (KeyValuePair<object, Delegate[]> pair in suppressedHandlers)
            {
                for (int x = 0; x < pair.Value.Length; x++)
                {

                    string methodName = pair.Value[x].Method.Name;
                    if (pMethodName == null || methodName.Equals(pMethodName))
                    {
                        _sourceEventHandlerList.AddHandler(pair.Key, pair.Value[x]);
                        toRemove.Add(pair.Key, pair.Value);
                    }
                }
            }
            // remove all un-suppressed handlers from the list of suppressed handlers
            foreach (KeyValuePair<object, Delegate[]> pair in toRemove)
            {
                for (int x = 0; x < pair.Value.Length; x++)
                {
                    suppressedHandlers.Remove(pair.Key);
                }
            }
            //_handlers = null;
        }

        public void Suppress()
        {
            Suppress(null);
        }
        public void Suppress(string pMethodName)
        {
            //if (_handlers != null)
            //    throw new ApplicationException("Events are already being suppressed.");

            BuildList();

            foreach (KeyValuePair<object, Delegate[]> pair in _handlers)
            {
                for (int x = pair.Value.Length - 1; x >= 0; x--)
                {
                    //MethodInfo mi = pair.Value[x].Method;
                    //string s1 = mi.Name; // name of the method
                    //object o = pair.Value[x].Target;
                    // can use this to invoke method    pair.Value[x].DynamicInvoke
                    string methodName = pair.Value[x].Method.Name;

                    if (pMethodName == null || methodName.Equals(pMethodName))
                    {
                        _sourceEventHandlerList.RemoveHandler(pair.Key, pair.Value[x]);
                        suppressedHandlers.Add(pair.Key, pair.Value);
                    }
                }
            }
        }

    }
}
