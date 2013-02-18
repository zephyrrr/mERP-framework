namespace Feng
{
    using System;
    using System.Security.Permissions;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ListChangedEventHandler(object sender, ListChangedEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public enum ListChangedType
    {
        /// <summary>
        /// 
        /// </summary>
        Reset,
        /// <summary>
        /// 
        /// </summary>
        ItemAdded,
        /// <summary>
        /// 
        /// </summary>
        ItemDeleted,
        /// <summary>
        /// 
        /// </summary>
        ItemMoved,
        /// <summary>
        /// 
        /// </summary>
        ItemChanged,
        /// <summary>
        /// 
        /// </summary>
        PropertyDescriptorAdded,
        /// <summary>
        /// 
        /// </summary>
        PropertyDescriptorDeleted,
        /// <summary>
        /// 
        /// </summary>
        PropertyDescriptorChanged
    }

    /// <summary>
    /// 
    /// </summary>
    public class ListChangedEventArgs : EventArgs
    {
        private ListChangedType listChangedType;
        private int newIndex;
        private int oldIndex;
        private System.ComponentModel.PropertyDescriptor propDesc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listChangedType"></param>
        /// <param name="propDesc"></param>
        public ListChangedEventArgs(ListChangedType listChangedType, System.ComponentModel.PropertyDescriptor propDesc)
        {
            this.listChangedType = listChangedType;
            this.propDesc = propDesc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listChangedType"></param>
        /// <param name="newIndex"></param>
        public ListChangedEventArgs(ListChangedType listChangedType, int newIndex) : this(listChangedType, newIndex, -1)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listChangedType"></param>
        /// <param name="newIndex"></param>
        /// <param name="propDesc"></param>
        public ListChangedEventArgs(ListChangedType listChangedType, int newIndex, System.ComponentModel.PropertyDescriptor propDesc) : this(listChangedType, newIndex)
        {
            this.propDesc = propDesc;
            this.oldIndex = newIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listChangedType"></param>
        /// <param name="newIndex"></param>
        /// <param name="oldIndex"></param>
        public ListChangedEventArgs(ListChangedType listChangedType, int newIndex, int oldIndex)
        {
            this.listChangedType = listChangedType;
            this.newIndex = newIndex;
            this.oldIndex = oldIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        public ListChangedType ListChangedType
        {
            get
            {
                return this.listChangedType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int NewIndex
        {
            get
            {
                return this.newIndex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int OldIndex
        {
            get
            {
                return this.oldIndex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.ComponentModel.PropertyDescriptor PropertyDescriptor
        {
            get
            {
                return this.propDesc;
            }
        }
    }
}

