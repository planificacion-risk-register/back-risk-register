namespace back_risk_register.Models
{
    public class TaskRegister
    {
        public int id_plan { get; set; }
        public string id_project { get; set; }
        public string id_task { get; set; }
        public string task_name { get; set; } 
        public string last_update { get; set;}
        public string risk_count { get; set;}
        public string total_points { get;set;}

    }
}
