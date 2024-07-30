namespace WalkProject.API.GraphQL.DTOs.Users
{
    public class UserProfileInput
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
        public DateTime? Dob { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
