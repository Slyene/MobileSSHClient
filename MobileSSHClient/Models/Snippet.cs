using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileSSHClient.Models
{
    public class Snippet
    {
        [PrimaryKey, Indexed, AutoIncrement, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int ConnectionId { get; set; }

        public string Name { get; set; }

        [NotNull]
        public string Command { get; set; }
    }
}
