using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 数据缓存
    /// </summary>
    public interface IDataBuffer
    {
        /// <summary>
        /// 重新读取所有缓存数据（清除未用到的数据）
        /// </summary>
        void Reload();

        /// <summary>
        /// 读取缓存数据
        /// </summary>
        void LoadData();

        /// <summary>
        /// 清楚数据
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IDataBuffers : IDataBuffer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buf"></param>
        void AddDataBuffer(IDataBuffer buf);
    }

    /// <summary>
    /// GlobalData 
    /// </summary>
    public class DataBuffers : IDataBuffers
    {
        private List<IDataBuffer> m_buffers = new List<IDataBuffer>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buf"></param>
        public void AddDataBuffer(IDataBuffer buf)
        {
            m_buffers.Add(buf);
        }

        /// <summary>
        /// Reload
        /// </summary>
        public void Reload()
        {
            foreach (IDataBuffer i in m_buffers)
            {
                i.Reload();
            }
        }

        /// <summary>
        /// LoadData
        /// </summary>
        public void LoadData()
        {
            foreach (IDataBuffer i in m_buffers)
            {
                i.LoadData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            foreach (IDataBuffer i in m_buffers)
            {
                i.Clear();
            }
        }
    }
}
