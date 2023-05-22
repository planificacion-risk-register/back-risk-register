namespace back_risk_register.DataBase
{

    public class Connection
    {

        private string connectionString = string.Empty;

        public Connection()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            connectionString = builder.GetConnectionString("DefaultConnection");
        }

        public String getConnection()
        {
            return connectionString;
        }
    }
}
