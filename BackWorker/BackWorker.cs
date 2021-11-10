using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace guigubahuang
{
    public class BackWorker
    {
        #region 后天工作状态通知事件

        private bool _isBusy = false;

        public event EventHandler<WorkerStateChangedEventArgs> WorkerStateChanged;
        public bool IsBusy 
        { 
            get { return _isBusy; } 
            set
            {
                _isBusy = value;
                if (WorkerStateChanged != null)
                {
                    WorkerStateChanged(this, new WorkerStateChangedEventArgs(_isBusy));
                }
            } 
        }

        #endregion

        public void SaveGongFa(JObject jObj, string heroID, int level, int xinFaIndex, string left, string right, string space, string r)
        {
            try
            {
                IsBusy = true;
                if (level != 0 && xinFaIndex != 0 &&
                    !string.IsNullOrEmpty(left) && !string.IsNullOrEmpty(right) &&
                    !string.IsNullOrEmpty(space) && !string.IsNullOrEmpty(r) && !string.IsNullOrEmpty(heroID))
                {
                    List<string> list = new List<string>();
                    string s = string.Empty;
                    //"\"N47y4t\": {\"q\": {\"q\": \"N47y4t\",\"w\": " + left + ",\"e\": \"\"},\"w\": 99999999,\"e\": 0,\"r\": {\"allObject\": { }}}";
                    //读取心法
                    using (StreamReader file = new StreamReader(CacheData.GetXinFa()))
                    {
                        string value = file.ReadLine();
                        while (!string.IsNullOrEmpty(value))
                        {
                            if (!value.Contains("--"))
                            {
                                string[] tmp = value.Split('=');
                                if (tmp[0] == "0" || tmp[0] == xinFaIndex.ToString())
                                {
                                    list.Add(tmp[2]);
                                    s = s + "\"" + tmp[2] + "\": {\"q\": {\"q\": \"" + tmp[2] + "\",\"w\": " + tmp[3].Replace("level", level.ToString()) + ",\"e\": \"\"},\"w\": 99999999,\"e\": 0,\"r\": {\"allObject\": { }}},";
                                }
                            }
                            value = file.ReadLine();
                        }
                    }

                    string[] zuojian = Tools.GetGongFa(left, CacheData.GetZuoJian()).Split('=');
                    s = s + "\"" + zuojian[1] + "\": {\"q\": {\"q\": \"" + zuojian[1] + "\",\"w\": " + zuojian[2].Replace("level", level.ToString()) + ",\"e\": \"\"},\"w\": 99999999,\"e\": 0,\"r\": {\"allObject\": { }}},";

                    string[] youjian = Tools.GetGongFa(right, CacheData.GetYouJian()).Split('=');
                    s = s + "\"" + youjian[1] + "\": {\"q\": {\"q\": \"" + youjian[1] + "\",\"w\": " + youjian[2].Replace("level", level.ToString()) + ",\"e\": \"\"},\"w\": 99999999,\"e\": 0,\"r\": {\"allObject\": { }}},";

                    string[] kongge = Tools.GetGongFa(space, CacheData.GetKongGe()).Split('=');
                    s = s + "\"" + kongge[1] + "\": {\"q\": {\"q\": \"" + kongge[1] + "\",\"w\": " + kongge[2].Replace("level", level.ToString()) + ",\"e\": \"\"},\"w\": 99999999,\"e\": 0,\"r\": {\"allObject\": { }}},";

                    string[] rjian = Tools.GetGongFa(r, CacheData.GetRJian()).Split('=');
                    s = s + "\"" + rjian[1] + "\": {\"q\": {\"q\": \"" + rjian[1] + "\",\"w\": " + rjian[2].Replace("level", level.ToString()) + ",\"e\": \"\"},\"w\": 99999999,\"e\": 0,\"r\": {\"allObject\": { }}},";

                    jObj["allUnit"][heroID]["k"] = JObject.Parse("{" + s.Remove(s.Length - 1, 1) + "}");
                    jObj["allUnit"][heroID]["a"] = zuojian[1];
                    jObj["allUnit"][heroID]["s"] = youjian[1];
                    jObj["allUnit"][heroID]["d"] = rjian[1];
                    int index = 0;
                    foreach (JValue f in jObj["allUnit"][heroID]["f"])
                    {
                        f.Value = list[index];
                        index++;
                    }
                    jObj["allUnit"][heroID]["g"] = kongge[1];
                }
                else
                    throw new Exception("各选项都不允许为空");
            }
            catch { throw; }
            finally { IsBusy = false; }
        }
        public void SaveNiTianGaiMing(JObject jObj, string heroID, Hero hero)
        {
            try
            {
                for (int i = 0; i < ((JArray)jObj["allUnit"][heroID]["q"]["qn"]).Count; i++)
                {
                    if (jObj["allUnit"][heroID]["q"]["qn"][i]["q"].ToString().Contains("700"))
                    {
                        jObj["allUnit"][heroID]["q"]["qn"][i].Remove();
                        i--;
                    }
                }
                foreach (string id in hero.NiTianGaiMing)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        ((JArray)jObj["allUnit"][heroID]["q"]["qn"]).Add(JObject.Parse("{\"q\":" + id + ",\"w\":-1,\"e\":80,\"t\":{\"allObject\":{\"\":{\"NewCreateLuck\":\"1\"}}}}"));
                    }
                }
            }
            catch { throw; }
        }
        public void SaveHeroShuXing(JObject jObj, string heroID, Hero hero)
        {
            try
            {
                IsBusy = true;
                if (hero.IsZhuJue)
                    SaveNiTianGaiMing(jObj, heroID, hero);
                else
                {
                    string daoxin = "{";
                    int index = 2;
                    for (int i = 0; i < ((JArray)jObj["allUnit"][heroID]["q"]["qn"]).Count; i++)
                    {
                        if (jObj["allUnit"][heroID]["q"]["qn"][i]["q"].ToString().Contains("700"))
                        {
                            jObj["allUnit"][heroID]["q"]["qn"][i].Remove();
                            i--;
                        }
                    }
                    foreach (string id in hero.DaoXin)
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            daoxin = daoxin + "\"" + index + "\": {\"quality\": 5,\"luck\": " + id + "}" + ",";
                            ((JArray)jObj["allUnit"][heroID]["q"]["qn"]).Add(JObject.Parse("{\"q\":" + id + ",\"w\":-1,\"e\":80,\"t\":{\"allObject\":{\"\":{\"NewCreateLuck\":\"1\"}}}}"));
                            index++;
                        }
                    }
                    if (!string.IsNullOrEmpty(daoxin) && daoxin[daoxin.Length - 1] != '{')
                        daoxin = daoxin.Remove(daoxin.Length - 1, 1) + "}";
                    else
                        daoxin = daoxin + "}";
                    jObj["allUnit"][heroID]["qr"] = JObject.Parse(daoxin);
                }
                ((JArray)jObj["allUnit"][heroID]["q"]["qv"]).Clear();
                foreach (string id in hero.XingQu)
                {
                    ((JArray)jObj["allUnit"][heroID]["q"]["qv"]).Add(id);
                }
                ((JArray)jObj["allUnit"][heroID]["q"]["qb"]).Clear();
                foreach (string id in hero.XianTianQiYun)
                {
                    ((JArray)jObj["allUnit"][heroID]["q"]["qb"]).Add(JObject.Parse("{\"q\":" + id + ",\"w\":-1,\"e\":80,\"t\":{\"allObject\":{\"\":{\"NewCreateLuck\":\"1\"}}}}"));
                }

                jObj["allUnit"][heroID]["q"]["q"][0] = hero.FirstName;
                jObj["allUnit"][heroID]["q"]["q"][1] = hero.LastName;
                jObj["allUnit"][heroID]["q"]["f"] = hero.NeiZaiXingGe;
                jObj["allUnit"][heroID]["q"]["g"] = hero.WaiZaiXingGe;
                jObj["allUnit"][heroID]["q"]["h"] = hero.WaiZaiXingGe2;
                jObj["allUnit"][heroID]["q"]["y"] = Math.Truncate(double.Parse(hero.NianLing) * 12).ToString();
                jObj["allUnit"][heroID]["q"]["u"] = Math.Truncate(double.Parse(hero.NianLingSX) * 12).ToString();
                jObj["allUnit"][heroID]["q"]["qx"] = hero.XinQing;
                jObj["allUnit"][heroID]["q"]["qi"] = hero.FengLingGen;
                jObj["allUnit"][heroID]["q"]["qc"] = hero.XinQingSX;
                jObj["allUnit"][heroID]["q"]["l"] = hero.JianKang;
                jObj["allUnit"][heroID]["q"]["z"] = hero.JianKangSX;
                jObj["allUnit"][heroID]["q"]["5"] = hero.JingLi;
                jObj["allUnit"][heroID]["q"]["6"] = hero.JingLiSX;
                jObj["allUnit"][heroID]["q"]["x"] = hero.TiLi;
                jObj["allUnit"][heroID]["q"]["c"] = hero.TiLiSX;
                jObj["allUnit"][heroID]["q"]["v"] = hero.LingLi;
                jObj["allUnit"][heroID]["q"]["b"] = hero.LingLiSX;
                jObj["allUnit"][heroID]["q"]["n"] = hero.NianLi;
                jObj["allUnit"][heroID]["q"]["m"] = hero.NianLiSX;
                jObj["allUnit"][heroID]["q"]["4"] = hero.XingYun;
                jObj["allUnit"][heroID]["q"]["k"] = hero.WuXing;
                jObj["allUnit"][heroID]["q"]["i"] = hero.MeiLi;
                jObj["allUnit"][heroID]["q"]["a"] = hero.ShengWang;
                jObj["allUnit"][heroID]["q"]["1"] = hero.GongJi;
                jObj["allUnit"][heroID]["q"]["2"] = hero.FangYu;
                jObj["allUnit"][heroID]["q"]["3"] = hero.JiaoLi;
                jObj["allUnit"][heroID]["q"]["qe"] = hero.GongFaKangXing;
                jObj["allUnit"][heroID]["q"]["qr"] = hero.LingGenKangXing;
                jObj["allUnit"][heroID]["q"]["7"] = hero.HuiXin;
                jObj["allUnit"][heroID]["q"]["9"] = hero.HuXin;
                jObj["allUnit"][heroID]["q"]["qw"] = hero.YiSu;
                jObj["allUnit"][heroID]["q"]["8"] = hero.BaoJiBeiShu;
                jObj["allUnit"][heroID]["q"]["qq"] = hero.KangBaoBeiShu;
                jObj["allUnit"][heroID]["q"]["j"] = hero.DaoDian;
                jObj["allUnit"][heroID]["q"]["qd"] = hero.DaoFa;
                jObj["allUnit"][heroID]["q"]["qs"] = hero.QiangFa;
                jObj["allUnit"][heroID]["q"]["qa"] = hero.JianFa;
                jObj["allUnit"][heroID]["q"]["qf"] = hero.QuanFa;
                jObj["allUnit"][heroID]["q"]["qg"] = hero.ZhangFa;
                jObj["allUnit"][heroID]["q"]["qh"] = hero.ZhiFa;
                jObj["allUnit"][heroID]["q"]["qt"] = hero.HuoLingGen;
                jObj["allUnit"][heroID]["q"]["qy"] = hero.ShuiLingGen;
                jObj["allUnit"][heroID]["q"]["qu"] = hero.LeiLingGen;
                jObj["allUnit"][heroID]["q"]["qo"] = hero.TuLingGen;
                jObj["allUnit"][heroID]["q"]["qp"] = hero.MuLingGen;
                jObj["allUnit"][heroID]["q"]["qj"] = hero.LianDan;
                jObj["allUnit"][heroID]["q"]["qk"] = hero.LianQi;
                jObj["allUnit"][heroID]["q"]["ql"] = hero.FengShui;
                jObj["allUnit"][heroID]["q"]["qz"] = hero.HuaFu;
                jObj["allUnit"][heroID]["q"]["wq"] = hero.YaoCai;
                jObj["allUnit"][heroID]["q"]["ww"] = hero.KuangCai;
                jObj["allUnit"][heroID]["q"]["o"] = hero.ZhengDao;
                jObj["allUnit"][heroID]["q"]["p"] = hero.MoDao;
                if (hero.IsTianJiao)
                    jObj["allUnit"][heroID]["qe"]["u"] = 2;
            }
            catch { throw; }
            finally { IsBusy = false; }
        }
        public void UpdateHero(JObject jObj, List<Hero> list, string oldHeroID, string newHeroID, string newSchoolID)
        {
            try
            {
                if (!string.IsNullOrEmpty(oldHeroID))
                    jObj["allUnit"][oldHeroID]["u"] = "";
                if (!string.IsNullOrEmpty(newHeroID))
                    jObj["allUnit"][newHeroID]["u"] = newSchoolID;

                foreach (Hero item in list)
                {
                    if (item.ID == oldHeroID)
                        item.ZongMenID = "";
                    if (item.ID == newHeroID)
                        item.ZongMenID = newSchoolID;
                }
            }
            catch (Exception e) { throw e; }
        }
        public void ChangedSchoolUnit(JObject jObj, School school, string oldHeroID, string heroID, int zhiwei)
        {
            try
            {
                if (Tools.GetZhiWei(school, oldHeroID) == 0) return;
                if (Tools.GetZhiWei(school, oldHeroID) == 1)
                {
                    ClearZongZhu(jObj, school, oldHeroID);
                    AddZongZhu(jObj, school, heroID, 0);
                }
                if (Tools.GetZhiWei(school, oldHeroID) == 2)
                {
                    AddDaZhangLao(jObj, school, heroID, ClearDaZhangLao(jObj, school, oldHeroID));
                }
                if (Tools.GetZhiWei(school, oldHeroID) == 3)
                {
                    AddZhangLao(jObj, school, heroID, ClearZhangLao(jObj, school, oldHeroID));
                }
                if (Tools.GetZhiWei(school, oldHeroID) == 4)
                {
                    AddZhenChuan(jObj, school, heroID, ClearZhenChuan(jObj, school, oldHeroID));
                }
                if (Tools.GetZhiWei(school, oldHeroID) == 5)
                {
                    AddNeiMen(jObj, school, heroID, ClearNeiMen(jObj, school, oldHeroID));
                }
                if (Tools.GetZhiWei(school, oldHeroID) == 6)
                {
                    AddWaiMen(jObj, school, heroID, ClearWaiMen(jObj, school, oldHeroID));
                }
            }
            catch { throw; }
        }
        public void AddWaiMen(JObject jObj, School school, string heroID, int zhiwei)
        {
            try
            {
                ((JArray)jObj["allBuild"][school.ID]["npcOut"]).Add(heroID);
                ((JArray)jObj["allBuild"][school.ID]["postData"]["postElders"][Tools.GetTangKou(zhiwei)]["manageOut"]).Add(heroID);

            }
            catch  { throw ; }
        }
        public void AddNeiMen(JObject jObj, School school, string heroID, int zhiwei)
        {
            try
            {
                ((JArray)jObj["allBuild"][school.ID]["npcIn"]).Add(heroID);
                ((JArray)jObj["allBuild"][school.ID]["postData"]["postElders"][Tools.GetTangKou(zhiwei)]["manageIn"]).Add(heroID);

            }
            catch  { throw ; }
        }
        public void AddZhenChuan(JObject jObj, School school, string heroID, int zhiwei)
        {
            try
            {
                ((JArray)jObj["allBuild"][school.ID]["npcInherit"]).Add(heroID);
                ((JArray)jObj["allBuild"][school.ID]["postData"]["postElders"][Tools.GetTangKou(zhiwei)]["manageInherit"]).Add(heroID);

            }
            catch  { throw ; }
        }
        public void AddZhangLao(JObject jObj, School school, string heroID, int zhiwei)
        {
            try
            {
                string oldHeroID = jObj["allBuild"][school.ID]["postData"]["postElders"][Tools.GetTangKou(zhiwei)]["unitData"]["unitID"].ToString();

                if (!string.IsNullOrEmpty(oldHeroID))
                {
                    ClearZhangLao(jObj, school, oldHeroID);
                    AddZhenChuan(jObj, school, oldHeroID, zhiwei);
                }

                int zhiwei2 = 0;
                if (zhiwei == 0) zhiwei2 = 0;
                if (zhiwei == 1) zhiwei2 = 1;
                if (zhiwei == 2) zhiwei2 = 3;
                if (zhiwei == 3) zhiwei2 = 4;
                if (zhiwei == 4) zhiwei2 = 2;

                ((JArray)jObj["allBuild"][school.ID]["npcElders"]).Add(heroID);
                jObj["allBuild"][school.ID]["postData"]["postElders"][Tools.GetTangKou(zhiwei)]["unitData"]["unitID"] = heroID;
                jObj["allBuild"][school.ID]["postData"]["postElders"][Tools.GetTangKou(zhiwei)]["unitData"]["agentUnitID"] = "";
                JObject obj = JsonConvert.DeserializeObject<JObject>(jObj["allBuild"][school.ID]["allBuildSub"]["1003"]["objData"]["allObject"][""]["data"].ToString());
                ((JArray)obj["departmentDatas"])[zhiwei2]["elderUnitID"] = heroID;
                jObj["allBuild"][school.ID]["allBuildSub"]["1003"]["objData"]["allObject"][""]["data"] = obj.ToString().Replace("\r\n", "").Replace(" ", "");
            }
            catch { throw; }
        }
        public void AddDaZhangLao(JObject jObj, School school, string heroID, int zhiwei)
        {
            try
            {
                string oldHeroID = jObj["allBuild"][school.ID]["npcBigElders"][zhiwei].ToString();

                if (!string.IsNullOrEmpty(oldHeroID))
                {
                    ClearDaZhangLao(jObj, school, oldHeroID);
                    AddZhenChuan(jObj, school, oldHeroID, zhiwei);
                }

                jObj["allBuild"][school.ID]["npcBigElders"][zhiwei] = heroID;

                int index = 0;
                foreach (JObject value in jObj["allBuild"][school.ID]["postData"]["postBigElders"])
                {
                    if (index == zhiwei)
                    {
                        value["unitData"]["unitID"] = heroID;
                        value["unitData"]["agentUnitID"] = "";
                        break;
                    }
                    index++;
                }
            }
            catch { throw; }
        }
        public void AddZongZhu(JObject jObj, School school, string heroID, int zhiwei)
        {
            try
            {
                string oldHeroID = jObj["allBuild"][school.ID]["npcSchoolMain"].ToString();

                if (!string.IsNullOrEmpty(oldHeroID))
                {
                    ClearZongZhu(jObj, school, oldHeroID);
                    AddZhenChuan(jObj, school, oldHeroID, zhiwei);
                }

                foreach (string value in GetSchoolBranch(jObj, school))
                {
                    jObj["allBuild"][value]["npcSchoolMain"] = heroID;
                    jObj["allBuild"][value]["postData"]["postMain"]["unitData"]["unitID"] = heroID;
                    jObj["allBuild"][value]["postData"]["postMain"]["unitData"]["agentUnitID"] = "";
                }

                jObj["allBuild"][school.ID]["npcSchoolMain"] = heroID;
                jObj["allBuild"][school.ID]["postData"]["postMain"]["unitData"]["unitID"] = heroID;
                jObj["allBuild"][school.ID]["postData"]["postMain"]["unitData"]["agentUnitID"] = "";

            }
            catch { throw; }
        }
        public void ClearZongZhu(JObject jObj, School school, string heroID)
        {
            try
            {
                //先把分舵的宗主清除
                foreach (string value in GetSchoolBranch(jObj, school))
                {
                    if (jObj["allBuild"][value]["npcSchoolMain"].ToString() == heroID)
                        jObj["allBuild"][value]["npcSchoolMain"] = "";

                    if (Equals(jObj["allBuild"][value]["postData"]["postMain"]["unitData"]["agentUnitID"].ToString(), heroID))
                    {
                        jObj["allBuild"][value]["postData"]["postMain"]["unitData"]["agentUnitID"] = "";
                    }
                    if (Equals(jObj["allBuild"][value]["postData"]["postMain"]["unitData"]["unitID"].ToString(), heroID))
                    {
                        jObj["allBuild"][value]["postData"]["postMain"]["unitData"]["unitID"] = "";
                    }
                }

                if (jObj["allBuild"][school.ID]["npcSchoolMain"].ToString() == heroID)
                    jObj["allBuild"][school.ID]["npcSchoolMain"] = "";

                if (Equals(jObj["allBuild"][school.ID]["postData"]["postMain"]["unitData"]["agentUnitID"].ToString(), heroID))
                {
                    jObj["allBuild"][school.ID]["postData"]["postMain"]["unitData"]["agentUnitID"] = "";
                }
                if (Equals(jObj["allBuild"][school.ID]["postData"]["postMain"]["unitData"]["unitID"].ToString(), heroID))
                {
                    jObj["allBuild"][school.ID]["postData"]["postMain"]["unitData"]["unitID"] = "";
                }
            }
            catch { throw; }
        }
        public int ClearDaZhangLao(JObject jObj, School school, string heroID)
        {
            try
            {
                int index = 0;
                for (int i = 0; i < ((JArray)jObj["allBuild"][school.ID]["npcBigElders"]).Count; i++)
                {
                    if (jObj["allBuild"][school.ID]["npcBigElders"][i].ToString() == heroID)
                    {
                        jObj["allBuild"][school.ID]["npcBigElders"][i] = "";
                        index = i;
                        break;
                    }
                }

                foreach (JObject value in jObj["allBuild"][school.ID]["postData"]["postBigElders"])
                {
                    if (Equals(value["unitData"]["unitID"].ToString(), heroID))
                    {
                        value["unitData"]["unitID"] = "";
                    }
                    if (Equals(value["unitData"]["agentUnitID"].ToString(), heroID))
                    {
                        value["unitData"]["agentUnitID"] = "";
                    }
                }
                return index;
            }
            catch  { throw ; }
        }
        public int ClearZhangLao(JObject jObj, School school, string heroID)
        {
            try
            {
                int index = -1;  //用于替换长老时记录原长老职位索引
                foreach (JValue value in jObj["allBuild"][school.ID]["npcElders"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        value.Remove();
                        break;
                    }
                }

                if (Equals(jObj["allBuild"][school.ID]["postData"]["postElders"]["Personnel"]["unitData"]["unitID"].ToString(), heroID))
                {
                    index = 0;
                    jObj["allBuild"][school.ID]["postData"]["postElders"]["Personnel"]["unitData"]["unitID"] = "";
                }
                if (Equals(jObj["allBuild"][school.ID]["postData"]["postElders"]["Personnel"]["unitData"]["agentUnitID"].ToString(), heroID))
                {
                    jObj["allBuild"][school.ID]["postData"]["postElders"]["Personnel"]["unitData"]["agentUnitID"] = "";
                }
                if (Equals(jObj["allBuild"][school.ID]["postData"]["postElders"]["Security"]["unitData"]["unitID"].ToString(), heroID))
                {
                    index = 1;
                    jObj["allBuild"][school.ID]["postData"]["postElders"]["Security"]["unitData"]["unitID"] = "";
                }
                if (Equals(jObj["allBuild"][school.ID]["postData"]["postElders"]["Security"]["unitData"]["agentUnitID"].ToString(), heroID))
                {
                    jObj["allBuild"][school.ID]["postData"]["postElders"]["Security"]["unitData"]["agentUnitID"] = "";
                }
                if (Equals(jObj["allBuild"][school.ID]["postData"]["postElders"]["Diplomacy"]["unitData"]["unitID"].ToString(), heroID))
                {
                    index = 4;
                    jObj["allBuild"][school.ID]["postData"]["postElders"]["Diplomacy"]["unitData"]["unitID"] = "";
                }
                if (Equals(jObj["allBuild"][school.ID]["postData"]["postElders"]["Diplomacy"]["unitData"]["agentUnitID"].ToString(), heroID))
                {
                    jObj["allBuild"][school.ID]["postData"]["postElders"]["Diplomacy"]["unitData"]["agentUnitID"] = "";
                }
                if (Equals(jObj["allBuild"][school.ID]["postData"]["postElders"]["Constructor"]["unitData"]["unitID"].ToString(), heroID))
                {
                    index = 2;
                    jObj["allBuild"][school.ID]["postData"]["postElders"]["Constructor"]["unitData"]["unitID"] = "";
                }
                if (Equals(jObj["allBuild"][school.ID]["postData"]["postElders"]["Constructor"]["unitData"]["agentUnitID"].ToString(), heroID))
                {
                    jObj["allBuild"][school.ID]["postData"]["postElders"]["Constructor"]["unitData"]["agentUnitID"] = "";
                }
                if (Equals(jObj["allBuild"][school.ID]["postData"]["postElders"]["Train"]["unitData"]["unitID"].ToString(), heroID))
                {
                    index = 3;
                    jObj["allBuild"][school.ID]["postData"]["postElders"]["Train"]["unitData"]["unitID"] = "";
                }
                if (Equals(jObj["allBuild"][school.ID]["postData"]["postElders"]["Train"]["unitData"]["agentUnitID"].ToString(), heroID))
                {
                    jObj["allBuild"][school.ID]["postData"]["postElders"]["Train"]["unitData"]["agentUnitID"] = "";
                }

                JObject obj = JsonConvert.DeserializeObject<JObject>(jObj["allBuild"][school.ID]["allBuildSub"]["1003"]["objData"]["allObject"][""]["data"].ToString());
                jObj["allBuild"][school.ID]["allBuildSub"]["1003"]["objData"]["allObject"][""]["data"] = obj.ToString().Replace(heroID, "").Replace("\r\n", "").Replace(" ", "");

                return index;
            }
            catch  { throw ; }
        }
        public int ClearZhenChuan(JObject jObj, School school, string heroID)
        {
            try
            {
                int index = -1;
                foreach (JValue value in jObj["allBuild"][school.ID]["npcInherit"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        value.Remove();
                        break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Personnel"]["manageInherit"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 0;
                        value.Remove(); break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Security"]["manageInherit"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 1;
                        value.Remove(); break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Diplomacy"]["manageInherit"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 4;
                        value.Remove(); break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Constructor"]["manageInherit"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 2;
                        value.Remove(); break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Train"]["manageInherit"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 3;
                        value.Remove(); break;
                    }
                }

                return index;
            }
            catch  { throw ; }
        }
        public int ClearNeiMen(JObject jObj, School school, string heroID)
        {
            try
            {
                int index = -1;
                foreach (JValue value in jObj["allBuild"][school.ID]["npcIn"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        value.Remove();
                        break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Personnel"]["manageIn"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 0;
                        value.Remove(); break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Security"]["manageIn"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 1;
                        value.Remove(); break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Diplomacy"]["manageIn"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 4;
                        value.Remove(); break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Constructor"]["manageIn"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 2;
                        value.Remove(); break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Train"]["manageIn"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 3;
                        value.Remove(); break;
                    }
                }

                return index;
            }
            catch  { throw ; }
        }
        public int ClearWaiMen(JObject jObj, School school, string heroID)
        {
            try
            {
                int index = -1;
                foreach (JValue value in jObj["allBuild"][school.ID]["npcOut"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        value.Remove();
                        break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Personnel"]["manageOut"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 0;
                        value.Remove();
                        break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Security"]["manageOut"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 1;
                        value.Remove(); break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Diplomacy"]["manageOut"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 4;
                        value.Remove(); break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Constructor"]["manageOut"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 2;
                        value.Remove(); break;
                    }
                }
                foreach (JValue value in jObj["allBuild"][school.ID]["postData"]["postElders"]["Train"]["manageOut"])
                {
                    if (Equals(value.ToString(), heroID))
                    {
                        index = 3;
                        value.Remove(); break;
                    }
                }

                return index;
            }
            catch  { throw ; }
        }
        public void ReviveSchoolUnit(JObject jObj, List<Hero> list)
        {
            try
            {
                IsBusy = true;

                foreach (Hero hero in list)
                {
                    foreach (JProperty item in jObj["allUnit"])
                    {
                        if (item.Value.Path.Split('.')[1] == hero.ID)
                        {
                            for (int i = 0; i < ((JContainer)item.Value["q"]["qn"]).Count; i++)
                            {
                                if (item.Value["q"]["qn"][i]["q"].ToString() == "1012" ||
                                    item.Value["q"]["qn"][i]["q"].ToString() == "1013" ||
                                    item.Value["q"]["qn"][i]["q"].ToString() == "1011" ||
                                    item.Value["q"]["qn"][i]["q"].ToString() == "101")
                                {
                                    item.Value["q"]["qn"][i].Remove();
                                    break;
                                }
                            }
                            break;
                        }
                    } 
                }
            }
            catch { throw; }
            finally { IsBusy = false; }
        }
        public List<Hero> GetSchoolYiSiYuanHun(JObject jObj, string schoolID)
        {
            try
            {
                IsBusy = true;
                List<Hero> list = new List<Hero>();
                foreach (JProperty item in jObj["allUnit"])
                {
                    foreach (JObject array in item.Value["q"]["qn"])
                    {
                        if ((array["q"].ToString() == "1012" || array["q"].ToString() == "1011" || array["q"].ToString() == "101" || array["q"].ToString() == "1013") &&
                            item.Value["u"].ToString() == schoolID)
                        {
                            Hero hero = new Hero();
                            //hero.ID = item.Value.Path.Split('.')[1];
                            hero.FirstName = item.Value["q"]["q"][0].ToString();
                            hero.LastName = item.Value["q"]["q"][1].ToString();
                            hero.JingJie = item.Value["q"]["d"].ToString();
                            list.Add(hero);
                        }
                    }
                }
                return list;
            }
            catch { throw; }
            finally { IsBusy = false; }
        }
        public void SaveSchoolShuXing(JObject jObj, School school, List<School> list)
        {
            try
            {
                IsBusy = true;
                jObj["allBuild"][school.ID]["money"] = int.Parse(school.LingShi);
                jObj["allBuild"][school.ID]["totalMember"] = int.Parse(school.JiMingDiZi);
                jObj["allBuild"][school.ID]["reputation"] = int.Parse(school.ZongMenRongYu);
                jObj["allBuild"][school.ID]["propertyData"]["prosperous"] = int.Parse(school.FanRong);
                jObj["allBuild"][school.ID]["propertyData"]["loyal"] = int.Parse(school.ZhongCheng);
                jObj["allBuild"][school.ID]["manorData"]["mainManor"]["stable"] = int.Parse(school.AnDing);
                jObj["allBuild"][school.ID]["manorData"]["mainManor"]["Stable"] = int.Parse(school.AnDing);
                jObj["allBuild"][school.ID]["propertyData"]["mine"] = int.Parse(school.KuangCai);
                jObj["allBuild"][school.ID]["propertyData"]["medicina"] = int.Parse(school.YaoCai);

                foreach (School item in list)
                {
                    if (Equals(item.ID, school.ID))
                    {
                        item.LingShi = school.LingShi;
                        item.JiMingDiZi = school.JiMingDiZi;
                        item.ZongMenRongYu = school.ZongMenRongYu;
                        item.FanRong = school.FanRong;
                        item.ZhongCheng = school.ZhongCheng;
                        item.AnDing = school.AnDing;
                        item.KuangCai = school.KuangCai;
                        item.YaoCai = school.YaoCai;
                    }
                }
            }
            catch { throw; }
            finally { IsBusy = false; }
        }
        public void SaveGuanXi(JObject jObj, int parentID, string parentHeroID, string heroID, string haogan = "0")
        {
            try
            {
                if (parentID == 1)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["w"]).Add(heroID);
                    ((JArray)jObj["allUnit"][heroID]["x"]["q"])[0] = parentHeroID;
                    ((JArray)jObj["allUnit"][heroID]["x"]["q"])[1] = "";
                }
                if (parentID == 2)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["e"]).Add(heroID);
                    ((JArray)jObj["allUnit"][heroID]["x"]["q"])[0] = parentHeroID;
                    ((JArray)jObj["allUnit"][heroID]["x"]["q"])[1] = "";
                }
                if (parentID == 3)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["r"]).Add(heroID);
                    ((JArray)jObj["allUnit"][heroID]["x"]["r"]).Add(parentHeroID);
                }
                if (parentID == 4)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["t"]).Add(heroID);
                    ((JArray)jObj["allUnit"][heroID]["x"]["y"]).Add(parentHeroID);
                }
                if (parentID == 5)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["y"]).Add(heroID);
                    ((JArray)jObj["allUnit"][heroID]["x"]["t"]).Add(parentHeroID);
                }
                if (parentID == 6)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["u"]).Add(heroID);
                    ((JArray)jObj["allUnit"][heroID]["x"]["u"]).Add(parentHeroID);
                }
                if (parentID == 8)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["o"]).Add(heroID);
                    ((JArray)jObj["allUnit"][heroID]["x"]["o"]).Add(parentHeroID);
                }
                if (parentID == 9)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["p"]).Add(heroID);
                    ((JArray)jObj["allUnit"][heroID]["x"]["a"]).Add(parentHeroID);
                }
                if (parentID == 10)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["a"]).Add(heroID);
                    ((JArray)jObj["allUnit"][heroID]["x"]["p"]).Add(parentHeroID);
                }

                bool isHave = false;
                foreach (JToken token in jObj["allUnit"][parentHeroID]["x"]["d"])
                {
                    if (token.ToString().Contains(heroID))
                    {
                        jObj["allUnit"][parentHeroID]["x"]["d"][heroID] = double.Parse(haogan);
                        isHave = true;
                        break;
                    }
                }
                if (!isHave)
                    ((JObject)jObj["allUnit"][parentHeroID]["x"]["d"]).Add(heroID, double.Parse(haogan));
            }
            catch { throw; }
        }
        public void ChangedGuanxi(JObject jObj, int parentID, string parentHeroID, string oldHeroID, string heroID, int index)
        {
            try
            {
                if (parentID == 0)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["q"])[index] = heroID;
                    ((JArray)jObj["allUnit"][heroID]["x"]["w"]).Add(parentHeroID);
                    if (!string.IsNullOrEmpty(oldHeroID))
                        foreach (JValue value in jObj["allUnit"][oldHeroID]["x"]["w"])
                        {
                            if (value.ToString() == parentHeroID)
                            {
                                value.Remove();
                                break;
                            }
                        }
                }
                if (parentID == 1)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["w"])[index] = heroID;
                    ((JArray)jObj["allUnit"][heroID]["x"]["q"])[0] = parentHeroID;
                    if (!string.IsNullOrEmpty(oldHeroID))
                        for (int i = 0; i < ((JArray)jObj["allUnit"][oldHeroID]["x"]["q"]).Count; i++)
                        {
                            if (((JArray)jObj["allUnit"][oldHeroID]["x"]["q"])[i].ToString() == parentHeroID)
                            {
                                ((JArray)jObj["allUnit"][oldHeroID]["x"]["q"])[i] = "";
                            }
                        }
                }
                if (parentID == 2)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["e"])[index] = heroID;
                    ((JArray)jObj["allUnit"][heroID]["x"]["q"])[0] = parentHeroID;
                    if (!string.IsNullOrEmpty(oldHeroID))
                        for (int i = 0; i < ((JArray)jObj["allUnit"][oldHeroID]["x"]["q"]).Count; i++)
                        {
                            if (((JArray)jObj["allUnit"][oldHeroID]["x"]["q"])[i].ToString() == parentHeroID)
                            {
                                ((JArray)jObj["allUnit"][oldHeroID]["x"]["q"])[i] = "";
                            }
                        }
                }
                if (parentID == 3)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["r"])[index] = heroID;
                    ((JArray)jObj["allUnit"][heroID]["x"]["r"]).Add(parentHeroID);
                    if (!string.IsNullOrEmpty(oldHeroID))
                        for (int i = 0; i < ((JArray)jObj["allUnit"][oldHeroID]["x"]["r"]).Count; i++)
                        {
                            if (((JArray)jObj["allUnit"][oldHeroID]["x"]["r"])[i].ToString() == parentHeroID)
                            {
                                ((JArray)jObj["allUnit"][oldHeroID]["x"]["r"])[i].Remove();
                            }
                        }
                }
                if (parentID == 4)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["t"])[index] = heroID;
                    ((JArray)jObj["allUnit"][heroID]["x"]["y"]).Add(parentHeroID);
                    if (!string.IsNullOrEmpty(oldHeroID))
                        for (int i = 0; i < ((JArray)jObj["allUnit"][oldHeroID]["x"]["y"]).Count; i++)
                        {
                            if (((JArray)jObj["allUnit"][oldHeroID]["x"]["y"])[i].ToString() == parentHeroID)
                            {
                                ((JArray)jObj["allUnit"][oldHeroID]["x"]["y"])[i].Remove();
                            }
                        }
                }
                if (parentID == 5)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["y"])[index] = heroID;
                    ((JArray)jObj["allUnit"][heroID]["x"]["t"]).Add(parentHeroID);
                    if (!string.IsNullOrEmpty(oldHeroID))
                        for (int i = 0; i < ((JArray)jObj["allUnit"][oldHeroID]["x"]["t"]).Count; i++)
                        {
                            if (((JArray)jObj["allUnit"][oldHeroID]["x"]["t"])[i].ToString() == parentHeroID)
                            {
                                ((JArray)jObj["allUnit"][oldHeroID]["x"]["t"])[i].Remove();
                            }
                        }
                }
                if (parentID == 6)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["u"])[index] = heroID;
                    ((JArray)jObj["allUnit"][heroID]["x"]["u"]).Add(parentHeroID);
                    if (!string.IsNullOrEmpty(oldHeroID))
                        for (int i = 0; i < ((JArray)jObj["allUnit"][oldHeroID]["x"]["u"]).Count; i++)
                        {
                            if (((JArray)jObj["allUnit"][oldHeroID]["x"]["u"])[i].ToString() == parentHeroID)
                            {
                                ((JArray)jObj["allUnit"][oldHeroID]["x"]["u"])[i].Remove();
                            }
                        }
                }
                if (parentID == 7)
                {
                    jObj["allUnit"][parentHeroID]["x"]["i"] = heroID;
                    jObj["allUnit"][heroID]["x"]["i"] = parentHeroID;
                    if (!string.IsNullOrEmpty(oldHeroID))
                        jObj["allUnit"][oldHeroID]["x"]["i"] = "";
                }
                if (parentID == 8)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["o"])[index] = heroID;
                    ((JArray)jObj["allUnit"][heroID]["x"]["o"]).Add(parentHeroID);
                    if (!string.IsNullOrEmpty(oldHeroID))
                        for (int i = 0; i < ((JArray)jObj["allUnit"][oldHeroID]["x"]["o"]).Count; i++)
                        {
                            if (((JArray)jObj["allUnit"][oldHeroID]["x"]["o"])[i].ToString() == parentHeroID)
                            {
                                ((JArray)jObj["allUnit"][oldHeroID]["x"]["o"])[i].Remove();
                            }
                        }
                }
                if (parentID == 9)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["p"])[index] = heroID;
                    ((JArray)jObj["allUnit"][heroID]["x"]["a"]).Add(parentHeroID);
                    if (!string.IsNullOrEmpty(oldHeroID))
                        for (int i = 0; i < ((JArray)jObj["allUnit"][oldHeroID]["x"]["a"]).Count; i++)
                        {
                            if (((JArray)jObj["allUnit"][oldHeroID]["x"]["a"])[i].ToString() == parentHeroID)
                            {
                                ((JArray)jObj["allUnit"][oldHeroID]["x"]["a"])[i].Remove();
                            }
                        }
                }
                if (parentID == 10)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["a"])[index] = heroID;
                    ((JArray)jObj["allUnit"][heroID]["x"]["p"]).Add(parentHeroID);
                    if (!string.IsNullOrEmpty(oldHeroID))
                        for (int i = 0; i < ((JArray)jObj["allUnit"][oldHeroID]["x"]["p"]).Count; i++)
                        {
                            if (((JArray)jObj["allUnit"][oldHeroID]["x"]["p"])[i].ToString() == parentHeroID)
                            {
                                ((JArray)jObj["allUnit"][oldHeroID]["x"]["p"])[i].Remove();
                            }
                        }
                }
            }
            catch
            { throw; }
        }
        public void ChangedHaoGan(JObject jObj, int parentID, string parentHeroID, string heroID, string haogan)
        {
            try
            {
                if (parentID == 12)
                {
                    jObj["allUnit"][parentHeroID]["x"]["d"][heroID] = double.Parse(haogan);
                }
                if (parentID == 11)
                {
                    jObj["allUnit"][parentHeroID]["x"]["s"] = double.Parse(haogan);
                }
            }
            catch
            { throw; }
        }
        public void DelGuanXi(JObject jObj, int parentID, string parentHeroID, string heroID, int index)
        {
            try
            {
                if (parentID == 0)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["q"])[index] = "";
                    for (int i = 0; i < ((JArray)jObj["allUnit"][heroID]["x"]["w"]).Count; i++)
                    {
                        if (((JArray)jObj["allUnit"][heroID]["x"]["w"])[i].ToString() == parentHeroID)
                        {
                            ((JArray)jObj["allUnit"][heroID]["x"]["w"])[i].Remove();
                        }
                    }
                }
                if (parentID == 1)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["w"])[index].Remove();
                    for (int i = 0; i < ((JArray)jObj["allUnit"][heroID]["x"]["q"]).Count; i++)
                    {
                        if (((JArray)jObj["allUnit"][heroID]["x"]["q"])[i].ToString() == parentHeroID)
                        {
                            ((JArray)jObj["allUnit"][heroID]["x"]["q"])[i] = "";
                        }
                    }
                }
                if (parentID == 2)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["e"])[index].Remove();
                    for (int i = 0; i < ((JArray)jObj["allUnit"][heroID]["x"]["q"]).Count; i++)
                    {
                        if (((JArray)jObj["allUnit"][heroID]["x"]["q"])[i].ToString() == parentHeroID)
                        {
                            ((JArray)jObj["allUnit"][heroID]["x"]["q"])[i] = "";
                        }
                    }
                }
                if (parentID == 3)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["r"])[index].Remove();
                    for (int i = 0; i < ((JArray)jObj["allUnit"][heroID]["x"]["r"]).Count; i++)
                    {
                        if (((JArray)jObj["allUnit"][heroID]["x"]["r"])[i].ToString() == parentHeroID)
                        {
                            ((JArray)jObj["allUnit"][heroID]["x"]["r"])[i].Remove();
                        }
                    }
                }
                if (parentID == 4)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["t"])[index].Remove();
                    for (int i = 0; i < ((JArray)jObj["allUnit"][heroID]["x"]["y"]).Count; i++)
                    {
                        if (((JArray)jObj["allUnit"][heroID]["x"]["y"])[i].ToString() == parentHeroID)
                        {
                            ((JArray)jObj["allUnit"][heroID]["x"]["y"])[i].Remove();
                        }
                    }
                }
                if (parentID == 5)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["y"])[index].Remove();
                    for (int i = 0; i < ((JArray)jObj["allUnit"][heroID]["x"]["t"]).Count; i++)
                    {
                        if (((JArray)jObj["allUnit"][heroID]["x"]["t"])[i].ToString() == parentHeroID)
                        {
                            ((JArray)jObj["allUnit"][heroID]["x"]["t"])[i].Remove();
                        }
                    }
                }
                if (parentID == 6)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["u"])[index].Remove();
                    for (int i = 0; i < ((JArray)jObj["allUnit"][heroID]["x"]["u"]).Count; i++)
                    {
                        if (((JArray)jObj["allUnit"][heroID]["x"]["u"])[i].ToString() == parentHeroID)
                        {
                            ((JArray)jObj["allUnit"][heroID]["x"]["u"])[i].Remove();
                        }
                    }
                }
                if (parentID == 7)
                {
                    jObj["allUnit"][parentHeroID]["x"]["i"] = "";
                    jObj["allUnit"][heroID]["x"]["i"] = "";
                }
                if (parentID == 8)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["o"])[index].Remove();
                    for (int i = 0; i < ((JArray)jObj["allUnit"][heroID]["x"]["o"]).Count; i++)
                    {
                        if (((JArray)jObj["allUnit"][heroID]["x"]["o"])[i].ToString() == parentHeroID)
                        {
                            ((JArray)jObj["allUnit"][heroID]["x"]["o"])[i].Remove();
                        }
                    }
                }
                if (parentID == 9)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["p"])[index].Remove();
                    for (int i = 0; i < ((JArray)jObj["allUnit"][heroID]["x"]["a"]).Count; i++)
                    {
                        if (((JArray)jObj["allUnit"][heroID]["x"]["a"])[i].ToString() == parentHeroID)
                        {
                            ((JArray)jObj["allUnit"][heroID]["x"]["a"])[i].Remove();
                        }
                    }
                }
                if (parentID == 10)
                {
                    ((JArray)jObj["allUnit"][parentHeroID]["x"]["a"])[index].Remove();
                    for (int i = 0; i < ((JArray)jObj["allUnit"][heroID]["x"]["p"]).Count; i++)
                    {
                        if (((JArray)jObj["allUnit"][heroID]["x"]["p"])[i].ToString() == parentHeroID)
                        {
                            ((JArray)jObj["allUnit"][heroID]["x"]["p"])[i].Remove();
                        }
                    }
                }
            }
            catch { throw; }
        }
        public List<School> GetSchoolList(JObject jObj)
        {
            try
            {
                IsBusy = true;
                List<School> list = new List<School>();

                foreach (JProperty item in jObj["allBuild"])
                {
                    string name = string.Empty;
                    bool isMainSchool = false;
                    if (string.IsNullOrEmpty(JsonConvert.DeserializeObject<JObject>(item.Value["values"].First.ToString())["mainSchoolPoint"].ToString()))
                        isMainSchool = true;

                    foreach (JObject obj in item.Value["logs"])
                    {
                        if (obj["id"].ToString() == "2501")
                        {
                            name = obj["values"][1].ToString();
                            break;
                        }
                    }

                    string daZhangLao = string.Empty;
                    string zhangLao = string.Empty;
                    string zhenChuan = string.Empty;
                    string neiMen = string.Empty;
                    string waiMen = string.Empty;

                    foreach (JValue value in item.Value["npcBigElders"])
                    {
                        daZhangLao = daZhangLao + value + ",";
                    }
                    foreach (JValue value in item.Value["npcElders"])
                    {
                        zhangLao = zhangLao + value + ",";
                    }
                    foreach (JValue value in item.Value["npcInherit"])
                    {
                        zhenChuan = zhenChuan + value + ",";
                    }
                    foreach (JValue value in item.Value["npcIn"])
                    {
                        neiMen = neiMen + value + ",";
                    }
                    foreach (JValue value in item.Value["npcOut"])
                    {
                        waiMen = waiMen + value + ",";
                    }

                    if (!string.IsNullOrEmpty(daZhangLao))
                        daZhangLao = daZhangLao.Remove(daZhangLao.Length - 1, 1);
                    if (!string.IsNullOrEmpty(zhangLao))
                        zhangLao = zhangLao.Remove(zhangLao.Length - 1, 1);
                    if (!string.IsNullOrEmpty(zhenChuan))
                        zhenChuan = zhenChuan.Remove(zhenChuan.Length - 1, 1);
                    if (!string.IsNullOrEmpty(neiMen))
                        neiMen = neiMen.Remove(neiMen.Length - 1, 1);
                    if (!string.IsNullOrEmpty(waiMen))
                        waiMen = waiMen.Remove(waiMen.Length - 1, 1);

                    list.Add(new School()
                    {
                        ID = item.Name,
                        Name = name,
                        Name1ID = JsonConvert.DeserializeObject<JObject>(item.Value["values"].First.ToString())["name1ID"].ToString(),
                        Name2ID = JsonConvert.DeserializeObject<JObject>(item.Value["values"].First.ToString())["name2ID"].ToString(),
                        IsMainSchool = isMainSchool,
                        LingShi = item.Value["money"].ToString(),
                        ZongMenRongYu = item.Value["reputation"].ToString(),
                        JiMingDiZi = item.Value["totalMember"].ToString(),
                        FanRong = item.Value["propertyData"]["prosperous"].ToString(),
                        ZhongCheng = item.Value["propertyData"]["loyal"].ToString(),
                        AnDing = item.Value["manorData"]["mainManor"]["stable"].ToString(),
                        YaoCai = item.Value["propertyData"]["medicina"].ToString(),
                        KuangCai = item.Value["propertyData"]["mine"].ToString(),
                        ZongZhu = item.Value["npcSchoolMain"].ToString(),
                        DaZhangLao = daZhangLao.Split(','),
                        ZhangLao = zhangLao.Split(','),
                        ZhenChuan = zhenChuan.Split(','),
                        NeiMen = neiMen.Split(','),
                        WaiMen = waiMen.Split(','),
                    });
                }

                SetSchoolName(jObj, list);

                return list;
            }
            catch { throw; }
            finally
            { IsBusy = false; }
        }
        public List<Hero> GetHeroList(JObject jObj)
        {
            try
            {
                IsBusy = true;
                List<Hero> list = new List<Hero>();
                bool isZhuJue = true;
                foreach (JProperty item in jObj["allUnit"])
                {
                    string[] aihao = new string[] { item.Value["q"]["qv"][0].ToString(), item.Value["q"]["qv"][1].ToString(), item.Value["q"]["qv"][2].ToString() };
                    
                    bool isTianJiao = false;
                    string xiantianqiyun = string.Empty;
                    string nitiangaiming = string.Empty;
                    string daoxin = string.Empty;
                    string lingshi = string.Empty;

                    if (item.Value["qe"]["u"].ToString() == "2")
                        isTianJiao = true;

                    foreach (JObject value in item.Value["q"]["qb"])
                    {
                        xiantianqiyun = xiantianqiyun + value["q"] + ",";
                    }
                    foreach (JObject value in item.Value["q"]["qn"])
                    {
                        if (value["q"].ToString().Contains("700"))
                            nitiangaiming = nitiangaiming + value["q"] + ",";
                    }
                    foreach (JProperty value in item.Value["qr"])
                    {
                        daoxin = daoxin + value.Value["luck"] + ",";
                    }

                    if (!string.IsNullOrEmpty(xiantianqiyun))
                        xiantianqiyun = xiantianqiyun.Remove(xiantianqiyun.Length - 1, 1);
                    if (!string.IsNullOrEmpty(nitiangaiming))
                        nitiangaiming = nitiangaiming.Remove(nitiangaiming.Length - 1, 1);
                    if (!string.IsNullOrEmpty(daoxin))
                        daoxin = daoxin.Remove(daoxin.Length - 1, 1);

                    foreach (JObject o in item.Value["z"]["allProps"])
                    {
                        if (o["w"][1].ToString() == "10001")
                        {
                            lingshi = o["w"][2].ToString();
                        }
                    }

                    list.Add(new Hero()
                    {
                        IsZhuJue = isZhuJue,
                        ID = item.Name,
                        FirstName = item.Value["q"]["q"][0].ToString(),
                        LastName = item.Value["q"]["q"][1].ToString(),
                        IsTianJiao = isTianJiao,
                        NianLing = Math.Truncate((double)item.Value["q"]["y"] / 12).ToString(),
                        NianLingSX = Math.Truncate((double)item.Value["q"]["u"] / 12).ToString(),
                        NeiZaiXingGe = item.Value["q"]["f"].ToString(),
                        WaiZaiXingGe = item.Value["q"]["g"].ToString(),
                        WaiZaiXingGe2 = item.Value["q"]["h"].ToString(),
                        JingJie = item.Value["q"]["d"].ToString(),
                        XianTianQiYun = xiantianqiyun.Split(','),
                        NiTianGaiMing=nitiangaiming.Split(','),
                        DaoXin = daoxin.Split(','),
                        XingQu = aihao,
                        ZongMenID = item.Value["u"].ToString(),
                        XinQing = item.Value["q"]["qx"].ToString(),
                        XinQingSX = item.Value["q"]["qc"].ToString(),
                        JianKang = item.Value["q"]["l"].ToString(),
                        JianKangSX = item.Value["q"]["z"].ToString(),
                        JingLi = item.Value["q"]["5"].ToString(),
                        JingLiSX = item.Value["q"]["6"].ToString(),
                        TiLi = item.Value["q"]["x"].ToString(),
                        TiLiSX = item.Value["q"]["c"].ToString(),
                        LingLi = item.Value["q"]["v"].ToString(),
                        LingLiSX = item.Value["q"]["b"].ToString(),
                        NianLi = item.Value["q"]["n"].ToString(),
                        NianLiSX = item.Value["q"]["m"].ToString(),
                        XingYun = item.Value["q"]["4"].ToString(),
                        WuXing = item.Value["q"]["k"].ToString(),
                        MeiLi = item.Value["q"]["i"].ToString(),
                        FengLingGen = item.Value["q"]["qi"].ToString(),
                        ShengWang = item.Value["q"]["a"].ToString(),
                        GongJi = item.Value["q"]["1"].ToString(),
                        FangYu = item.Value["q"]["2"].ToString(),
                        JiaoLi = item.Value["q"]["3"].ToString(),
                        GongFaKangXing = item.Value["q"]["qe"].ToString(),
                        LingGenKangXing = item.Value["q"]["qr"].ToString(),
                        HuiXin = item.Value["q"]["7"].ToString(),
                        HuXin = item.Value["q"]["9"].ToString(),
                        YiSu = item.Value["q"]["qw"].ToString(),
                        BaoJiBeiShu = item.Value["q"]["8"].ToString(),
                        KangBaoBeiShu = item.Value["q"]["qq"].ToString(),
                        DaoDian = item.Value["q"]["j"].ToString(),
                        DaoFa = item.Value["q"]["qd"].ToString(),
                        QiangFa = item.Value["q"]["qs"].ToString(),
                        JianFa = item.Value["q"]["qa"].ToString(),
                        QuanFa = item.Value["q"]["qf"].ToString(),
                        ZhangFa = item.Value["q"]["qg"].ToString(),
                        ZhiFa = item.Value["q"]["qh"].ToString(),
                        HuoLingGen = item.Value["q"]["qt"].ToString(),
                        ShuiLingGen = item.Value["q"]["qy"].ToString(),
                        LeiLingGen = item.Value["q"]["qu"].ToString(),
                        TuLingGen = item.Value["q"]["qo"].ToString(),
                        MuLingGen = item.Value["q"]["qp"].ToString(),
                        LianDan = item.Value["q"]["qj"].ToString(),
                        LianQi = item.Value["q"]["qk"].ToString(),
                        FengShui = item.Value["q"]["ql"].ToString(),
                        HuaFu = item.Value["q"]["qz"].ToString(),
                        YaoCai = item.Value["q"]["wq"].ToString(),
                        KuangCai = item.Value["q"]["ww"].ToString(),
                        ZhengDao = item.Value["q"]["o"].ToString(),
                        MoDao = item.Value["q"]["p"].ToString(),
                        LingShi = lingshi,
                    });

                    isZhuJue = false;
                }

                return list;
            }
            catch { throw; }
            finally
            { IsBusy = false; }
        }
        public Hero GetHeroGuanXi(JObject jObj, string id, List<Hero> list)
        {
            try
            {
                Hero hero = new Hero();

                foreach (JValue value in (JArray)jObj["allUnit"][id]["x"]["q"])
                {
                    hero.FuMu.Add(new Relation(value.ToString(), Tools.GetName(value.ToString(), list)));
                }
                foreach (JValue value in (JArray)jObj["allUnit"][id]["x"]["w"])
                {
                    hero.ZiNv.Add(new Relation(value.ToString(), Tools.GetName(value.ToString(), list)));
                }
                foreach (JValue value in (JArray)jObj["allUnit"][id]["x"]["e"])
                {
                    hero.SiShengZi.Add(new Relation(value.ToString(), Tools.GetName(value.ToString(), list)));
                }
                foreach (JValue value in (JArray)jObj["allUnit"][id]["x"]["r"])
                {
                    hero.XiongDiJieMei.Add(new Relation(value.ToString(), Tools.GetName(value.ToString(), list)));
                }
                foreach (JValue value in (JArray)jObj["allUnit"][id]["x"]["t"])
                {
                    hero.YiFuMu.Add(new Relation(value.ToString(), Tools.GetName(value.ToString(), list)));
                }
                foreach (JValue value in (JArray)jObj["allUnit"][id]["x"]["y"])
                {
                    hero.YiZiNv.Add(new Relation(value.ToString(), Tools.GetName(value.ToString(), list)));
                }
                foreach (JValue value in (JArray)jObj["allUnit"][id]["x"]["u"])
                {
                    hero.YiXiongDiJieMei.Add(new Relation(value.ToString(), Tools.GetName(value.ToString(), list)));
                }
                hero.FuQi = new Relation(jObj["allUnit"][id]["x"]["i"].ToString(), Tools.GetName(jObj["allUnit"][id]["x"]["i"].ToString(), list));
                foreach (JValue value in (JArray)jObj["allUnit"][id]["x"]["o"])
                {
                    hero.Daolv.Add(new Relation(value.ToString(), Tools.GetName(value.ToString(), list)));
                }
                foreach (JValue value in (JArray)jObj["allUnit"][id]["x"]["p"])
                {
                    hero.ShiFu.Add(new Relation(value.ToString(), Tools.GetName(value.ToString(), list)));
                }
                foreach (JValue value in (JArray)jObj["allUnit"][id]["x"]["a"])
                {
                    hero.TuDi.Add(new Relation(value.ToString(), Tools.GetName(value.ToString(), list)));
                }
                hero.HeroHaoGan = jObj["allUnit"][id]["x"]["s"].ToString();
                foreach (JToken value in jObj["allUnit"][id]["x"]["d"])
                {
                    hero.HaoYou.Add(new Relation(value.ToString().Replace("\"", "").Split(':')[0],
                                               Tools.GetName(value.ToString().Replace("\"", "").Split(':')[0], list),
                                               value.ToString().Replace("\"", "").Replace(" ", "").Split(':')[1]));
                }

                return hero;
            }
            catch { throw; }
        }

        public async Task<bool> SaveJson(JObject jObj, string savePath, string jsonPath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    IsBusy = true;
                    if (jObj != null)
                        Tools.WriteJson(JsonConvert.SerializeObject(jObj), jsonPath);
                    EN.SaveJson(savePath, jsonPath);

                    return true;
                }
                catch { throw; }
                finally { IsBusy = false; }
            });
        }
        public async Task<JObject> LoadingJson(string jsonPath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    IsBusy = true;
                    return JsonConvert.DeserializeObject<JObject>(Tools.GetTxt(jsonPath).ToString());
                }
                catch { throw; }
                finally 
                { IsBusy = false; }
            });
        }

        private void SetSchoolName(JObject jObj, List<School> list)
        {
            try
            {
                foreach (School item in list)
                {
                    if (string.IsNullOrEmpty(item.Name))
                    {
                        string[] array = Tools.GetZongMenName();
                        //name1ID = JsonConvert.DeserializeObject<JObject>(jObj["allBuild"][item.ID]["values"].First.ToString())["name1ID"].ToString();
                        //name2ID = JsonConvert.DeserializeObject<JObject>(jObj["allBuild"][item.ID]["values"].First.ToString())["name2ID"].ToString();
                        if (item.IsMainSchool)
                            item.Name = Tools.GetZongMenName(array[0], item.Name1ID) + Tools.GetZongMenName(array[1], item.Name2ID) + "?";
                        else
                            item.Name = Tools.GetZongMenName(array[0], item.Name1ID) + Tools.GetZongMenName(array[1], item.Name2ID) + "?" + Tools.GetZongMenPoint(((JArray)jObj["allBuild"][item.ID]["points"])[0].ToString()) + "分舵";
                    }
                }

                foreach (School item in list)
                {
                    if (item.Name[2] == '?')
                    {
                        foreach (School item2 in list)
                        {
                            if (item.Name[0] == item2.Name[0] && item.Name[1] == item2.Name[1])
                            {
                                item.Name = item.Name.Replace('?', item2.Name[2]);
                            }
                        }
                    }
                }
            }
            catch { throw; }
        }
        private List<string> GetSchoolBranch(JObject jObj, School school)
        {
            List<string> tmpList = new List<string>();

            if (school.IsMainSchool)
            {
                foreach (JProperty item in jObj["allBuild"])
                {
                    if (!string.IsNullOrEmpty(JsonConvert.DeserializeObject<JObject>(item.Value["values"].First.ToString())["mainSchoolPoint"].ToString()))
                    {
                        if (JsonConvert.DeserializeObject<JObject>(item.Value["values"].First.ToString())["name1ID"].ToString() == school.Name1ID &&
                            JsonConvert.DeserializeObject<JObject>(item.Value["values"].First.ToString())["name2ID"].ToString() == school.Name2ID)
                        {
                            tmpList.Add(item.Name);
                        }
                    }
                }
            }

            return tmpList;
        }
    }
}