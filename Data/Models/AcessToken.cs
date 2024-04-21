using Data.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class AcessToken
    {

        [Key]
        public int Id { get; set; }
        public int UserID { get; set; }
        public string AccessToken { get; set; }

        public StatusToken statusToken { get; set; }

        public DateTime ExpirationDate { get; set; } = DateTime.Now;
    }
}
