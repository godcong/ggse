using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace guigubahuang
{
    public partial class Editor : Form
    {
        private Hero _heroGuanXi;

        private string _heroPath;
        private string _herologPath;
        private string _schoolPath;

        private JObject _heroJObj;
        private JObject _schoolJObj;

        private BackWorker _worker = new BackWorker();

        private List<Hero> _heroList = new List<Hero>();
        private List<School> _schoolList = new List<School>();

        public Editor()
        {
            InitializeComponent();
            txtJsonPath.Text = Config.ReadIni();
            _worker.WorkerStateChanged += _worker_WorkerStateChanged;
        }

        private void InitTxtData()
        {
            if (cboXG1.Items.Count <= 0)
                cboXG1.Items.AddRange(Tools.GetTxt(CacheData.GetNXingGePath()).Replace("\r\n", ",").ToString().Split(','));
            if (cboXG2.Items.Count <= 0)
                cboXG2.Items.AddRange(Tools.GetTxt(CacheData.GetWXingGePath()).Replace("\r\n", ",").ToString().Split(','));
            if (cboXG3.Items.Count <= 0)
                cboXG3.Items.AddRange(Tools.GetTxt(CacheData.GetWXingGePath()).Replace("\r\n", ",").ToString().Split(','));
            if (cboAH1.Items.Count <= 0)
                cboAH1.Items.AddRange(Tools.GetTxt(CacheData.GetAiHao()).Replace("\r\n", ",").ToString().Split(','));
            if (cboAH2.Items.Count <= 0)
                cboAH2.Items.AddRange(Tools.GetTxt(CacheData.GetAiHao()).Replace("\r\n", ",").ToString().Split(','));
            if (cboAH3.Items.Count <= 0)
                cboAH3.Items.AddRange(Tools.GetTxt(CacheData.GetAiHao()).Replace("\r\n", ",").ToString().Split(','));
            if (cboXTQY.Items.Count <= 0)
                cboXTQY.Items.AddRange(Tools.GetTxt(CacheData.GetXianTian()).Replace("\r\n", ",").ToString().Split(','));
            if (cboDaoXin.Items.Count <= 0)
                cboDaoXin.Items.AddRange(Tools.GetTxt(CacheData.GetNiTian()).Replace("\r\n", ",").ToString().Split(','));
            if (cboSchoolLevel.Items.Count <= 0)
                cboSchoolLevel.Items.AddRange(Tools.GetTxt(CacheData.GetZongMenZhiWei()).Replace("\r\n", ",").ToString().Split(','));
            if (cboZMNPCLevel.Items.Count <= 0)
                cboZMNPCLevel.Items.AddRange(Tools.GetTxt(CacheData.GetJingJie()).Replace("\r\n", ",").ToString().Split(','));
            if (cboNiTianGaiMing.Items.Count <= 0)
                cboNiTianGaiMing.Items.AddRange(Tools.GetTxt(CacheData.GetNiTian()).Replace("\r\n", ",").ToString().Split(','));
            Tools.SetCboGongFa(cboLeft, CacheData.GetZuoJian());
            Tools.SetCboGongFa(cboRight, CacheData.GetYouJian());
            Tools.SetCboGongFa(cboSpace, CacheData.GetKongGe());
            Tools.SetCboGongFa(cboR, CacheData.GetRJian());
        }
        private Hero GetHero()
        {
            return new Hero()
            {
                IsZhuJue=(lBoxUnit.SelectedItem as Hero).IsZhuJue,
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                NeiZaiXingGe = Tools.GetID(cboXG1.Text),
                WaiZaiXingGe = Tools.GetID(cboXG2.Text),
                WaiZaiXingGe2 = Tools.GetID(cboXG3.Text),
                XingQu = new string[] { Tools.GetID(cboAH1.Text), Tools.GetID(cboAH2.Text), Tools.GetID(cboAH3.Text) },
                XianTianQiYun = Tools.GetQiYunID(lBoxXTQY, cboXTQY),
                NiTianGaiMing = Tools.GetQiYunID(lBoxNiTianGaiMing, cboNiTianGaiMing),
                DaoXin = Tools.GetQiYunID(lBoxDaoXin, cboDaoXin),
                IsTianJiao = checkBox4.Checked,
                LingShi = txtLingshi2.Text,
                NianLing = txtSM1.Text,
                NianLingSX = txtSM2.Text,
                XinQing = txtXQ1.Text,
                XinQingSX = txtXQ2.Text,
                JianKang = txtJK1.Text,
                JianKangSX = txtJK2.Text,
                JingLi = txtJL1.Text,
                JingLiSX = txtJL2.Text,
                TiLi = txtTL1.Text,
                TiLiSX = txtTL2.Text,
                LingLi = txtLL1.Text,
                LingLiSX = txtLL2.Text,
                NianLi = txtNL1.Text,
                NianLiSX = txtNL2.Text,
                XingYun = txtXY.Text,
                WuXing = txtWX.Text,
                MeiLi = txtML.Text,
                ShengWang = txtSW.Text,
                GongJi = txtGJ.Text,
                FangYu = txtFY.Text,
                JiaoLi = txtJL.Text,
                GongFaKangXing = txtGFKX.Text,
                LingGenKangXing = txtLGKX.Text,
                FengLingGen = txtFLG.Text,
                HuiXin = txtHX.Text,
                HuXin = txtHuXin.Text,
                YiSu = txtYS.Text,
                BaoJiBeiShu = txtKJBS.Text,
                KangBaoBeiShu = txtKBBS.Text,
                DaoDian = txtDX.Text,
                DaoFa = txtDF.Text,
                QiangFa = txtQF.Text,
                JianFa = txtJF.Text,
                QuanFa = txtQuanFa.Text,
                ZhangFa = txtZF.Text,
                ZhiFa = txtZhiFa.Text,
                HuoLingGen = txtHLG.Text,
                ShuiLingGen = txtSLG.Text,
                LeiLingGen = txtLLG.Text,
                TuLingGen = txtTLG.Text,
                MuLingGen = txtMLG.Text,
                LianDan = txtLD.Text,
                LianQi = txtLQ.Text,
                FengShui = txtFS.Text,
                HuaFu = txtHF.Text,
                YaoCai = txtYC.Text,
                KuangCai = txtKC.Text,
                ZhengDao = txtZD.Text,
                MoDao = txtMD.Text
            };
        }
        private void SetHeroShuXing(Hero hero)
        {
            if (hero != null)
            {
                txtFirstName.Text = hero.FirstName;
                txtLastName.Text = hero.LastName;
                Tools.SelectCbo(cboXG1, hero.NeiZaiXingGe);
                Tools.SelectCbo(cboXG2, hero.WaiZaiXingGe);
                Tools.SelectCbo(cboXG3, hero.WaiZaiXingGe2);
                Tools.SelectCbo(cboAH1, hero.XingQu[0]);
                Tools.SelectCbo(cboAH2, hero.XingQu[1]);
                Tools.SelectCbo(cboAH3, hero.XingQu[2]);
                Tools.ShowXianTianQiYun(lBoxXTQY, cboXTQY, hero.XianTianQiYun);
                Tools.ShowXianTianQiYun(lBoxDaoXin, cboDaoXin, hero.DaoXin);
                Tools.ShowXianTianQiYun(lBoxNiTianGaiMing, cboNiTianGaiMing, hero.NiTianGaiMing);
                checkBox4.Checked = hero.IsTianJiao;
                txtLingshi2.Text = hero.LingShi;
                txtSM1.Text = hero.NianLing;
                txtSM2.Text = hero.NianLingSX;
                txtXQ1.Text = hero.XinQing;
                txtXQ2.Text = hero.XinQingSX;
                txtJK1.Text = hero.JianKang;
                txtJK2.Text = hero.JianKangSX;
                txtFLG.Text = hero.FengLingGen;
                txtTL1.Text = hero.TiLi;
                txtTL2.Text = hero.TiLiSX;
                txtLL1.Text = hero.LingLi;
                txtLL2.Text = hero.LingLiSX;
                txtJL1.Text = hero.JingLi;
                txtJL2.Text = hero.JingLiSX;
                txtNL1.Text = hero.NianLi;
                txtNL2.Text = hero.NianLiSX;
                txtXY.Text = hero.XingYun;
                txtWX.Text = hero.WuXing;
                txtML.Text = hero.MeiLi;
                txtSW.Text = hero.ShengWang;
                txtGJ.Text = hero.GongJi;
                txtFY.Text = hero.FangYu;
                txtJL.Text = hero.JiaoLi;
                txtGFKX.Text = hero.GongFaKangXing;
                txtLGKX.Text = hero.LingGenKangXing;
                txtHX.Text = hero.HuiXin;
                txtHuXin.Text = hero.HuXin;
                txtYS.Text = hero.YiSu;
                txtKJBS.Text = hero.BaoJiBeiShu;
                txtKBBS.Text = hero.KangBaoBeiShu;
                txtDX.Text = hero.DaoDian;
                txtDF.Text = hero.DaoFa;
                txtQF.Text = hero.QiangFa;
                txtJF.Text = hero.JianFa;
                txtQuanFa.Text = hero.QuanFa;
                txtZF.Text = hero.ZhangFa;
                txtZhiFa.Text = hero.ZhiFa;
                txtHLG.Text = hero.HuoLingGen;
                txtSLG.Text = hero.ShuiLingGen;
                txtLLG.Text = hero.LeiLingGen;
                txtTLG.Text = hero.TuLingGen;
                txtMLG.Text = hero.MuLingGen;
                txtLD.Text = hero.LianDan;
                txtLQ.Text = hero.LianQi;
                txtFS.Text = hero.FengShui;
                txtHF.Text = hero.HuaFu;
                txtYC.Text = hero.YaoCai;
                txtKC.Text = hero.KuangCai;
                txtZD.Text = hero.ZhengDao;
                txtMD.Text = hero.MoDao;
            }
        }
        private void SetHeroGuanXi(Hero hero)
        {
            try
            {
                _heroGuanXi = _worker.GetHeroGuanXi(_heroJObj, hero.ID, _heroList);
                Tools.ShowGuanXi(treeView1, _heroGuanXi);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void SetGongFaJiYi()
        {
            txtDF.Text = "999";
            txtQF.Text = "999";
            txtJF.Text = "999";
            txtQuanFa.Text = "999";
            txtZF.Text = "999";
            txtZhiFa.Text = "999";
            txtHLG.Text = "999";
            txtSLG.Text = "999";
            txtLLG.Text = "999";
            txtTLG.Text = "999";
            txtMLG.Text = "999";
            txtFLG.Text = "999";
            txtLD.Text = "999";
            txtLQ.Text = "999";
            txtFS.Text = "999";
            txtHF.Text = "999";
            txtYC.Text = "999";
            txtKC.Text = "999";
        }
        private void SetSchoolShuXing(School school)
        {
            if (school != null)
            {
                txtLingShi.Text = school.LingShi;
                txtRongYu.Text = school.ZongMenRongYu;
                txtFanRong.Text = school.FanRong;
                txtZhongCheng.Text = school.ZhongCheng;
                txtAnDing.Text = school.AnDing;
                txtJiMingDiZi.Text = school.JiMingDiZi;
                txtYaoCai.Text = school.YaoCai;
                txtKuangCai.Text = school.KuangCai;
                Tools.ShowSchoolUnitList(lBoxSchoolUnit, _heroList, cboSchoolLevel, school);
                lBoxSchoolDie.Items.Clear();
                foreach (Hero hero in _worker.GetSchoolYiSiYuanHun(_heroJObj, school.ID))
                {
                    lBoxSchoolDie.Items.Add(hero.GetName() + "   " + hero.GetJingJie(hero.JingJie));
                }
            }
        }
        private void ClearSchoolUnit(JObject jObj, School school, string heroID)
        {
            try
            {
                _worker.ClearZongZhu(jObj, school, heroID);
                _worker.ClearDaZhangLao(jObj, school, heroID);
                _worker.ClearZhangLao(jObj, school, heroID);
                _worker.ClearZhenChuan(jObj, school, heroID);
                _worker.ClearNeiMen(jObj, school, heroID);
                _worker.ClearWaiMen(jObj, school, heroID);
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        #region 保存角色tab

        private async void SaveHero()
        { 
            try
            {
                if (lBoxUnit.SelectedItem != null)
                {
                    Tools.Bak(txtJsonPath.Text);
                    Hero hero = lBoxUnit.SelectedItem as Hero;
                    Hero newHero = GetHero();
                    _worker.SaveHeroShuXing(_heroJObj, hero.ID, newHero);

                    Tools.ChangedLogName(txtJsonPath.Text + "\\" + CacheData.JsonName[1], newHero.Name, hero.Name);
                    lBoxAllUnit.Items.Clear();
                    _heroList.Clear();
                    _heroList = _worker.GetHeroList(_heroJObj);
                    Tools.ShowListBox(lBoxUnit, _heroList);
                    foreach (Hero item in _heroList)
                    { 
                        if(item.Name == newHero.Name)
                            lBoxUnit.SelectedItem = item;
                    }
                    
                    await _worker.SaveJson(_heroJObj, txtJsonPath.Text + "\\" + CacheData.CacheName[0], _heroPath);
                    await _worker.SaveJson(null, txtJsonPath.Text + "\\" + CacheData.CacheName[1], _herologPath);
                    MessageBox.Show("保存成功");
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        private async void AddGuanXi()
        {
            try
            {
                if (lBoxUnit.SelectedItem != null)
                {
                    Tools.Bak(txtJsonPath.Text);
                    Hero hero = lBoxUnit.SelectedItem as Hero;

                    Form2 form = new Form2();
                    form.Text = "添加";
                    string name = form.Dialog(this, _heroList);

                    if (!string.IsNullOrEmpty(name))
                    {
                        if (treeView1.SelectedNode != null)
                            _worker.SaveGuanXi(_heroJObj, treeView1.SelectedNode.Index,
                                                  hero.ID,
                                                  Tools.GetID(name, _heroList));

                        _heroList = _worker.GetHeroList(_heroJObj);
                        Tools.ShowListBox(lBoxUnit, _heroList);

                        await _worker.SaveJson(_heroJObj, txtJsonPath.Text + "\\" + CacheData.CacheName[0], _heroPath);
                        foreach (Hero item in _heroList)
                        {
                            if (item.Name == hero.Name)
                                lBoxUnit.SelectedItem = item;
                        }
                        MessageBox.Show("保存成功");
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private async void DelGuanXi()
        {
            try
            {
                if (lBoxUnit.SelectedItem != null)
                {
                    Tools.Bak(txtJsonPath.Text);
                    Hero hero = lBoxUnit.SelectedItem as Hero;
                    if (treeView1.SelectedNode != null && !string.IsNullOrEmpty(treeView1.SelectedNode.Text))
                        _worker.DelGuanXi(_heroJObj, treeView1.SelectedNode.Parent.Index,
                                             hero.ID,
                                             Tools.GetID(treeView1.SelectedNode.Text, _heroList),
                                             treeView1.SelectedNode.Index);

                    _heroList = _worker.GetHeroList(_heroJObj);
                    Tools.ShowListBox(lBoxUnit, _heroList);

                    await _worker.SaveJson(_heroJObj, txtJsonPath.Text + "\\" + CacheData.CacheName[0], _heroPath);
                    foreach (Hero item in _heroList)
                    {
                        if (item.Name == hero.Name)
                            lBoxUnit.SelectedItem = item;
                    }
                    MessageBox.Show("保存成功");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private async void ChangedGuanXi()
        {
            try
            {
                if (lBoxUnit.SelectedItem != null)
                {
                    if (treeView1.SelectedNode.Parent != null &&
                        treeView1.SelectedNode.Parent.Index == 12)
                    {
                        txtHaoGan.Text = treeView1.SelectedNode.Text.Split(':')[1];
                        return;
                    }
                    if (treeView1.SelectedNode.Parent != null &&
                        treeView1.SelectedNode.Parent.Index == 11)
                    {
                        txtHaoGan.Text = treeView1.SelectedNode.Text;
                        return;
                    }
                    Tools.Bak(txtJsonPath.Text);
                    Hero hero = lBoxUnit.SelectedItem as Hero;
                    Form2 form = new Form2();
                    form.Text = "修改";
                    string name = form.Dialog(this, _heroList);
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (treeView1.SelectedNode != null)
                            _worker.ChangedGuanxi(_heroJObj, treeView1.SelectedNode.Parent.Index,
                                                 hero.ID,
                                                 Tools.GetID(treeView1.SelectedNode.Text, _heroList),
                                                 Tools.GetID(name, _heroList), treeView1.SelectedNode.Index);

                        _heroList = _worker.GetHeroList(_heroJObj);
                        Tools.ShowListBox(lBoxUnit, _heroList);

                        await _worker.SaveJson(_heroJObj, txtJsonPath.Text + "\\" + CacheData.CacheName[0], _heroPath);
                        foreach (Hero item in _heroList)
                        {
                            if (item.Name == hero.Name)
                                lBoxUnit.SelectedItem = item;
                        }
                        MessageBox.Show("保存成功");
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private async void ChangedHaoGan()
        {
            try
            {
                if (lBoxUnit.SelectedItem != null && treeView1.SelectedNode != null && treeView1.SelectedNode.Parent != null)
                {
                    Tools.Bak(txtJsonPath.Text);
                    Hero hero = lBoxUnit.SelectedItem as Hero;
                    string heroID = string.Empty;

                    foreach (Hero item in _heroList)
                    {
                        if (treeView1.SelectedNode.Parent.Index == 12 && item.GetName() == treeView1.SelectedNode.Text.Split(':')[0])
                        {
                            heroID = item.ID;
                        }
                    }

                    _worker.ChangedHaoGan(_heroJObj, treeView1.SelectedNode.Parent.Index, hero.ID, heroID, txtHaoGan.Text);

                    _heroList = _worker.GetHeroList(_heroJObj);
                    Tools.ShowListBox(lBoxUnit, _heroList);

                    await _worker.SaveJson(_heroJObj, txtJsonPath.Text + "\\" + CacheData.CacheName[0], _heroPath);
                    foreach (Hero item in _heroList)
                    {
                        if (item.Name == hero.Name)
                            lBoxUnit.SelectedItem = item;
                    }
                    MessageBox.Show("保存成功");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private async void SaveGongFa()
        {
            try
            {
                Tools.Bak(txtJsonPath.Text);
                _worker.SaveGongFa(_heroJObj, (lBoxUnit.SelectedItem as Hero).ID, cboJingJie.SelectedIndex + 1, cboXinFa.SelectedIndex + 1, cboLeft.Text, cboRight.Text, cboSpace.Text, cboR.Text);
                await _worker.SaveJson(_heroJObj, txtJsonPath.Text + "\\" + CacheData.CacheName[0], _heroPath);
                MessageBox.Show("保存成功");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

        #region 保存宗门tab

        private async void SaveSchoolShuXing(School school)
        {
            try
            {
                if (school != null)
                {
                    Tools.Bak(txtJsonPath.Text);
                    school.LingShi = txtLingShi.Text;
                    school.JiMingDiZi = txtJiMingDiZi.Text;
                    school.ZongMenRongYu = txtRongYu.Text;
                    school.FanRong = txtFanRong.Text;
                    school.ZhongCheng = txtZhongCheng.Text;
                    school.AnDing = txtAnDing.Text;
                    school.KuangCai = txtKuangCai.Text;
                    school.YaoCai = txtYaoCai.Text;

                    _worker.SaveSchoolShuXing(_schoolJObj, school, _schoolList);

                    await _worker.SaveJson(_schoolJObj, txtJsonPath.Text + "\\" + CacheData.CacheName[4], _schoolPath);
                    MessageBox.Show(this, "保存成功");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private async void ReviveSchoolUnit()
        {
            try
            {
                if (lBoxSchoolDie.SelectedItems.Count > 0)
                {
                    Tools.Bak(txtJsonPath.Text);
                    List<Hero> tmpList = new List<Hero>();

                    foreach (string str in lBoxSchoolDie.SelectedItems)
                    {
                        foreach (Hero hero in _heroList)
                        {
                            if (str.Split(' ')[0] == hero.GetName())
                            {
                                tmpList.Add(hero);
                                break;
                            }
                        }
                    }
                    _worker.ReviveSchoolUnit(_heroJObj, tmpList);

                    for (int i = 0; i < lBoxSchoolDie.SelectedItems.Count; i++)
                    {
                        lBoxSchoolDie.Items.Remove(lBoxSchoolDie.SelectedItems[i]);
                        i--;
                    }

                    await _worker.SaveJson(_heroJObj, txtJsonPath.Text + "\\" + CacheData.CacheName[0], _heroPath);
                    MessageBox.Show(this, "复活成功");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private async void AddSchoolUnit()
        {
            if (lBoxSchool.SelectedItem != null && lBoxAllUnit.SelectedItem != null)
            {
                try
                {
                    Tools.Bak(txtJsonPath.Text);
                    string heroID = string.Empty;
                    School school = lBoxSchool.SelectedItem as School;

                    if (!school.IsMainSchool && cboSchoolLevel.SelectedIndex == 1)
                    {
                        MessageBox.Show("分舵不能操作宗主数据！");
                        return;
                    }
                    if (cboSchoolLevel.SelectedIndex == 3 || cboSchoolLevel.SelectedIndex == 2 || cboSchoolLevel.SelectedIndex == 1)
                    {
                        if (MessageBox.Show("宗主只能有1个，大长老只能有2个，长老只能有5个，若原位置有人则自动将其降为真传弟子，确定吗？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                        {
                            return;
                        }
                    }

                    foreach (Hero item in _heroList)
                    {
                        if (item.Name == lBoxAllUnit.SelectedItem.ToString().Split(' ')[0])
                        {
                            heroID = item.ID;
                            break;
                        }
                    }

                    //先把选择的角色从自己宗门删除
                    ClearSchoolUnit(_schoolJObj, school, heroID);

                    if (cboSchoolLevel.SelectedIndex == 1)
                    {
                        _worker.AddZongZhu(_schoolJObj, school, heroID, comboBox1.SelectedIndex);
                    }
                    if (cboSchoolLevel.SelectedIndex == 2)
                    {
                        _worker.AddDaZhangLao(_schoolJObj, school, heroID, comboBox1.SelectedIndex);
                    }
                    if (cboSchoolLevel.SelectedIndex == 3)
                    {
                        _worker.AddZhangLao(_schoolJObj, school, heroID, comboBox1.SelectedIndex);
                    }
                    if (cboSchoolLevel.SelectedIndex == 4)
                    {
                        _worker.AddZhenChuan(_schoolJObj, school, heroID, comboBox1.SelectedIndex);
                    }
                    if (cboSchoolLevel.SelectedIndex == 5)
                    {
                        _worker.AddNeiMen(_schoolJObj, school, heroID, comboBox1.SelectedIndex);
                    }
                    if (cboSchoolLevel.SelectedIndex == 6 ||
                        cboSchoolLevel.SelectedIndex == 0)
                    {
                        _worker.AddWaiMen(_schoolJObj, school, heroID, comboBox1.SelectedIndex);
                    }

                    _schoolList.Clear();
                    _schoolList = _worker.GetSchoolList(_schoolJObj);

                    School selectSchool = null;
                    foreach (School item in _schoolList)
                    {
                        if (string.Equals(item.ID, school.ID))
                        {
                            selectSchool = item;
                            //Tools.ShowSchoolUnitList(lBoxSchoolUnit, _heroList, cboSchoolLevel, item);
                            Tools.ShowListBox(lBoxSchool, _schoolList);
                            break;
                        }
                    }
                    lBoxSchool.SelectedItem = selectSchool;
                    
                    _worker.UpdateHero(_heroJObj, _heroList, string.Empty, heroID, school.ID);
                    
                    await _worker.SaveJson(_heroJObj, txtJsonPath.Text + "\\" + CacheData.CacheName[0], _heroPath);
                    await _worker.SaveJson(_schoolJObj, txtJsonPath.Text + "\\" + CacheData.CacheName[4], _schoolPath);
                    MessageBox.Show(this, "添加成功");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else
                MessageBox.Show("选择要添加的宗门和角色");
        }
        private async void ChangedSchoolUnit()
        {
            try
            {
                if (lBoxSchool.SelectedItem != null && lBoxAllUnit.SelectedItem != null && lBoxSchoolUnit.SelectedItem != null)
                {
                    Tools.Bak(txtJsonPath.Text);
                    string heroID = string.Empty;
                    string oldHeroID = string.Empty;

                    School school = lBoxSchool.SelectedItem as School;

                    foreach (Hero item in _heroList)
                    {
                        if (item.Name == lBoxAllUnit.SelectedItem.ToString().Split(' ')[0])
                        {
                            heroID = item.ID;
                        }
                        if (item.Name == lBoxSchoolUnit.SelectedItem.ToString().Split(' ')[0])
                        {
                            oldHeroID = item.ID;
                        }
                    }

                    if (!school.IsMainSchool && school.ZongZhu == oldHeroID)
                    {
                        MessageBox.Show("分舵不能操作宗主数据！");
                        return;
                    }

                    if (MessageBox.Show("替换时被替换角色会变为散修，确定吗？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    {
                        return;
                    }

                    //先把选择的角色从自己宗门删除
                    ClearSchoolUnit(_schoolJObj, school, heroID);

                    _worker.ChangedSchoolUnit(_schoolJObj, school, oldHeroID, heroID, comboBox1.SelectedIndex);

                    School selectSchool = null;
                    _schoolList.Clear();
                    _schoolList = _worker.GetSchoolList(_schoolJObj);

                    foreach (School item in _schoolList)
                    {
                        if (string.Equals(item.ID, school.ID))
                        {
                            selectSchool = item;
                            //Tools.ShowSchoolUnitList(lBoxSchoolUnit, _heroList, cboSchoolLevel, item);
                            Tools.ShowListBox(lBoxSchool, _schoolList);
                            break;
                        }
                    }
                    lBoxSchool.SelectedItem = selectSchool;

                    _worker.UpdateHero(_heroJObj, _heroList, oldHeroID, heroID, school.ID);

                    await _worker.SaveJson(_heroJObj, txtJsonPath.Text + "\\" + CacheData.CacheName[0], _heroPath);
                    await _worker.SaveJson(_schoolJObj, txtJsonPath.Text + "\\" + CacheData.CacheName[4], _schoolPath);
                    MessageBox.Show(this, "替换成功");
                }
                else
                    MessageBox.Show("选择要替换的宗门和角色");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); } 
        }
        private async void DelSchoolUnit()
        {
            try
            {
                if (lBoxSchool.SelectedItem != null && lBoxSchoolUnit.SelectedItem != null)
                {
                    Tools.Bak(txtJsonPath.Text);
                    string heroID = string.Empty;
                    School school = lBoxSchool.SelectedItem as School;

                    foreach (Hero item in _heroList)
                    {
                        if (item.Name == lBoxSchoolUnit.SelectedItem.ToString().Split(' ')[0])
                        {
                            heroID = item.ID;
                        }
                    }

                    if (!school.IsMainSchool && school.ZongZhu == heroID)
                    {
                        MessageBox.Show("分舵不能操作宗主数据！");
                        return;
                    }

                    ClearSchoolUnit(_schoolJObj, school, heroID);

                    School selectSchool = null;
                    _schoolList.Clear();
                    _schoolList = _worker.GetSchoolList(_schoolJObj);

                    foreach (School item in _schoolList)
                    {
                        if (string.Equals(item.ID, school.ID))
                        {
                            selectSchool = item;
                            Tools.ShowListBox(lBoxSchool, _schoolList);
                            break;
                        }
                    }
                    lBoxSchool.SelectedItem = selectSchool;

                    _worker.UpdateHero(_heroJObj, _heroList, heroID, string.Empty, school.ID);

                    await _worker.SaveJson(_heroJObj, txtJsonPath.Text + "\\" + CacheData.CacheName[0], _heroPath);
                    await _worker.SaveJson(_schoolJObj, txtJsonPath.Text + "\\" + CacheData.CacheName[4], _schoolPath);
                    MessageBox.Show(this, "删除成功");
                }
                else
                    MessageBox.Show("选择要删除的宗门和角色");
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

        #region 加载json

        private void DEJson()
        {
            _heroPath = txtJsonPath.Text + "\\" + CacheData.JsonName[0];
            _herologPath = txtJsonPath.Text + "\\" + CacheData.JsonName[1];
            _schoolPath = txtJsonPath.Text + "\\" + CacheData.JsonName[4];

            DE.CreatJson(txtJsonPath.Text + "\\" + CacheData.CacheName[0], _heroPath);
            DE.CreatJson(txtJsonPath.Text + "\\" + CacheData.CacheName[1], _herologPath);
            DE.CreatJson(txtJsonPath.Text + "\\" + CacheData.CacheName[4], _schoolPath);
        }
        private async void LoadingData()
        {
            _schoolJObj = await _worker.LoadingJson(_schoolPath);
            _schoolList = _worker.GetSchoolList(_schoolJObj);

            _heroJObj = await _worker.LoadingJson(_heroPath);
            _heroList = _worker.GetHeroList(_heroJObj);
            
            gBoxUnit.Text = "成员(" + _heroList.Count + ")";
            Tools.ShowListBox(lBoxUnit, _heroList);
            Tools.ShowListBox(lBoxSchool, _schoolList);
            GC.Collect();
        }

        #endregion

        #region 角色tab event

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            txtJsonPath.Text = Tools.SelectedJsonPath();
        }
        private void btnLoadJson_Click(object sender, EventArgs e)
        {
            DEJson();
            InitTxtData();
            LoadingData();
        }
        private void _worker_WorkerStateChanged(object sender, WorkerStateChangedEventArgs e)
        {
            pgbWorkerState.Invoke(new Action(() => { pgbWorkerState.Visible = e.IsBusy; ; }));
        }
        private void txtSearchUnit_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearchUnit.Text))
                Tools.SearchListBoxUnit(lBoxUnit, txtSearchUnit.Text);
            else
            {
                _heroList = _worker.GetHeroList(_heroJObj);
                Tools.ShowListBox(lBoxUnit, _heroList);
            }
        }
        private void lBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lBoxUnit.SelectedItem != null)
            {
                SetHeroShuXing(lBoxUnit.SelectedItem as Hero);
                SetHeroGuanXi(lBoxUnit.SelectedItem as Hero);
            }
            if (lBoxUnit.SelectedIndex == 0)
            {
                groupBox10.Enabled = true;
                groupBox5.Enabled = false;
            }
            else
            {
                groupBox10.Enabled = false;
                groupBox5.Enabled = true;
            }
        }
        private void btnTJ_Click(object sender, EventArgs e)
        {
            if (cboXTQY.SelectedItem != null)
            {
                lBoxXTQY.Items.Add(cboXTQY.SelectedItem.ToString().Split('=')[1]);
            }
        }
        private void btnTHXZ_Click(object sender, EventArgs e)
        {
            if (cboXTQY.SelectedItem != null && lBoxXTQY.SelectedItem != null && lBoxXTQY.Items != null)
            {
                for (int i = 0; i < lBoxXTQY.Items.Count; i++)
                {
                    if (lBoxXTQY.Items[i].ToString() == lBoxXTQY.SelectedItem.ToString())
                    {
                        lBoxXTQY.Items[i] = cboXTQY.SelectedItem.ToString().Split('=')[1];
                        break;
                    }
                }
            }
        }
        private void btnYCXZ_Click(object sender, EventArgs e)
        {
            if (lBoxXTQY.SelectedItem != null && lBoxXTQY.Items != null)
            {
                for (int i = 0; i < lBoxXTQY.Items.Count; i++)
                {
                    if (lBoxXTQY.Items[i].ToString() == lBoxXTQY.SelectedItem.ToString())
                    {
                        lBoxXTQY.Items.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        private void btnAddDaoXin_Click(object sender, EventArgs e)
        {
            if (cboDaoXin.SelectedItem != null)
            {
                lBoxDaoXin.Items.Add(cboDaoXin.SelectedItem.ToString().Split('=')[1]);
            }
        }
        private void btnChangedDaoXin_Click(object sender, EventArgs e)
        {
            if (cboDaoXin.SelectedItem != null)
            {
                for (int i = 0; i < lBoxDaoXin.Items.Count; i++)
                {
                    if (lBoxDaoXin.Items[i].ToString() == lBoxDaoXin.SelectedItem.ToString())
                    {
                        lBoxDaoXin.Items[i] = cboDaoXin.SelectedItem.ToString().Split('=')[1];
                        break;
                    }
                }
            }
        }
        private void btnDelDaoXin_Click(object sender, EventArgs e)
        {
            if (lBoxDaoXin.SelectedItem != null)
            {
                for (int i = 0; i < lBoxDaoXin.Items.Count; i++)
                {
                    if (lBoxDaoXin.Items[i].ToString() == lBoxDaoXin.SelectedItem.ToString())
                    {
                        lBoxDaoXin.Items.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        private void btnYYXG_Click(object sender, EventArgs e)
        {
            if (lBoxUnit.SelectedItem != null)
                SaveHero();
        }
        private void btnAddNTGM_Click(object sender, EventArgs e)
        {
            if (cboNiTianGaiMing.SelectedItem != null)
            {
                lBoxNiTianGaiMing.Items.Add(cboNiTianGaiMing.SelectedItem.ToString().Split('=')[1]);
            }
        }
        private void btnChangedNTGM_Click(object sender, EventArgs e)
        {
            if (cboNiTianGaiMing.SelectedItem != null)
            {
                for (int i = 0; i < lBoxNiTianGaiMing.Items.Count; i++)
                {
                    if (lBoxNiTianGaiMing.Items[i].ToString() == lBoxNiTianGaiMing.SelectedItem.ToString())
                    {
                        lBoxNiTianGaiMing.Items[i] = cboNiTianGaiMing.SelectedItem.ToString().Split('=')[1];
                        break;
                    }
                }
            }
        }
        private void btnDelNTGM_Click(object sender, EventArgs e)
        {
            if (lBoxNiTianGaiMing.SelectedItem != null)
            {
                for (int i = 0; i < lBoxNiTianGaiMing.Items.Count; i++)
                {
                    if (lBoxNiTianGaiMing.Items[i].ToString() == lBoxNiTianGaiMing.SelectedItem.ToString())
                    {
                        lBoxNiTianGaiMing.Items.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        private void addMenu_Click(object sender, EventArgs e)
        {
            AddGuanXi();
        }
        private void changeMenu_Click(object sender, EventArgs e)
        {
            ChangedGuanXi();
        }
        private void delMenu_Click(object sender, EventArgs e)
        {
            DelGuanXi();
        }
        private void btnSaveGuanXi_Click(object sender, EventArgs e)
        {
            ChangedHaoGan();
        }
        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (Hero hero in lBoxUnit.Items)
            {
                if (treeView1.SelectedNode.Text.Contains(hero.Name))
                {
                    lBoxUnit.SelectedItem = hero;
                    break;
                }
            }
        }
        private void btnSaveGongFa_Click(object sender, EventArgs e)
        {
            if (lBoxUnit.SelectedItem != null)
                SaveGongFa();
        }
        private void cboLeft_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tools.CheckCboSelectItem(cboLeft);
        }
        private void cboRight_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tools.CheckCboSelectItem(cboRight);
        }
        private void cboSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tools.CheckCboSelectItem(cboSpace);
        }
        private void cboR_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tools.CheckCboSelectItem(cboR);
        }

        #endregion

        #region 宗门tab event

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)//判断你点的是不是右键
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode CurrentNode = treeView1.GetNodeAt(ClickPoint);
                if (CurrentNode != null)//判断你点的是不是一个节点
                {
                    CurrentNode.ContextMenuStrip = contextMenuStrip1;
                    treeView1.SelectedNode = CurrentNode;//选中这个节点
                    if (treeView1.SelectedNode.Parent != null)
                    {
                        if (treeView1.SelectedNode.Parent.Text == "父母" ||
                            treeView1.SelectedNode.Parent.Text == "夫妻")
                        {
                            addMenu.Enabled = false;
                            changeMenu.Enabled = true;
                            delMenu.Enabled = true;
                        }
                        else if (treeView1.SelectedNode.Parent.Text == "对主角好感度")
                        {
                            addMenu.Enabled = false;
                            changeMenu.Enabled = true;
                            delMenu.Enabled = false;
                        }
                        else
                        {
                            addMenu.Enabled = false;
                            changeMenu.Enabled = true;
                            delMenu.Enabled = true;
                        }
                    }
                    else
                    {
                        if (treeView1.SelectedNode.Text == "父母" ||
                            treeView1.SelectedNode.Text == "夫妻")
                        {
                            addMenu.Enabled = false;
                            changeMenu.Enabled = false;
                            delMenu.Enabled = false;
                        }
                        else if (treeView1.SelectedNode.Text == "对主角好感度")
                        {
                            addMenu.Enabled = false;
                            changeMenu.Enabled = false;
                            delMenu.Enabled = false;
                        }
                        else
                        {
                            addMenu.Enabled = true;
                            changeMenu.Enabled = false;
                            delMenu.Enabled = false;
                        }
                    }
                }
            }
        }
        private void lBoxSchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lBoxSchool.SelectedItem != null)
            {
                SetSchoolShuXing(lBoxSchool.SelectedItem as School);
                //cboSchoolLevel.SelectedIndex = 0;
                //cboZMNPCLevel.SelectedIndex = 0;
            }
            checkBox1.Checked = false;
        }
        private void cboSchoolLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lBoxSchool.SelectedItem != null)
            {
                Tools.ShowSchoolUnitList(lBoxSchoolUnit, _heroList, cboSchoolLevel, lBoxSchool.SelectedItem as School);
                comboBox1.Items.Clear();
                if (cboSchoolLevel.SelectedIndex == 2)
                {
                    comboBox1.Items.AddRange(new string[] { "右侧大长老", "左侧大长老" });
                    comboBox1.SelectedIndex = 0;
                }
                if (cboSchoolLevel.SelectedIndex == 3 ||
                    cboSchoolLevel.SelectedIndex == 4 ||
                    cboSchoolLevel.SelectedIndex == 5 ||
                    cboSchoolLevel.SelectedIndex == 1 ||
                    cboSchoolLevel.SelectedIndex == 6 ||
                    cboSchoolLevel.SelectedIndex == 0)
                {
                    comboBox1.Items.AddRange(new string[] { "招贤堂", "巡查堂", "修筑堂", "贮宝堂", "外务堂" });
                    comboBox1.SelectedIndex = 0;
                }
            }
        }
        private void cboZMNPCLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboZMNPCLevel.SelectedItem != null)
                Tools.ShowNPCLevelList(lBoxAllUnit, _heroList, _schoolList, cboZMNPCLevel.SelectedIndex, cboZMNPCLevel.Text, checkBox2.Checked, checkBox3.Checked);
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (cboZMNPCLevel.SelectedItem != null)
                Tools.ShowNPCLevelList(lBoxAllUnit, _heroList, _schoolList, cboZMNPCLevel.SelectedIndex, cboZMNPCLevel.Text, checkBox2.Checked, checkBox3.Checked);
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (cboZMNPCLevel.SelectedItem != null)
                Tools.ShowNPCLevelList(lBoxAllUnit, _heroList, _schoolList, cboZMNPCLevel.SelectedIndex, cboZMNPCLevel.Text, checkBox2.Checked, checkBox3.Checked);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                for (int i = 0; i < lBoxSchoolDie.Items.Count; i++)
                {
                    lBoxSchoolDie.SelectedIndex = i;
                }
            }
            else
                lBoxSchoolDie.SelectedIndex = -1;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lBoxSchool.SelectedItem != null)
                SaveSchoolShuXing(lBoxSchool.SelectedItem as School);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ReviveSchoolUnit();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddSchoolUnit();
        }
        private void btnChanged_Click(object sender, EventArgs e)
        {
            ChangedSchoolUnit();
        }
        private void btnDel_Click(object sender, EventArgs e)
        {
            DelSchoolUnit();
        }
        private void btnYJQM_Click(object sender, EventArgs e)
        {
            SetGongFaJiYi();
        }


        #endregion

        private void label73_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://bbs.3dmgame.com/thread-6132777-1-1.html");
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {
                lBoxBakDIr.Items.Clear();
                foreach (DirectoryInfo item in new DirectoryInfo(Application.StartupPath + "\\bak").GetDirectories())
                {
                    lBoxBakDIr.Items.Add(item.Name);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lBoxBakDIr.SelectedItem != null)
            {
                Tools.CopyDir(Application.StartupPath + "\\bak\\" + lBoxBakDIr.SelectedItem.ToString(), txtJsonPath.Text);
                MessageBox.Show("恢复成功");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Application.StartupPath + "\\bak");
                di.Delete(true);
                lBoxBakDIr.Items.Clear();
            }
            catch { }
            finally { Directory.CreateDirectory(Application.StartupPath + "\\bak"); }
        }
    }
}
