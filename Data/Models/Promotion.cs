using Data.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Promotion
    {
        [Key]
        public int Id { set; get; }
        public string PromotionNam { set; get; }
        public string Describe { set; get; }
        public double Value { set; get; }
        public MemberTypes ApplicableMemberType { get; set; }

    }
}
