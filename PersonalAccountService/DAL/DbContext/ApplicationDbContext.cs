using DAL.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DbContext
{
    public class ApplicationDbContext
    {
        private readonly MongoClient mongoClient;
        private readonly IMongoDatabase database;

        public IMongoCollection<User> Users { get
            {
                var users = database.GetCollection<User>("users");

                if(users == null)
                {
					database.CreateCollection("users");
                }

                return database.GetCollection<User>("users");
			} 
        }

        public IMongoCollection<UserData> UsersData
        {
            get
            {
				var usersdata = database.GetCollection<UserData>("user_data");

				if (usersdata == null)
				{
					database.CreateCollection("user_data");
				}

				return database.GetCollection<UserData>("user_data");
			}
        }

        public IMongoCollection<RefreshToken> RefreshTokens
        {
            get
            {
                var tokens = database.GetCollection<RefreshToken>("refresh_token");

                if(tokens == null)
                {
                    database.CreateCollection("refresh_token");
                }

				return database.GetCollection<RefreshToken>("refresh_token");
			}
        }

        public ApplicationDbContext(string connection, string dbName)
        {
            mongoClient = new MongoClient(connection);
            database = mongoClient.GetDatabase(dbName);
        }
    }
}
