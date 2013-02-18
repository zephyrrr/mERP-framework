using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Feng.Windows.Utils
{
	/// <summary>
	/// 关于中文的帮助类
	/// </summary>
	public static class ChineseHelper
    {
        #region "中文字符"
        /// <summary>
		/// 返回包含中文字符的字符串长度。
		/// C# 的string.Length中中文字只做1位统计,所以要将其转换为2位
		/// </summary>       
		/// <param name="str">要统计长度的字符串变量</param>
		/// <returns>字符串长度</returns>
		public static int GetChineseLength(string str)
		{
			return Encoding.GetEncoding("GB2312").GetBytes(str).Length;
		}

		private static string PadChinese(string str, int length, bool padLeft)
		{
			// 原始字符串长度，中文字符按2位计
			int objectStringLength = GetChineseLength(str);
			if (str == null || objectStringLength > length)
			{
				throw new ArgumentException("Invalid string", "str");
			}
			else
			{
				// 需自动填充的长度
				int suffixLength = length - objectStringLength;
				// 不足位数补" "
				StringBuilder sb = new StringBuilder();
				sb.Append(' ', suffixLength);
				return padLeft ? sb.Append(str).ToString() : sb.Insert(0, str).ToString();
			}
		}

		/// <summary>
		/// 左对齐字符串
		/// <remarks>
        /// 如原始字符串不满足参数<paramref name="length"/>指定的长度则在<paramref name="str"/>指定的原始字符串后补'~'
		/// </remarks>
		/// </summary>
		/// <param name="str">原始字符串</param>
		/// <param name="length">左对齐后的字符串长度</param>
		/// <returns>左对齐后的字符串</returns>
		/// <example>
		/// 如调用方式为PadRightString("123",5);
		/// 则返回值为"123~~"
		/// </example>
		/// <exception cref="ArgumentException">
        /// 当参数<paramref name="str"/>的长度超出参数<paramref name="length"/>指定的值时抛出
		/// </exception>
		public static string PadRightChinese(string str, int length)
		{
			return PadChinese(str, length, false);
		}

		/// <summary>
		/// 右对齐字符串
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string PadLeftChinese(string str, int length)
		{
			return PadChinese(str, length, true);
		}
		#endregion

		#region "大写金额"
		/// <summary>
		/// 中文大写金额转换
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string ConvertToChinese(decimal number)
		{
			return ConvertToChinese((double)number);
		}

		/// <summary>
		/// 中文大写金额转换（0 = 零元整）
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string ConvertToChinese(double number)
		{
			//return Microsoft.International.Formatters.EastAsiaNumericFormatter.FormatWithCulture("Lc", number, null, new System.Globalization.CultureInfo("zh-CN"));  
            if (Double.IsInfinity(number)
                || Double.IsNaN(number)
                || Double.IsNegativeInfinity(number)
                || Double.IsPositiveInfinity(number))
            {
                throw new ArgumentException("超出范围的人民币值");
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
		#endregion

		#region "拼音"
        private static IList<string> s_emtpyList = new List<string>();
		private static Dictionary<char, List<string>> s_buffers = new Dictionary<char, List<string>>();
		//private static int m_bufferCount = 1000;
		///// <summary>
		///// Buffer
		///// </summary>
		//public static int BufferCount
		//{
		//    get { return m_bufferCount; }
		//    set { m_bufferCount = value; }
		//}

		/// <summary>
		/// 汉字转拼音
		/// </summary>
		/// <param name="hanzi"></param>
		/// <returns></returns>
		private static IList<string> ConvertToPinYinInternal(char hanzi)
		{
			if (s_buffers.ContainsKey(hanzi))
				return s_buffers[hanzi];

			List<string> ret = new List<string>();
            Microsoft.International.Converters.PinYinConverter.ChineseChar chineseChar = new Microsoft.International.Converters.PinYinConverter.ChineseChar(hanzi);
			for (int i = 0; i < chineseChar.PinyinCount; ++i)
			{
				ret.Add(chineseChar.Pinyins[i]);
			}
			s_buffers[hanzi] = ret;
			return ret;
		}

		/// <summary>
		/// 单个汉字转拼音。
        /// 如输入字符非法，返回空列表
		/// </summary>
		/// <param name="hanzi"></param>
		/// <returns></returns>
		public static IList<string> ConvertToPinYin(char hanzi)
		{
            if (Microsoft.International.Converters.PinYinConverter.ChineseChar.IsValidChar(hanzi))
			{
				return ConvertToPinYinInternal(hanzi);
			}
            return s_emtpyList;
		}

		/// <summary>
		/// 检测text的拼音是否以pinyin开头
		/// </summary>
		/// <param name="pinyin"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool IsPinYinofText(string pinyin, string text)
		{
			int i = 0;
			for (int j = 0; j < text.Length; ++j)
			{
				if (i >= pinyin.Length)
					break;

				if (Char.IsDigit(text[j]) || Char.IsUpper(text[j]) || Char.IsLower(text[j]))
				{
					if (Char.ToLower(pinyin[i]) != Char.ToLower(text[j]))
						return false;

					i++;
				}
                else if (Microsoft.International.Converters.PinYinConverter.ChineseChar.IsValidChar(text[j]))
				{
					IList<string> pys = ConvertToPinYinInternal(text[j]);
					bool ok = false;
					foreach (string py in pys)
					{
						if (Char.ToLower(py[0]) == Char.ToLower(pinyin[i]))
						{
							ok = true;
							break;
						}
					}
					if (!ok)
						return false;

					i++;
				}
			}

			return (i == pinyin.Length);
		}

		///// <summary>
		///// 汉字词组转拼音
		///// </summary>
		///// <param name="hanzi"></param>
		///// <returns></returns>
		//private static string[] ConvertToPinYin(string hanzi)
		//{
		//    List<string> ret = new List<string>();
		//    foreach (char c in hanzi)
		//        ret.Add(ConvertToPinYin(c));
		//    return ret.ToArray();
		//}

		///// <summary>
		///// 汉字词组转拼音
		///// </summary>
		///// <param name="hanzi"></param>
		///// <returns></returns>
		//public static string ConvertToPinYinWithoutTone(string hanzi)
		//{
		//    string[] pys = ConvertToPinYin(hanzi);
		//    StringBuilder sb = new StringBuilder();
		//    foreach (string py in pys)
		//    {
		//        if (string.IsNullOrEmpty(py))
		//            continue;
		//        sb.Append(py.Substring(0, py.Length - 1));
		//    }
		//    return sb.ToString();
		//}

		//        private static readonly int[] pyvalue = new int[]{-20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,-20032,-20026, 
		//-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,-19756,-19751,-19746,-19741,-19739,-19728, 
		//-19725,-19715,-19540,-19531,-19525,-19515,-19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263, 
		//-19261,-19249,-19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,-19003,-18996, 
		//-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,-18731,-18722,-18710,-18697,-18696,-18526, 
		//-18518,-18501,-18490,-18478,-18463,-18448,-18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, 
		//-18181,-18012,-17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,-17733,-17730, 
		//-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,-17468,-17454,-17433,-17427,-17417,-17202, 
		//-17185,-16983,-16970,-16942,-16915,-16733,-16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459, 
		//-16452,-16448,-16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,-16212,-16205, 
		//-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,-15933,-15920,-15915,-15903,-15889,-15878, 
		//-15707,-15701,-15681,-15667,-15661,-15659,-15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416, 
		//-15408,-15394,-15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,-15149,-15144, 
		//-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,-14941,-14937,-14933,-14930,-14929,-14928, 
		//-14926,-14922,-14921,-14914,-14908,-14902,-14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668, 
		//-14663,-14654,-14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,-14170,-14159, 
		//-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,-14109,-14099,-14097,-14094,-14092,-14090, 
		//-14087,-14083,-13917,-13914,-13910,-13907,-13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658, 
		//-13611,-13601,-13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,-13340,-13329, 
		//-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,-13068,-13063,-13060,-12888,-12875,-12871, 
		//-12860,-12858,-12852,-12849,-12838,-12831,-12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346, 
		//-12320,-12300,-12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,-11781,-11604, 
		//-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,-11055,-11052,-11045,-11041,-11038,-11024, 
		//-11020,-11019,-11018,-11014,-10838,-10832,-10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331, 
		//-10329,-10328,-10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254};

		//        private static string[] pystr = new string[]{"a","ai","an","ang","ao","ba","bai","ban","bang","bao","bei","ben","beng","bi","bian","biao", 
		//"bie","bin","bing","bo","bu","ca","cai","can","cang","cao","ce","ceng","cha","chai","chan","chang","chao","che","chen", 
		//"cheng","chi","chong","chou","chu","chuai","chuan","chuang","chui","chun","chuo","ci","cong","cou","cu","cuan","cui", 
		//"cun","cuo","da","dai","dan","dang","dao","de","deng","di","dian","diao","die","ding","diu","dong","dou","du","duan", 
		//"dui","dun","duo","e","en","er","fa","fan","fang","fei","fen","feng","fo","fou","fu","ga","gai","gan","gang","gao", 
		//"ge","gei","gen","geng","gong","gou","gu","gua","guai","guan","guang","gui","gun","guo","ha","hai","han","hang", 
		//"hao","he","hei","hen","heng","hong","hou","hu","hua","huai","huan","huang","hui","hun","huo","ji","jia","jian", 
		//"jiang","jiao","jie","jin","jing","jiong","jiu","ju","juan","jue","jun","ka","kai","kan","kang","kao","ke","ken", 
		//"keng","kong","kou","ku","kua","kuai","kuan","kuang","kui","kun","kuo","la","lai","lan","lang","lao","le","lei", 
		//"leng","li","lia","lian","liang","liao","lie","lin","ling","liu","long","lou","lu","lv","luan","lue","lun","luo", 
		//"ma","mai","man","mang","mao","me","mei","men","meng","mi","mian","miao","mie","min","ming","miu","mo","mou","mu", 
		//"na","nai","nan","nang","nao","ne","nei","nen","neng","ni","nian","niang","niao","nie","nin","ning","niu","nong", 
		//"nu","nv","nuan","nue","nuo","o","ou","pa","pai","pan","pang","pao","pei","pen","peng","pi","pian","piao","pie", 
		//"pin","ping","po","pu","qi","qia","qian","qiang","qiao","qie","qin","qing","qiong","qiu","qu","quan","que","qun", 
		//"ran","rang","rao","re","ren","reng","ri","rong","rou","ru","ruan","rui","run","ruo","sa","sai","san","sang", 
		//"sao","se","sen","seng","sha","shai","shan","shang","shao","she","shen","sheng","shi","shou","shu","shua", 
		//"shuai","shuan","shuang","shui","shun","shuo","si","song","sou","su","suan","sui","sun","suo","ta","tai", 
		//"tan","tang","tao","te","teng","ti","tian","tiao","tie","ting","tong","tou","tu","tuan","tui","tun","tuo", 
		//"wa","wai","wan","wang","wei","wen","weng","wo","wu","xi","xia","xian","xiang","xiao","xie","xin","xing", 
		//"xiong","xiu","xu","xuan","xue","xun","ya","yan","yang","yao","ye","yi","yin","ying","yo","yong","you", 
		//"yu","yuan","yue","yun","za","zai","zan","zang","zao","ze","zei","zen","zeng","zha","zhai","zhan","zhang", 
		//"zhao","zhe","zhen","zheng","zhi","zhong","zhou","zhu","zhua","zhuai","zhuan","zhuang","zhui","zhun","zhuo", 
		//"zi","zong","zou","zu","zuan","zui","zun","zuo"};

		//        private static readonly Regex regexChinese = new Regex(@"^[\u4e00-\u9fa5]+$"); 
		//        /// <summary>
		//        /// 转化程序
		//        /// </summary>
		//        /// <param name="hanzi"></param>
		//        /// <returns></returns>
		//        public static string Convert(string hanzi) 
		//        {
		//            if (string.IsNullOrEmpty(hanzi))
		//            {
		//                throw new ArgumentNullException("chrstr");
		//            }

		//            if (!regexChinese.IsMatch(hanzi))
		//            {
		//                return "";
		//            }

		//            StringBuilder returnstr = new StringBuilder(); 
		//            char[] nowchar = hanzi.ToCharArray();
		//            for (int j = 0; j < nowchar.Length; j++)
		//            {
		//                string now = nowchar[j].ToString();
		//                byte[] array = System.Text.Encoding.Default.GetBytes(now);
		//                int i1 = (short)(array[0]);
		//                int i2 = (short)(array[1]);

		//                int chrasc = i1 * 256 + i2 - 65536;
		//                if (chrasc > 0 && chrasc < 160)
		//                {
		//                    returnstr.Append(now);
		//                }
		//                else
		//                {
		//                    for (int i = (pyvalue.Length - 1); i >= 0; i--)
		//                    {
		//                        if (pyvalue[i] <= chrasc)
		//                        {
		//                            returnstr.Append(pystr[i].ToString());
		//                            break;
		//                        }
		//                    }
		//                }
		//            }
		//            return returnstr.ToString(); 
		//        }
		#endregion
	}
}
