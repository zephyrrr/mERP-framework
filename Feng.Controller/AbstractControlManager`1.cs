//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Reflection;
//using System.Data;
//using System.ComponentModel;
//using System.Diagnostics;
//using Feng.Collections;
//using Feng.Utils;

//namespace Feng
//{
//    /// <summary>
//    /// 控制管理器。可对多个实体类操作，进行增加、修改、删除等操作。
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public abstract class AbstractControlManager<T> : AbstractControlManager, IControlManager<T>
//        where T : class, IEntity
//    {
//        #region "Constructor"

//        /// <summary>
//        /// Consturctor
//        /// </summary>
//        /// <param name="displayManager"></param>
//        protected AbstractControlManager(IDisplayManager<T> displayManager)
//            : base(displayManager)
//        {
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public IDisplayManager<T> DisplayManagerT
//        {
//            get { return base.DisplayManager as IDisplayManager<T>; }
//        }

//        #endregion

//        # region "Operations"


//        //protected override void DoOnFailedOperation()
//        //{
//            //// Todo: 可能不处理，手工刷新更好。 需要处理load Detail， MemoryBll等的关系
//            //IList<ISearchExpression> searchExpression = new List<ISearchExpression>();
//            //object id = this.DisplayManager.EntityInfo.EntityType.InvokeMember(this.DisplayManager.EntityInfo.IdName,
//            //                                                                   BindingFlags.GetProperty, null,
//            //                                                                   this.DisplayManagerT.CurrentEntity, null,
//            //                                                                   null, null, null);

//            //searchExpression.Add(SearchExpression.Eq(this.DisplayManager.EntityInfo.IdName, id));
//            //IList<T> list = this.DisplayManagerT.SearchManager.FindData(searchExpression, null) as IList<T>;
//            //Debug.Assert(list.Count <= 1);

//            //if (list.Count == 0)
//            //{
//            //    if (this.State == StateType.Edit
//            //        || this.State == StateType.Delete)
//            //        //|| this.State == StateType.Add)
//            //    {
//            //        this.DisplayManager.Items.RemoveAt(this.DisplayManagerT.Position);
//            //        OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, this.DisplayManagerT.Position));
//            //    }
//            //    else if (this.State == StateType.Add)
//            //    {
//            //        this.DisplayManager.OnPositionChanged(System.EventArgs.Empty);

//            //        // there is no grid row now
//            //        //OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, this.DisplayManagerT.Position));
//            //    }
//            //}
//            //else if (list.Count == 1)
//            //{
//            //    if (this.State == StateType.Edit)
//            //    {
//            //        // 出错，还让处于编辑状态（如果因为权限改变而不能修改，也必须先主动退出编辑状态）
//            //        // this.CancelEdit();
//            //        this.DisplayManager.Items[this.DisplayManagerT.Position] = list[0];
//            //        OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, this.DisplayManagerT.Position));

//            //        // 可能已经不能修改，不再进入修改状态
//            //        // this.EditCurrent();
//            //    }
//            //    else if (this.State == StateType.Delete)
//            //    // ||  this.State == StateType.Add
//            //    {
//            //        this.DisplayManager.Items[this.DisplayManagerT.Position] = list[0];
//            //        OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, this.DisplayManagerT.Position));
//            //    }
//            //    else if (this.State == StateType.View)
//            //    {
//            //        this.DisplayManager.Items[this.DisplayManagerT.Position] = list[0];
//            //        OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, this.DisplayManagerT.Position));
//            //    }
//            //}

//            //this.DisplayManager.DisplayCurrent();
//        //}


//        #endregion
//    }
//}