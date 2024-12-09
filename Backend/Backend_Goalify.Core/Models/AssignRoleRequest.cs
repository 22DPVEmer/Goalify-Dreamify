namespace Backend_Goalify.Core.Models
{
    public class AssignRoleRequest
    {
        public string UserId { get; set; } // The ID of the user to assign the role to
        public string Role { get; set; }     // The role to assign
    }
}