namespace guigubahuang
{
    public class School
    {
        public string ID { get; set; }
        public string Name1ID { get; set; }
        public string Name2ID { get; set; }
        public string Name { get; set; }
        public string Name2
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                    return ID;
                return Name;
            }
        }
        public bool IsMainSchool { get; set; }
        public string LingShi { get; set; } //money
        public string ZongMenRongYu { get; set; }   //reputation
        public string JiMingDiZi { get; set; }  //totalMember
        public string FanRong { get; set; } //propertyData/prosperous
        public string ZhongCheng { get; set; }  //propertyData/loyal
        public string AnDing { get; set; }  //manorData/mainManor/stable
        public string YaoCai { get; set; }  //propertyData/medicina
        public string KuangCai { get; set; }    //propertyData/mine
        public string ZongZhu { get; set; }
        public string[] DaZhangLao { get; set; }
        public string[] ZhangLao { get; set; }
        public string[] ZhenChuan { get; set; }
        public string[] NeiMen { get; set; }
        public string[] WaiMen { get; set; }
    }
}
