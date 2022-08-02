using Microsoft.International.Converters.PinYinConverter;
using System.Text.RegularExpressions;

namespace Common.Utils
{
    public static class PinYinHelper
    {
        public static PinYinModel GetPinYin(this string str)
        {
            var chs = str.ToCharArray();
            //记录每个汉字的全拼
            Dictionary<int, List<string>> TotalPingYinLists = new Dictionary<int, List<string>>();
            for (int i = 0; i < chs.Length; i++)
            {
                var pinyins = new List<string>();
                var ch = chs[i];
                //是否是有效的汉字
                if (ChineseChar.IsValidChar(ch))
                {
                    ChineseChar cc = new ChineseChar(ch);
                    pinyins = cc.Pinyins.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
                    //去除声调，转小写
                    pinyins = pinyins.ConvertAll(p => Regex.Replace(p, @"\d", "").ToLower());
                }
                else
                {
                    pinyins.Add(ch.ToString());
                }
                //去重
                pinyins = pinyins.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct().ToList();
                if (pinyins.Any())
                {
                    TotalPingYinLists[i] = pinyins;
                }
            }
            PinYinModel result = new PinYinModel();
            foreach (var pinyins in TotalPingYinLists)
            {
                var items = pinyins.Value;
                if (result.TotalPingYinList.Count <= 0)
                {
                    result.TotalPingYinList = items;
                    result.FirstPingYinList = items.ConvertAll(p => p.Substring(0, 1)).Distinct().ToList();
                }
                else
                {
                    //全拼循环匹配
                    var newTotalPingYinLists = new List<string>();
                    foreach (var TotalPingYinList in result.TotalPingYinList)
                    {
                        newTotalPingYinLists.AddRange(items.Select(item => TotalPingYinList + item));
                    }
                    newTotalPingYinLists = newTotalPingYinLists.Distinct().ToList();
                    result.TotalPingYinList = newTotalPingYinLists;

                    //首字母循环匹配
                    var newFirstPingYinLists = new List<string>();
                    foreach (var FirstPingYinList in result.FirstPingYinList)
                    {
                        newFirstPingYinLists.AddRange(items.Select(item => FirstPingYinList + item.Substring(0, 1)));
                    }
                    newFirstPingYinLists = newFirstPingYinLists.Distinct().ToList();
                    result.FirstPingYinList = newFirstPingYinLists;
                }
            }
            result.GetPinYinStr();
            return result;
        }
    }

    #region PinYinModel
    public class PinYinModel
    {
        public List<string> TotalPingYinList { get; set; } = new List<string>();
        public string TotalPingYin { get; set; }

        public List<string> FirstPingYinList { get; set; } = new List<string>();
        public string FirstPingYin { get; set; }

        public void GetPinYinStr()
        {
            this.TotalPingYin = string.Join(",", this.TotalPingYinList);
            this.FirstPingYin = string.Join(",", this.FirstPingYinList);
        }
    }
    #endregion
}
