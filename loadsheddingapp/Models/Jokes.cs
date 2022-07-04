using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace loadsheddingapp.Models
{
    public class Jokes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [MaxLength(500)]
        public string Body { get; set; }
        public DateTime TimeCreated { get; set; }
        public bool IsApproved { get; set; }

        public Jokes()
        {
        }

        public Jokes(int id, string username, string body, DateTime timeCreated, bool isApproved)
        {
            Id = id;
            Username = username;
            Body = body;
            TimeCreated = timeCreated;
            IsApproved = isApproved;
        }
    }
}
