using System;

namespace RestuarantWebApi.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class User :  TokenInfo, ICloneable
    {
        public int userId { get; set; }

        public string email { get; set; }

        public int mobileNo { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }



        /// <summary>
        /// Deep copy of object for cloning. 
        /// </summary>
        /// <returns>returns User</returns>
        public object Clone()
        {
            User user = new User();
            user.userId = this.userId;
            user.username = this.username;
            user.password = this.password;
            user.mobileNo = this.mobileNo;
            user.ValidFrom = this.ValidFrom;
            user.ValidTo = this.ValidTo;
            user.Token = this.Token;
            user.Uniquekey = this.Uniquekey;
            return user;
        }
    }
}
