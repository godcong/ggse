using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace guigubahuang
{
    public class Tools
    {
        public static void CheckCboSelectItem(ComboBox cbo)
        {
            if (cbo.Text.ToString().Contains("--"))
                cbo.SelectedItem = null;
        }
        public static string GetGongFa(string value, string path)
        {
            try
            {
                string str = string.Empty;
                using (StreamReader file = new StreamReader(path))
                {
                    str = file.ReadLine();
                    while (!string.IsNullOrEmpty(str))
                    {
                        if (!str.Contains("--"))
                        {
                            if (str.Contains(value))
                                return str;
                        }
                        str = file.ReadLine();
                    }
                }

                return str;
            }
            catch { throw; }
        }
        public static void SetCboGongFa(ComboBox cbo, string path)
        {
            if (cbo.Items.Count > 0) cbo.Items.Clear();
            try
            {
                using (StreamReader file = new StreamReader(path))
                {
                    string value = file.ReadLine();
                    while (!string.IsNullOrEmpty(value))
                    {
                        if (value.Length > 10)
                            cbo.Items.Add(value.Split('=')[0]);
                        else
                            cbo.Items.Add(value);
                        value = file.ReadLine();
                    }
                }
            }
            catch { throw; }
        }
        public static void ChangedLogName(string path, string newName, string oldName)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader reader = new StreamReader(path))
            {
                sb.Append(reader.ReadToEnd());
                sb.Replace(oldName, newName);
            }
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(sb);
                sw.Flush();
            }
        }
        public static int GetZhiWei(School school, string heroID)
        {
            //全部
            //宗主
            //大长老
            //长老
            //真传弟子
            //内门弟子
            //外门弟子
            if (school.ZongZhu == heroID) return 1;
            foreach (string id in school.DaZhangLao)
            {
                if (id == heroID)
                    return 2;
            }
            foreach (string id in school.ZhangLao)
            {
                if (id == heroID)
                    return 3;
            }
            foreach (string id in school.ZhenChuan)
            {
                if (id == heroID)
                    return 4;
            }
            foreach (string id in school.NeiMen)
            {
                if (id == heroID)
                    return 5;
            }
            foreach (string id in school.WaiMen)
            {
                if (id == heroID)
                    return 6;
            }

            return 0;
        }
        public static void ShowNPCLevelList(ListBox lBox, List<Hero> list, List<School> sList, int level, string levelName, bool isTianJiao, bool isSanXiu)
        {
            lBox.Items.Clear();
            string zongmenName = string.Empty;
            foreach (Hero item in list)
            {
                zongmenName = GetZongMenName(sList, item.ZongMenID);

                if (level == 0)
                {
                    if (isTianJiao && isSanXiu)
                    {
                        if (item.IsTianJiao && string.IsNullOrEmpty(zongmenName))
                            lBox.Items.Add(item.GetName() + "   " + item.GetJingJie(item.JingJie) + "    " + zongmenName);
                    }
                    else if (isTianJiao && !isSanXiu)
                    {
                        if (item.IsTianJiao && !string.IsNullOrEmpty(zongmenName))
                            lBox.Items.Add(item.GetName() + "   " + item.GetJingJie(item.JingJie) + "    " + zongmenName);
                    }
                    else if (!isTianJiao && isSanXiu)
                    {
                        if (!item.IsTianJiao && string.IsNullOrEmpty(zongmenName))
                            lBox.Items.Add(item.GetName() + "   " + item.GetJingJie(item.JingJie) + "    " + zongmenName);
                    }
                    else
                        lBox.Items.Add(item.GetName() + "   " + item.GetJingJie(item.JingJie) + "    " + zongmenName);
                }
                else
                {
                    if (item.GetJingJie(item.JingJie).Contains(levelName))
                    {
                        if (isTianJiao && isSanXiu)
                        {
                            if (item.IsTianJiao && string.IsNullOrEmpty(zongmenName))
                                lBox.Items.Add(item.GetName() + "   " + item.GetJingJie(item.JingJie) + "    " + zongmenName);
                        }
                        else if (isTianJiao && !isSanXiu)
                        {
                            if (item.IsTianJiao && !string.IsNullOrEmpty(zongmenName))
                                lBox.Items.Add(item.GetName() + "   " + item.GetJingJie(item.JingJie) + "    " + zongmenName);
                        }
                        else if (!isTianJiao && isSanXiu)
                        {
                            if (!item.IsTianJiao && string.IsNullOrEmpty(zongmenName))
                                lBox.Items.Add(item.GetName() + "   " + item.GetJingJie(item.JingJie) + "    " + zongmenName);
                        }
                        else
                            lBox.Items.Add(item.GetName() + "   " + item.GetJingJie(item.JingJie) + "    " + zongmenName);
                    }
                }
            }
        }
        public static void ShowSchoolUnitList(ListBox lBox, List<Hero> list, ComboBox cboLevel, School school)
        {
            lBox.Items.Clear();
            switch (cboLevel.SelectedIndex)
            {
                case 0:
                    ShowSchoolUnitList(lBox, school.ZongZhu, cboLevel.Items[1].ToString(), list);
                    ShowSchoolUnitList(lBox, school.DaZhangLao, cboLevel.Items[2].ToString(), list);
                    ShowSchoolUnitList(lBox, school.ZhangLao, cboLevel.Items[3].ToString(), list);
                    ShowSchoolUnitList(lBox, school.ZhenChuan, cboLevel.Items[4].ToString(), list);
                    ShowSchoolUnitList(lBox, school.NeiMen, cboLevel.Items[5].ToString(), list);
                    ShowSchoolUnitList(lBox, school.WaiMen, cboLevel.Items[6].ToString(), list);
                    break;
                case 1:
                    ShowSchoolUnitList(lBox, school.ZongZhu, cboLevel.Items[1].ToString(), list);
                    break;
                case 2:
                    ShowSchoolUnitList(lBox, school.DaZhangLao, cboLevel.Items[2].ToString(), list);
                    break;
                case 3:
                    ShowSchoolUnitList(lBox, school.ZhangLao, cboLevel.Items[3].ToString(), list);
                    break;
                case 4:
                    ShowSchoolUnitList(lBox, school.ZhenChuan, cboLevel.Items[4].ToString(), list);
                    break;
                case 5:
                    ShowSchoolUnitList(lBox, school.NeiMen, cboLevel.Items[5].ToString(), list);
                    break;
                case 6:
                    ShowSchoolUnitList(lBox, school.WaiMen, cboLevel.Items[6].ToString(), list);
                    break;
            }
        }
        public static void ShowXianTianQiYun(ListBox lBox, ComboBox cbox, string[] str)
        {
            lBox.Items.Clear();
            foreach (string s in str)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    foreach (string ss in cbox.Items)
                    {
                        if (Equals(s, ss.Split('=')[0]))
                        {
                            lBox.Items.Add(ss.Split('=')[1]);
                            break;
                        }
                    }
                }
            }
        }
        public static void ShowGuanXi(TreeView tv, Hero hero)
        {
            tv.Nodes[0].Nodes[0].Text = string.Empty;
            tv.Nodes[0].Nodes[1].Text = string.Empty;
            tv.Nodes[1].Nodes.Clear();
            tv.Nodes[2].Nodes.Clear();
            tv.Nodes[3].Nodes.Clear();
            tv.Nodes[4].Nodes.Clear();
            tv.Nodes[5].Nodes.Clear();
            tv.Nodes[6].Nodes.Clear();
            tv.Nodes[7].Nodes[0].Text = string.Empty;
            tv.Nodes[8].Nodes.Clear();
            tv.Nodes[9].Nodes.Clear();
            tv.Nodes[10].Nodes.Clear();
            tv.Nodes[11].Nodes.Clear();
            tv.Nodes[12].Nodes.Clear();
            tv.Nodes[0].Nodes[0].Text = hero.FuMu[0].Name;
            tv.Nodes[0].Nodes[1].Text = hero.FuMu[1].Name;
            foreach (Relation item in hero.ZiNv)
            {
                if (string.IsNullOrEmpty(item.Name))
                    tv.Nodes[1].Nodes.Add(item.ID);
                else
                    tv.Nodes[1].Nodes.Add(item.Name);
            }
            foreach (Relation item in hero.SiShengZi)
            {
                if (string.IsNullOrEmpty(item.Name))
                    tv.Nodes[2].Nodes.Add(item.ID);
                else
                    tv.Nodes[2].Nodes.Add(item.Name);
            }
            foreach (Relation item in hero.XiongDiJieMei)
            {
                if (string.IsNullOrEmpty(item.Name))
                    tv.Nodes[3].Nodes.Add(item.ID);
                else
                    tv.Nodes[3].Nodes.Add(item.Name);
            }
            foreach (Relation item in hero.YiFuMu)
            {
                if (string.IsNullOrEmpty(item.Name))
                    tv.Nodes[4].Nodes.Add(item.ID);
                else
                    tv.Nodes[4].Nodes.Add(item.Name);
            }
            foreach (Relation item in hero.YiZiNv)
            {
                if (string.IsNullOrEmpty(item.Name))
                    tv.Nodes[5].Nodes.Add(item.ID);
                else
                    tv.Nodes[5].Nodes.Add(item.Name);
            }
            foreach (Relation item in hero.YiXiongDiJieMei)
            {
                if (string.IsNullOrEmpty(item.Name))
                    tv.Nodes[6].Nodes.Add(item.ID);
                else
                    tv.Nodes[6].Nodes.Add(item.Name);
            }
            if (string.IsNullOrEmpty(hero.FuQi.Name))
                tv.Nodes[7].Nodes[0].Text = hero.FuQi.ID;
            else
                tv.Nodes[7].Nodes[0].Text = hero.FuQi.Name;
            foreach (Relation item in hero.Daolv)
            {
                if (string.IsNullOrEmpty(item.Name))
                    tv.Nodes[8].Nodes.Add(item.ID);
                else
                    tv.Nodes[8].Nodes.Add(item.Name);
            }
            foreach (Relation item in hero.ShiFu)
            {
                if (string.IsNullOrEmpty(item.Name))
                    tv.Nodes[9].Nodes.Add(item.ID);
                else
                    tv.Nodes[9].Nodes.Add(item.Name);
            }
            foreach (Relation item in hero.TuDi)
            {
                if (string.IsNullOrEmpty(item.Name))
                    tv.Nodes[10].Nodes.Add(item.ID);
                else
                    tv.Nodes[10].Nodes.Add(item.Name);
            }
            tv.Nodes[11].Nodes.Add(hero.HeroHaoGan);
            foreach (Relation item in hero.HaoYou)
            {
                if (string.IsNullOrEmpty(item.Name))
                    tv.Nodes[12].Nodes.Add(item.ID + ":" + item.HaoGanDu);
                else
                    tv.Nodes[12].Nodes.Add(item.Name + ":" + item.HaoGanDu);
            }
            tv.ExpandAll();
        }
        public static void SelectCbo(ComboBox cbo, string id)
        {
            foreach (string str in cbo.Items)
            {
                if (Equals(str.Split('=')[0], id))
                {
                    cbo.Text = str;
                    break;
                }
            }
        }
        public static string GetZongMenPoint(string point)
        {
            string[] jingwei = point.Split(',');

            //只是大概范围，按宗门大小不可能出这个范围
            if (int.Parse(jingwei[0]) < 90 && int.Parse(jingwei[0]) > 0 &&
                int.Parse(jingwei[1]) < 100 && int.Parse(jingwei[1]) > 0)
            {
                return "永宁州";
            }
            if (int.Parse(jingwei[0]) > 90 && int.Parse(jingwei[0]) < 200 &&
                int.Parse(jingwei[1]) < 70 && int.Parse(jingwei[1]) > 0)
            {
                return "华封州";
            }
            if (int.Parse(jingwei[0]) > 120 && int.Parse(jingwei[0]) < 200 &&
                int.Parse(jingwei[1]) > 79 && int.Parse(jingwei[1]) < 200)
            {
                return "云陌州";
            }
            if (int.Parse(jingwei[0]) > 67 && int.Parse(jingwei[0]) < 120 &&
                int.Parse(jingwei[1]) > 114 && int.Parse(jingwei[1]) < 200)
            {
                return "幕仙州";
            }
            return "赤幽州";
        }
        public static string GetZongMenName(string name, string id)
        {
            foreach (string s in name.Split(','))
            {
                if (id == s.Split('=')[0])
                {
                    return s.Split('=')[1];
                }
            }
            return string.Empty;
        }
        public static string GetZongMenName(List<School> list, string id)
        {
            foreach (School item in list)
            {
                if (id == item.ID)
                    return item.Name;
            }
            return string.Empty;
        }
        public static string[] GetZongMenName()
        {
            string name1 = string.Empty;
            string name2 = string.Empty;
            string name3 = string.Empty;

            int i = 0;
            using (StreamReader reader = new StreamReader(CacheData.GetZongMenNamePath()))
            {
                string tmp = reader.ReadLine();
                while (!string.IsNullOrEmpty(tmp))
                {
                    if (i < 52)
                    {
                        name1 = name1 + tmp + ",";
                    }
                    else if (i > 51 && i < 104)
                    {
                        name2 = name2 + tmp + ",";
                    }
                    else
                    {
                        name3 = name3 + tmp + ",";
                    }
                    i++;
                    tmp = reader.ReadLine();
                }
            }
            name1 = name1.Remove(name1.Length - 1, 1);
            name2 = name2.Remove(name2.Length - 1, 1);
            name3 = name3.Remove(name3.Length - 1, 1);
            return new string[] { name1, name2, name3 };
        }
        public static void SearchListBoxUnit(ListBox lBox, string value)
        {
            if (lBox.Items != null)
            {
                List<Hero> list = new List<Hero>();
                foreach (Hero item in lBox.Items)
                {
                    if (item.Name.Contains(value))
                    {
                        list.Add(item);
                    }
                }
                ShowListBox(lBox, list);
            }
        }
        public static void ShowListBox(ListBox lBox, object list)
        {
            if (list != null)
            {
                lBox.DataSource = null;
                lBox.Items.Clear();
                lBox.DataSource = list;
                if (list is List<Hero>)
                    lBox.DisplayMember = "Name";
                else
                    lBox.DisplayMember = "Name2";   //暂时无奈的选择
            }
        }
        public static string SelectedJsonPath()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "请选择游戏存档所在文件夹";
                dialog.SelectedPath = @"C:\Users\" + Environment.UserName + @"\AppData\LocalLow\guigugame\guigubahuang\Steam\CacheData";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(dialog.SelectedPath))
                    {
                        Config.WriteIni(dialog.SelectedPath);
                        return dialog.SelectedPath;
                    }
                }
                return string.Empty;
            }
        }
        public static string GetName(string id, List<Hero> list)
        {
            string name = string.Empty;
            foreach (Hero item in list)
            {
                if (id == item.ID)
                {
                    name = item.GetName();
                    break;
                }
            }
            return name;
        }
        public static string GetID(string value)
        {
            string[] str = value.Split('=');
            return str[0];
        }
        public static string GetID(string name, List<Hero> list)
        {
            string id = string.Empty;
            foreach (Hero item in list)
            {
                if (item.GetName() == name)
                {
                    id = item.ID;
                    break;
                }
            }
            return id;
        }
        public static string[] GetQiYunID(ListBox lbox, ComboBox cbo)
        {
            string tmp = string.Empty;
            if (lbox.Items.Count > 0)
            {
                foreach (string s in lbox.Items)
                {
                    foreach (string ss in cbo.Items)
                    {
                        if (ss.Contains(s))
                        {
                            tmp = tmp + GetID(ss) + ",";
                            break;
                        }
                    }
                }
                tmp = tmp.Remove(tmp.Length - 1, 1);
            }
            return tmp.Split(',');
        }
        public static StringBuilder GetTxt(string path)
        {
            try
            {
                using (StreamReader file = new StreamReader(path))
                {
                    return new StringBuilder(file.ReadToEnd());
                }
            }
            catch
            {
                return new StringBuilder();
            }
        }
        public static string GetJingJie(string id)
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
                    return string.Empty;
            }
        }
        public static string GetTangKou(int id)
        {
            switch (id)
            {
                case 0:
                    return "Personnel"; //招贤堂
                case 1:
                    return "Security";  //巡察堂
                case 2:
                    return "Constructor";   //修筑堂
                case 3:
                    return "Train"; //贮宝堂
                case 4:
                    return "Diplomacy"; //外务堂
                default:
                    return string.Empty;
            }
        }
        public static void WriteJson(string value, string path)
        {
            File.WriteAllText(path, value);
        }
        public static void Bak(string path)
        {
            try
            {
                Tools.CopyDir(path, Application.StartupPath + "\\bak\\" + DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒"));
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                {
                    aimPath += System.IO.Path.DirectorySeparatorChar;
                }
                if (!System.IO.Directory.Exists(aimPath))
                {
                    System.IO.Directory.CreateDirectory(aimPath);
                }
                string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
                foreach (string file in fileList)
                {
                    if (System.IO.Directory.Exists(file))
                    {
                        CopyDir(file, aimPath + System.IO.Path.GetFileName(file));
                    }
                    else
                    {
                        System.IO.File.Copy(file, aimPath + System.IO.Path.GetFileName(file), true);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void ShowSchoolUnitList(ListBox lBox, string[] nameID, string zhiwei, List<Hero> list)
        {
            foreach (string id in nameID)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    foreach (Hero hero in list)
                    {
                        if (string.Equals(hero.ID, id))
                        {
                            lBox.Items.Add(hero.GetName() + "   " + zhiwei + "  " + hero.GetJingJie(hero.JingJie));
                            break;
                        }
                    }
                }
            }
        }
        private static void ShowSchoolUnitList(ListBox lBox, string nameID, string zhiwei, List<Hero> list)
        {
            if (!string.IsNullOrEmpty(nameID))
            {
                foreach (Hero hero in list)
                {
                    if (string.Equals(hero.ID, nameID))
                    {
                        lBox.Items.Add(hero.GetName() + "    "+ zhiwei + "  " + hero.GetJingJie(hero.JingJie));
                        break;
                    }
                }
            }
        }
    }
}
