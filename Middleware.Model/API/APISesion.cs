namespace Middleware.Model.API
{
    public class APISesion
    {
        private string _fecha;
        private string _fechach;
        private string _fechafac;
        public int id { get; set; }
        public string tipo { get; set; }
        public int numero { get; set; }
        public int correl { get; set; }
        public int va_ifrs { get; set; }
        public int canbco { get; set; }
        public string banco { get; set; }
        public string cuenta { get; set; }
        public int cheque { get; set; }
        public string fecha
        {
            get { return _fecha; }
            set { _fecha = value.Replace("00:00:00", string.Empty).Trim(); }
        }
        public string glosa { get; set; }
        public string benefi { get; set; }
        public string fechach
        {
            get { return _fechach; }
            set { _fechach = value.Replace("00:00:00", string.Empty).Trim(); }
        }
        public string area { get; set; }
        public int linea { get; set; }
        public string codigo { get; set; }
        public string tipdoc { get; set; }
        public string fechafac
        {
            get { return _fechafac; }
            set { _fechafac = value.Replace("00:00:00", string.Empty).Trim(); }
        }
        public int fac { get; set; }
        public int corrfac { get; set; }
        public string detalle1 { get; set; }
        public string detalle2 { get; set; }
        public string detalle3 { get; set; }
        public string detalle4 { get; set; }
        public string imp { get; set; }
        public int debe { get; set; }
        public int haber { get; set; }
        public string estado { get; set; }
    }
}
