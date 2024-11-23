using System.ComponentModel.DataAnnotations.Schema;

namespace WebShell.Models
{
    public class CommandModel
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("command")]
        public string? Command { get; set; }

        [Column("last_usage")]
        public DateTime Last_Usage { get; set; } = DateTime.Now;
    }
}
