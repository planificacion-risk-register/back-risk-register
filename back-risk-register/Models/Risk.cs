namespace back_risk_register.Models
{ //
    public class Risk
    {
        public int id_plan { get; set; }
        public int id_risk { get; set; }
        public string risk_description { get; set; }
        public string impact_description { get; set; }
        public string impact { get; set; }
        public string probability { get; set; }
        public string owner { get; set; }
        public string response_plan { get; set; }
        public string priority { get; set; }
        public bool newT { get; set; }
    }
}
