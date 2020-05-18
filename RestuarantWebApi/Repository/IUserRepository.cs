using RestuarantWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestuarantWebApi.Repository
{
    public interface IUserRepository
    {
         User AddUser(User user);

         List<User> GetUsers();

         User GetUser(string username,string password);

         User UpdateUserDetails(User user);

         User DeleteUser(User user);

         string GetUserGuId(TokenInfo info);
    }
}
