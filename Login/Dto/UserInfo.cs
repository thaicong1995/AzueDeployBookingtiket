using Data.Models.Enum;

namespace AirPlane.Dto
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string aliases { set; get; }
        public string Name { set; get; }
        public MemberTypes memberTypes { set; get; }
        public Roles roles { set; get; }
    }
}
