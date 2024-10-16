using System;
namespace UserManagementService.Settings
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UserCollectionName { get; set; }
        private readonly IConfiguration _configuration;

        public MongoDBSettings(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetSection("MongoDBSettings:ConnectionString").Value;
            DatabaseName = _configuration.GetSection("MongoDBSettings:DatabaseName").Value;
            UserCollectionName = _configuration.GetSection("MongoDBSettings:UserCollectionName").Value;
        }

        public string GetConnectionString()
        {
            return ConnectionString;
        }
        public string GetDatabaseName()
        {
            return DatabaseName;
        }
        public string GetUserCollectionName()
        {
            return UserCollectionName;
        }
    }
}
