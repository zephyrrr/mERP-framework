using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 和数据绑定的SmartExplorerTaskPane
    /// </summary>
    public class ToDoTaskTaskPane : Xceed.SmartUI.Controls.ExplorerTaskPane.SmartExplorerTaskPane
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ToDoTaskTaskPane()
        {
            Feng.Windows.Utils.XceedUtility.SetUIStyle(this);

            this.KeyDown += new KeyEventHandler(smartExplorerTaskPane1_KeyDown);
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="container"></param>
        //public ToDoTaskTaskPane(System.ComponentModel.IContainer container)
        //    : base(container)
        //{
        //    this.KeyDown += new KeyEventHandler(smartExplorerTaskPane1_KeyDown);
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        static ToDoTaskTaskPane()
        {
            s_Images[0] = ImageResource.Get("Feng", "Icons.time_star.png").Reference;
            s_Images[1] = ImageResource.Get("Feng", "Icons.time_yueliang.png").Reference;
            s_Images[2] = ImageResource.Get("Feng", "Icons.time_sun.png").Reference;
        }

        /// <summary>
        /// Task method
        /// </summary>
        /// <returns></returns>
        public delegate TaskPaneData GetTaskPaneInfosMethod();

        private List<GetTaskPaneInfosMethod> m_methods = new List<GetTaskPaneInfosMethod>();
        private static Image[] s_Images = new Image[3];


        /// <summary>
        /// 加入任务
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(GetTaskPaneInfosMethod task)
        {
            m_methods.Add(task);
        }

        /// <summary>
        /// Execute all task
        /// </summary>
        public void Execute()
        {
            foreach (GetTaskPaneInfosMethod method in m_methods)
            {
                TaskPaneData taskData = method();

                Xceed.SmartUI.SmartItem group;
                group = this.Items[taskData.GroupTitle];
                if (group == null)
                {
                    int groupIndex = this.Items.Add(taskData.GroupTitle);
                    group = this.Items[groupIndex];
                    group.Key = taskData.GroupTitle;
                }

                Xceed.SmartUI.SmartItem item;
                item = group.Items[taskData.TaskTitle];
                if (item == null)
                {
                    int taskIndex = group.Items.Add(taskData.TaskTitle);
                    item = group.Items[taskIndex];
                }
                if (taskData.Level > 0 && taskData.Level <= 3)
                {
                    item.Image = s_Images[taskData.Level - 1];
                }
                item.Tag = taskData.Task;
                item.Click -= new Xceed.SmartUI.SmartItemClickEventHandler(item_Click);
                item.Click += new Xceed.SmartUI.SmartItemClickEventHandler(item_Click);
            }
        }

        private void smartExplorerTaskPane1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                this.Items.Clear();
                Execute();
            }
        }

        private void item_Click(object sender, Xceed.SmartUI.SmartItemClickEventArgs e)
        {
            Xceed.SmartUI.SmartItem item = sender as Xceed.SmartUI.SmartItem;
            TaskPaneData.DoTask task = item.Tag as TaskPaneData.DoTask;
            if (task != null)
            {
                task();
            }
        }
    }

    /// <summary>
    /// 任务面板
    /// </summary>
    public class TaskPaneData
    {
        /// <summary>
        /// delegate for DoTask()
        /// </summary>
        public delegate System.Windows.Forms.Form DoTask();

        private DoTask m_doTask;
        private int m_level;
        private string m_taskTitle;
        private string m_groupTitle;

        /// <summary>
        /// 是否需要提醒
        /// </summary>
        public int Level
        {
            get { return m_level; }
        }

        /// <summary>
        /// 任务标题
        /// </summary>
        public string TaskTitle
        {
            get { return m_taskTitle; }
        }

        /// <summary>
        /// 任务组标题
        /// </summary>
        public string GroupTitle
        {
            get { return m_groupTitle; }
        }

        /// <summary>
        /// 对应任务
        /// </summary>
        public DoTask Task
        {
            get { return m_doTask; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="groupTitle"></param>
        /// <param name="taskTitle"></param>
        /// <param name="level"></param>
        /// <param name="doTask"></param>
        public TaskPaneData(string groupTitle, string taskTitle, int level, DoTask doTask)
        {
            m_groupTitle = groupTitle;
            m_taskTitle = taskTitle;
            m_level = level;
            m_doTask = doTask;
        }
    }
}