using Microsoft.Extensions.Configuration;
using RestuarantWebApi.DbService;
using RestuarantWebApi.Model;
using RestuarantWebApi.Repository;
using RestuarantWebApi.Utils;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RestuarantWebApi.SqlService
{
    /// <summary>
    /// Class UserService used to interact with Database.
    /// </summary>
    public class UserService : IUserRepository
    {

        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public UserService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public User AddUser(User user)
        {
            User u = null;
            using (DbServicecls service = new DbServicecls(Configuration))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UserTransaction";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username",user.username);
                cmd.Parameters.AddWithValue("@password", user.password);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@mobileNo", user.mobileNo);
                cmd.Parameters.AddWithValue("@validFrom", user.ValidFrom);
                cmd.Parameters.AddWithValue("@ValidTo", user.ValidTo);
                cmd.Parameters.AddWithValue("@mode", "Insert");
                DataSet ds = service.Transaction(cmd);  
                if(ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    u = CollectionHelper.convetDataTableToSingleRecord<User>(ds.Tables[0]);
                }
            }
            return u;
        }

        /// <summary>
        /// Used to Delete User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User DeleteUser(User user)
        {
            return null;
        }

      
        /// <summary>
        /// Abhi shah : Used to Get Username and password.
        /// </summary>
        /// <param name="username">username.</param>
        /// <param name="password">password.</param>
        /// <returns></returns>
        public User GetUser(string username, string password)
        {
            User u = null;
            using (DbServicecls service = new DbServicecls(Configuration))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "GetUserDetails";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                DataSet ds = service.GetAllRecords(cmd);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    u = CollectionHelper.convetDataTableToSingleRecord<User>(ds.Tables[0]);
                }
            }
            return u;
        }

        /// <summary>
        /// Get List of Users.
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            return null;
        }

        /// <summary>
        /// Used to Update User detail password.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User UpdateUserDetails(User user)
        {
            return null;
        }

        /// <summary>
        /// Used to get UserGuid for Generating Token.
        /// </summary>
        /// <param name="info">Token Info.</param>
        /// <returns>returns string that used to create token.</returns>
        public string GetUserGuId(TokenInfo info)
        {
            using (DbServicecls db = new DbServicecls(Configuration))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "GetUserGuid";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", info.username);
                cmd.Parameters.AddWithValue("@Issuetime", info.Issuetime);
                object obj = db.GetScalerValue(cmd);
                return obj.ToString();
            }
        }
    }
}
