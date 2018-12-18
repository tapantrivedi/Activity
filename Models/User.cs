using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Activity.Models
{
    public class User
    {
        [Key]
        
        public int UserId {get;set;}

        [Required(ErrorMessage = "First Name cannot be empty!!")]
        [MinLength(2,ErrorMessage="First name is required and must be at least 2 characters long and must be characters only")]
        [RegularExpression("^[a-zA-Z ]*$")]
        public string FirstName {get;set;}

            [Required(ErrorMessage=" Last Name cannot be Empty!!")]
        [MinLength(2,ErrorMessage="Last name is required and must be at least 2 characters long and must be characters only")]
        [RegularExpression("^[a-zA-Z ]*$")]
        public string LastName {get;set;}

            [Required(ErrorMessage="Email cannot be empty!!")]
            [EmailAddress]
        public string Email {get;set;}

            [Required]
        [MinLength(8, ErrorMessage = "Password must contain 1 letter, 1 number and 1 special character and MUST be 8 characters long")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage = "Password must contain 1 letter, 1 number and 1 special character and MUST be 6 characters long")]

            [DataType(DataType.Password)]
            
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

            [NotMapped]
            [Compare("Password", ErrorMessage="Password and confirm password did not match!!")]
            [DataType(DataType.Password)]
        public string ConfirmPassword {get;set;}

        public List<ActivitySchedule> activitySchedule {get;set;}
        public List<Guest> guest {get;set;}
    }
}