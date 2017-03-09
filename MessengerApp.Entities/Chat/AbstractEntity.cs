using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Entities.Chat
{
    public class AbstractEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
