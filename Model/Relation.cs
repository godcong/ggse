namespace guigubahuang
{
    public class Relation
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string HaoGanDu { get; set; }

        public Relation(string id, string name)
        {
            ID = id;
            Name = name;
        }

        public Relation(string id, string name, string haogan)
        {
            ID = id;
            Name = name;
            HaoGanDu = haogan;
        }
    }
}
