using System.Data.SqlTypes;

namespace back_risk_register.Models
{
    public class TaskRegister
    {
        public int id_plan { get; set; }
        public int id_project { get; set; }
        public int id_task { get; set; }
        public string task_name { get; set; } 
        public DateTime last_update { get; set;}
        public int risk_count { get; set;}
        public int total_points { get;set;}

    }
}
