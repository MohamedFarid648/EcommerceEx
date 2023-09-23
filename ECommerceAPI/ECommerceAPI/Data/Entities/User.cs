using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Data.Entities
{

    [Index(nameof(User.UserName), IsUnique = true)]
    [Index(nameof(User.EmailAddress), IsUnique = true)]

    public class User
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public  required string EmailAddress { get; set; }
        public  DateTime lastLoginTime { get; set; }

    }
}
