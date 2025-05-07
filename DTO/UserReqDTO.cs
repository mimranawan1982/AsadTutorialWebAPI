using System.ComponentModel.DataAnnotations;

namespace AsadTutorialWebAPI.DTO
{
    public class UserReqDTO
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
