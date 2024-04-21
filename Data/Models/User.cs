using Data.Models;
using Data.Models.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string aliases { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public StatusUser status { get; set; }
        public string ActivationToken { get; set; }
        public DateTime? ExpLink { get; set; }
        public MemberTypes memberTypes { set; get; }
        public Roles roles { get; set; }

    }
}
