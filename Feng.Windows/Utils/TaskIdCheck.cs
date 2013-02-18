using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 箱号格式检查
    /// </summary>
    public sealed class TaskIdCheckHelper
    {
        /// <summary>
        /// private Constructor
        /// </summary>
        private TaskIdCheckHelper()
        {
        }

        /// <summary>
        /// 验证集装箱箱号
        /// 集装箱编号共11位，前四位是字母，最后一位为校验码，举例如◎◎◎◎×××××××。
        /// 字母取数值规则为：A＝10，B至K依次取12至21，L至U依次取23至32，V至Z依次取34至38。
        /// 箱号第一位的值乘以2的0次幂，第二位乘以2的1次幂，...，第十位乘以2的9次幂，然后求和。
        /// 其和除以11的余数即为校验码的值
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public static bool Check(string taskId)
        {
            if (string.IsNullOrEmpty(taskId))
            {
                return false;
            }
            if (taskId.Length != 11)
            {
                return false;
            }
            for (int i = 0; i < 4; ++i)
            {
                if (taskId[i] < 'A' || taskId[i] > 'Z')
                {
                    return false;
                }
            }
            for (int i = 4; i < 11; ++i)
            {
                if (taskId[i] < '0' || taskId[i] > '9')
                {
                    return false;
                }
            }
            int sum = 0;
            for (int i = 0; i < 10; ++i)
            {
                int c = 0;
                if (taskId[i] == 'A')
                {
                    c = 10;
                }
                else if (taskId[i] >= 'B' && taskId[i] <= 'K')
                {
                    c = 12 + taskId[i] - 'B';
                }
                else if (taskId[i] >= 'L' && taskId[i] <= 'U')
                {
                    c = 23 + taskId[i] - 'L';
                }
                else if (taskId[i] >= 'V' && taskId[i] <= 'Z')
                {
                    c = 34 + taskId[i] - 'V';
                }
                else if (taskId[i] >= '0' && taskId[i] <= '9')
                {
                    c = taskId[i] - '0';
                }

                sum += c << i;
            }
            if (sum % 11 % 10 != (taskId[10] - '0'))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}