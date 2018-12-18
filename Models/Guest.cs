using System.ComponentModel.DataAnnotations;

namespace Activity.Models
{
    public class Guest
    {
        [Key]
        public int GuestId {get;set;}
        public int UserId {get;set;}
        public int ActivityId {get;set;}
        public User User {get;set;}
        

    }
}