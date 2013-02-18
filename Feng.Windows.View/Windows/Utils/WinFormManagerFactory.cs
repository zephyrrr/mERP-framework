using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using System.Linq.Expressions;

namespace Feng.Windows.Utils
{
    public class WinFormManagerFactory : ManagerFactory
    {
        delegate void Del(object sender, object e);

        private void AddEvent(WindowTabInfo tabInfo, object manager, WindowTabEventManagerType type)
        {
            var list = ADInfoBll.Instance.GetWindowTabEventInfos(tabInfo.Name, type);
            foreach (var info in list)
            {
                System.Reflection.EventInfo eventInfo = manager.GetType().GetEvent(info.EventName);

                Type handlerType = eventInfo.EventHandlerType;
                //MethodInfo invokeMethod = handlerType.GetMethod("Invoke");
                //ParameterInfo[] parms = invokeMethod.GetParameters();
                //Type[] parmTypes = new Type[parms.Length];
                //for (int i = 0; i < parms.Length; i++)
                //{
                //    parmTypes[i] = parms[i].ParameterType;
                //}
                //Type dType = Expression.GetActionType(parmTypes);
                Delegate d = null;
                switch (type)
                {
                    case WindowTabEventManagerType.SearchManager:
                        switch (info.EventName)
                        {
                            case "DataLoaded":
                                {
                                    EventHandler<DataLoadedEventArgs> d1 = (sender, e) =>
                                    {
                                        EventProcessUtils.ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(info.EventProcessName), sender, e);
                                    };
                                    d = Delegate.CreateDelegate(handlerType, d1.Target, d1.Method);
                                }
                                break;
                            case "DataLoading":
                                {
                                    EventHandler<DataLoadingEventArgs> d1 = (sender, e) =>
                                    {
                                        EventProcessUtils.ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(info.EventProcessName), sender, e);
                                    };
                                    d = Delegate.CreateDelegate(handlerType, d1.Target, d1.Method);
                                }
                                break;
                            default:
                                throw new ArgumentException("invalid EventName of " + info.EventName);
                        }
                        break;
                    case WindowTabEventManagerType.DisplayManager:
                        switch (info.EventName)
                        {
                            case "PositionChanging":
                                {
                                    EventHandler<System.ComponentModel.CancelEventArgs> d1 = (sender, e) =>
                                    {
                                        EventProcessUtils.ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(info.EventProcessName), sender, e);
                                    };
                                    d = Delegate.CreateDelegate(handlerType, d1.Target, d1.Method);
                                }
                                break;
                            case "PositionChanged":
                                {
                                    EventHandler d1 = (sender, e) =>
                                    {
                                        EventProcessUtils.ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(info.EventProcessName), sender, e);
                                    };
                                    d = Delegate.CreateDelegate(handlerType, d1.Target, d1.Method);
                                }
                                break;
                            case "SelectedDataValueChanged":
                                {
                                    EventHandler<SelectedDataValueChangedEventArgs> d1 = (sender, e) =>
                                    {
                                        EventProcessUtils.ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(info.EventProcessName), sender, e);
                                    };
                                    d = Delegate.CreateDelegate(handlerType, d1.Target, d1.Method);
                                }
                                break;
                            default:
                                throw new ArgumentException("invalid EventName of " + info.EventName);
                        }
                        break;
                    case WindowTabEventManagerType.ControlManager:
                        switch (info.EventName)
                        {
                            case "BeginningEdit":
                            case "EditBegun":
                            case "EndingEdit":
                            case "EditEnded":
                            case "EditCanceled":
                            case "StateChanged":
                                {
                                    EventHandler d1 = (sender, e) =>
                                    {
                                        EventProcessUtils.ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(info.EventProcessName), sender, e);
                                    };
                                    d = Delegate.CreateDelegate(handlerType, d1.Target, d1.Method);
                                }
                                break;
                            case "CancellingEdit":
                                {
                                    EventHandler<System.ComponentModel.CancelEventArgs> d1 = (sender, e) =>
                                    {
                                        EventProcessUtils.ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(info.EventProcessName), sender, e);
                                    };
                                    d = Delegate.CreateDelegate(handlerType, d1.Target, d1.Method);
                                }
                                break;
                            default:
                                throw new ArgumentException("invalid EventName of " + info.EventName);
                        }

                        break;
                    case WindowTabEventManagerType.BusinessLayer:
                        break;
                    default:
                        throw new ArgumentException("invalid WindowTabEventManagerType of " + type);
                }
                if (d != null)
                {
                    eventInfo.AddEventHandler(manager, d);
                }
            }
        }

        public override IControlManager GenerateControlManager(WindowTabInfo tabInfo, ISearchManager sm)
        {
            IControlManager cm = base.GenerateControlManager(tabInfo, sm);
            IDisplayManager dm = cm.DisplayManager;

            if (tabInfo.SelectedDataValueChanged != null)
            {
                dm.SelectedDataValueChanged += new EventHandler<SelectedDataValueChangedEventArgs>(delegate(object sender, SelectedDataValueChangedEventArgs e)
                {
                    EventProcessUtils.ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(tabInfo.SelectedDataValueChanged), sender, e);
                });
            }
            if (tabInfo.PositionChanged != null)
            {
                dm.PositionChanged += new EventHandler(delegate(object sender, EventArgs e)
                {
                    EventProcessUtils.ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(tabInfo.PositionChanged), sender, e);
                });
            }

            AddEvent(tabInfo, cm, WindowTabEventManagerType.ControlManager);
            AddEvent(tabInfo, cm.DisplayManager, WindowTabEventManagerType.DisplayManager);

            return cm;
        }

        public override IDisplayManager GenerateDisplayManager(WindowTabInfo tabInfo, ISearchManager sm)
        {
            IDisplayManager dm = base.GenerateDisplayManager(tabInfo, sm);

            if (tabInfo.SelectedDataValueChanged != null)
            {
                dm.SelectedDataValueChanged += new EventHandler<SelectedDataValueChangedEventArgs>(delegate(object sender, SelectedDataValueChangedEventArgs e)
                {
                    EventProcessUtils.ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(tabInfo.SelectedDataValueChanged), sender, e);
                });
            }
            if (tabInfo.PositionChanged != null)
            {
                dm.PositionChanged += new EventHandler(delegate(object sender, EventArgs e)
                {
                    EventProcessUtils.ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(tabInfo.PositionChanged), sender, e);
                });
            }

            AddEvent(tabInfo, dm, WindowTabEventManagerType.DisplayManager);
            return dm;
        }

        public override ISearchManager GenerateSearchManager(WindowTabInfo tabInfo, IDisplayManager dmParent)
        {
            ISearchManager sm = base.GenerateSearchManager(tabInfo, dmParent);
            AddEvent(tabInfo, sm, WindowTabEventManagerType.SearchManager);
            return sm;
        }

        public override IBaseDao GenerateBusinessLayer(WindowTabInfo tabInfo)
        {
            IBaseDao dao = base.GenerateBusinessLayer(tabInfo);
            AddEvent(tabInfo, dao, WindowTabEventManagerType.BusinessLayer);
            return dao;
        }
    }
}
