
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Activity.Models
{
    public class ActivitySchedule
    {
        [Key]
        public int ActivityId {get;set;}

            [Required]
            [MinLength(2, ErrorMessage="Title Should be 2 charactr long!!")]       
        public string Title {get;set;}

            [Required]
            // [RegularExpression(@"\b((1[0-2]|0?[1-9]):([0-5][0-9]) ([AaPp][Mm]))", ErrorMessage="Please Enter correct  time is chosen hh/mm AM or PM")]
            public string Time {get;set;} 

        [RegularExpression(@"^[+]?\d+([.]\d+)?$", ErrorMessage = "Hint: positive numbers allowed")]
        public int Duration {get;set;}

            [Required]
        public DateTime Date {get;set;}

            [Required]
        
            [MinLength(2, ErrorMessage="Description Should be 2 charactr long!!")] 
        public string Description {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public int UserId {get;set;}
        public List<Guest> guest {get;set;}

    }
}