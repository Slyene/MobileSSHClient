using SQLite;

namespace MobileSSHClient.Models
{
    public class Connection
    {
        [PrimaryKey, Indexed ,AutoIncrement, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Ip { get; set; }

        [NotNull]
        public string Username { get; set; }

        [NotNull]
        public string Password { get; set; }
    }
}
