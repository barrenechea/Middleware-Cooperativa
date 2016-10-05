namespace Middleware.Models.API
{
    public class APITabanco
    {
        public int id { get; set; }
        public string codempre { get; set; }
        public string codbanco { get; set; }
        public string nombanco { get; set; }
        public string codctacc { get; set; }
        public string ctacc { get; set; }
        public string ctacontab { get; set; }
        public int chequeact { get; set; }
        public int chequefin { get; set; }
        public int ingreact { get; set; }
        public int ingrefin { get; set; }
        public int egreact { get; set; }
        public int egrefin { get; set; }
        public int trasact { get; set; }
        public int trasfin { get; set; }
        public int compact { get; set; }
        public int compfin { get; set; }
        public int ventact { get; set; }
        public int ventfin { get; set; }
        public int uniact { get; set; }
        public bool estado { get; set; }
        public int ano { get; set; }
        public bool flg_ing { get; set; }
    }
}
