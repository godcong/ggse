using System.Windows.Forms;

namespace guigubahuang
{
    public class CacheData
    {
        public static string[] CacheName = { "O33a@9fkh++YwffVNeV1RA==.cache",
                                             "O33a@9fkh+9d5nka84Pi7w==.cache",
                                             "O33a@9fkh+8yY0g1c2@89Q==.cache",
                                             "eMZk7avfjaaSiFBwLwtZHg==.cache",
                                             "soegnXJ65qU3ZS4O9xLDgg==.cache"};

        public static string[] JsonName = { "DataUnit.json", "DataUnitLog.json", "DataUnitDie.json", "DataWorld.json", "DataBuildSchool.json" };

        public static string GetXinFa()
        {
            return Application.StartupPath + "\\心法.txt";
        }
        public static string GetZuoJian()
        {
            return Application.StartupPath + "\\左键.txt";
        }
        public static string GetYouJian()
        {
            return Application.StartupPath + "\\右键.txt";
        }
        public static string GetKongGe()
        {
            return Application.StartupPath + "\\空格.txt";
        }
        public static string GetRJian()
        {
            return Application.StartupPath + "\\R键.txt";
        }
        public static string GetZongMenZhiWei()
        {
            return Application.StartupPath + "\\职位.txt";
        }
        public static string GetJingJie()
        {
            return Application.StartupPath + "\\境界.txt";
        }
        public static string GetZongMenNamePath()
        {
            return Application.StartupPath + "\\宗门名.txt";
        }
        public static string GetNXingGePath()
        {
            return Application.StartupPath + "\\内在性格.txt";
        }
        public static string GetWXingGePath()
        {
            return Application.StartupPath + "\\外在性格.txt";
        }
        public static string GetAiHao()
        {
            return Application.StartupPath + "\\爱好.txt";
        }
        public static string GetXianTian()
        {
            return Application.StartupPath + "\\先天气运.txt";
        }
        public static string GetNiTian()
        {
            return Application.StartupPath + "\\逆天改命.txt";
        }
    }
}