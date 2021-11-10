using System.Collections.Generic;

namespace guigubahuang
{
    public class Hero
    {
        public string ID { get; set; }
        public string Name { get { return FirstName + LastName; } }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsZhuJue { get; set; }
        public string ZongMenID { get; set; }
        public string LingShi { get; set; }
        public string JingJie { get; set; }
        public string NianLing { get; set; }
        public string NianLingSX { get; set; }
        public string XinQing { get; set; }
        public string XinQingSX { get; set; }
        public string JianKang { get; set; }
        public string JianKangSX { get; set; }
        public string JingLi { get; set; }
        public string JingLiSX { get; set; }
        public string TiLi { get; set; }
        public string TiLiSX { get; set; }
        public string LingLi { get; set; }
        public string LingLiSX { get; set; }
        public string NianLi { get; set; }
        public string NianLiSX { get; set; }
        public string XingYun { get; set; }
        public string WuXing { get; set; }
        public string MeiLi { get; set; }
        public string ShengWang { get; set; }
        public string GongJi { get; set; }
        public string FangYu { get; set; }
        public string JiaoLi { get; set; }
        public string GongFaKangXing { get; set; }
        public string LingGenKangXing { get; set; }
        public string HuiXin { get; set; }
        public string HuXin { get; set; }
        public string YiSu { get; set; }
        public string BaoJiBeiShu { get; set; }
        public string KangBaoBeiShu { get; set; }
        public string DaoDian { get; set; }
        public string DaoFa { get; set; }
        public string QiangFa { get; set; }
        public string JianFa { get; set; }
        public string QuanFa { get; set; }
        public string ZhangFa { get; set; }
        public string ZhiFa { get; set; }
        public string HuoLingGen { get; set; }
        public string ShuiLingGen { get; set; }
        public string LeiLingGen { get; set; }
        public string TuLingGen { get; set; }
        public string MuLingGen { get; set; }
        public string FengLingGen { get; set; }
        public string LianDan { get; set; }
        public string LianQi { get; set; }
        public string FengShui { get; set; }
        public string HuaFu { get; set; }
        public string YaoCai { get; set; }
        public string KuangCai { get; set; }
        public string ZhengDao { get; set; }
        public string MoDao { get; set; }
        public string NeiZaiXingGe { get; set; }
        public string WaiZaiXingGe { get; set; }
        public string WaiZaiXingGe2 { get; set; }
        public string[] XingQu { get; set; }
        public string[] XianTianQiYun { get; set; }
        public string[] NiTianGaiMing { get; set; }
        public string[] DaoXin { get; set; }
        public bool IsTianJiao { get; set; }

        public List<Relation> FuMu { get; set; } = new List<Relation>();
        public List<Relation> ZiNv { get; set; } = new List<Relation>();
        public List<Relation> SiShengZi { get; set; } = new List<Relation>();
        public List<Relation> XiongDiJieMei { get; set; } = new List<Relation>();
        public List<Relation> YiFuMu { get; set; } = new List<Relation>();
        public List<Relation> YiZiNv { get; set; } = new List<Relation>();
        public List<Relation> YiXiongDiJieMei { get; set; } = new List<Relation>();
        public Relation FuQi { get; set; }
        public List<Relation> Daolv { get; set; } = new List<Relation>();
        public List<Relation> ShiFu { get; set; } = new List<Relation>();
        public List<Relation> TuDi { get; set; } = new List<Relation>();
        public string HeroHaoGan { get; set; }
        public List<Relation> HaoYou { get; set; } = new List<Relation>();

        public string GetName()
        {
            return FirstName + LastName;
        }

        public string GetJingJie(string id)
        {
            switch (int.Parse(id))
            {
                case 1:
                    return "练气初期";
                case 2:
                    return "练气中期";
                case 3:
                    return "练气后期";
                case 4:
                case 5:
                case 6:
                    return "筑基初期";
                case 7:
                    return "筑基中期";
                case 8:
                    return "筑基后期";
                case 9:
                    return "结晶初期";
                case 10:
                    return "结晶中期";
                case 11:
                    return "结晶后期";
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                    return "金丹初期";
                case 17:
                    return "金丹中期";
                case 18:
                    return "金丹后期";
                case 19:
                    return "具灵初期";
                case 20:
                    return "具灵中期";
                case 21:
                    return "具灵后期";
                case 22:
                case 23:
                case 24:
                    return "元婴初期";
                case 25:
                    return "元婴中期";
                case 26:
                    return "元婴后期";
                case 27:
                    return "化神初期";
                case 28:
                    return "化神中期";
                case 29:
                    return "化神后期";
                case 30:
                case 31:
                case 32:
                case 33:
                    return "悟道初期";
                case 34:
                    return "悟道中期";
                case 35:
                    return "悟道后期";
                case 36:
                    return "羽化初期";
                case 37:
                    return "羽化中期";
                case 38:
                    return "羽化后期";
                case 39:
                case 40:
                case 41:
                case 42:
                    return "登仙初期";
                case 43:
                    return "登仙中期";
                case 44:
                    return "登仙后期";
                default:
                    return "";
            }
        }
    }
}
