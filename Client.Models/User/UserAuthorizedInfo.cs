using System;

namespace Client.Models.User
{
    public class UserAuthorizedInfo
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}