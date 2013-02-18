using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.ComponentModel;

namespace Feng.Data
{
    /// <summary>
    /// 期望命令执行结果
    /// </summary>
    public enum ExpectedResultTypes
    {
        /// <summary>
        /// 大于等于0条
        /// </summary>
        GreaterThanOrEqualZero = 0,
        /// <summary>
        /// 大于0条
        /// </summary>
        GreaterThanZero = 1,
        /// <summary>
        /// 任意
        /// </summary>
        Any = 2,
        /// <summary>
        /// 特殊，看<see cref="MyDbCommand.ExpectedResult"/>
        /// </summary>
        [Description("Special Value for ExpectedResult")] Special = 9
    }

    /// <summary>
    /// 用于执行自定义命令的MyDbCommand，包含希望影响的行数，出错信息 
    /// </summary>
    public class MyDbCommand
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="type">期望命令执行结果</param>
        public MyDbCommand(DbCommand cmd, ExpectedResultTypes type)
            : this(cmd, type, null, s_defaultErrorMsg)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="type">期望命令执行结果</param>
        /// <param name="expected">期望特殊结果</param>
        public MyDbCommand(DbCommand cmd, ExpectedResultTypes type, string expected)
            : this(cmd, type, expected, s_defaultErrorMsg)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="type">期望命令执行结果</param>
        /// <param name="expected">期望特殊结果</param>
        /// <param name="errorMsg">错误提示</param>
        public MyDbCommand(DbCommand cmd, ExpectedResultTypes type, string expected, string errorMsg)
        {
            m_command = cmd;
            m_errorMsg = errorMsg;
            m_expectedResultType = type;
            m_expectedResult = expected;
        }

        private const string s_defaultErrorMsg = "本次操作未能成功完成，可能有其他人同时操作，请刷新后重试！";


        private DbCommand m_command;
        private ExpectedResultTypes m_expectedResultType;
        private string m_expectedResult;
        private string m_errorMsg;

        /// <summary>
        /// 数据库命令
        /// </summary>
        public DbCommand Command
        {
            get { return m_command; }
        }

        /// <summary>
        /// 如果是Update,Insert,Delete, 希望影响的行数
        /// </summary>
        public ExpectedResultTypes ExpectedResultType
        {
            get { return m_expectedResultType; }
        }

        /// <summary>
        /// 如果是Select语句，理想返回值
        /// </summary>
        public string ExpectedResult
        {
            get { return m_expectedResult; }
        }

        /// <summary>
        /// 出错信息 
        /// </summary>
        public string ErrorMsg
        {
            get { return m_errorMsg; }
        }

        /// <summary>
        /// 命令文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_command.CommandText;
        }
    }
}