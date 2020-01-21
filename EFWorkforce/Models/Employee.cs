using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EFWorkforce.Models
{
    public class Employee
    {
        //[Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(55, MinimumLength = 1, ErrorMessage = "First Name length should be between 1 and 55 characters")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(55, MinimumLength = 1, ErrorMessage = "Last Name length should be between 1 and 55 characters")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Employee eMail is required")]
        [DataType(DataType.EmailAddress)]
        [StringLength(99, MinimumLength = 5, ErrorMessage = "eMail length should be between 5 and 99 characters")]
        [Display(Name = "e-Mail")]
        public string Email { get; set; }

        //[Required]
        [Display(Name = "Is supervisor")]
        public bool IsSupervisor { get; set; } = false;

        [Required(ErrorMessage = "Department Id is required")]
        [StringLength(99, MinimumLength = 5, ErrorMessage = "Department Id should be valid integer number")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        //[Required(ErrorMessage = "Computer Id is required")]
        [Display(Name = "Computer")]
        public int ComputerId { get; set; } = -1;
        public Computer Computer { get; set; } // related properties entities
    }
}
