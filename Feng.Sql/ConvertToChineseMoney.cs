using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString ConvertToChineseMoney(SqlDecimal money)
    {
        // 在此处放置代码
        return new SqlString(ConvertToChinese(money.ToDouble()));
    }

    /// <summary>
    /// 中文大写金额转换（0 = 零元整）
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static string ConvertToChinese(double number)
    {
        if (Double.IsInfinity(number)
                || Double.IsNaN(number)
                || Double.IsNegativeInfinity(number)
                || Double.IsPositiveInfinity(number))
        {
            throw new ArgumentException("超出范围的人民币值", "number");
        }

        if (Double.MaxValue == number
            || Double.MinValue == number)
        {
            return string.Empty;
        }

        if (number == 0)
        {
            return "零元整";
        }
        bool negative = false;
        if (number < 0)
        {
            number = -number;
            negative = true;
        }

        string numList = "零壹贰叁肆伍陆柒捌玖";
        string rmbList = "分角元拾佰仟万拾佰仟亿拾佰仟万";
        StringBuilder tempOutString = new StringBuilder();

        //将小数转化为整数字符串
        string tempNumberString = Convert.ToInt64(number * 100).ToString();
        int tempNmberLength = tempNumberString.Length;
        int i = 0;
        while (i < tempNmberLength)
        {
            int oneNumber = Convert.ToInt32(tempNumberString.Substring(i, 1));
            string oneNumberChar = numList.Substring(oneNumber, 1);
            string oneNumberUnit = rmbList.Substring(tempNmberLength - i - 1, 1);
            if (oneNumberChar != "零")
                tempOutString.Append(oneNumberChar + oneNumberUnit);
            else
            {
                if (oneNumberUnit == "亿" || oneNumberUnit == "万" || oneNumberUnit == "元" || oneNumberUnit == "零")
                {
                    while (tempOutString.ToString().EndsWith("零", StringComparison.Ordinal))
                    {
                        tempOutString.Remove(tempOutString.Length - 1, 1);
                    }
                }
                if (oneNumberUnit == "亿" || (oneNumberUnit == "万" && !tempOutString.ToString().EndsWith("亿", StringComparison.Ordinal)) || oneNumberUnit == "元")
                {
                    tempOutString.Append(oneNumberUnit);
                }
                else
                {
                    bool tempEnd = tempOutString.ToString().EndsWith("亿", StringComparison.Ordinal);
                    bool zeroEnd = tempOutString.ToString().EndsWith("零", StringComparison.Ordinal);
                    if (tempOutString.Length > 1)
                    {
                        bool zeroStart = tempOutString.ToString().Substring(tempOutString.Length - 2, 2).StartsWith("零", StringComparison.Ordinal);
                        if (!zeroEnd && (zeroStart || !tempEnd))
                            tempOutString.Append(oneNumberChar);
                    }
                    else
                    {
                        if (!zeroEnd && !tempEnd)
                            tempOutString.Append(oneNumberChar);
                    }
                }
            }
            i += 1;
        }

        while (tempOutString.ToString().EndsWith("零", StringComparison.Ordinal))
        {
            tempOutString.Remove(tempOutString.Length - 1, 1);
        }

        while (tempOutString.ToString().EndsWith("元", StringComparison.Ordinal))
        {
            tempOutString.Append("整");
        }

        if (negative)
        {
            tempOutString.Insert(0, "负");
        }

        return tempOutString.ToString();
    }
};

