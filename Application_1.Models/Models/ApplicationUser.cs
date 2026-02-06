using System.ComponentModel.DataAnnotations;
// using Microsoft.AspNetCore.Identity.IdentityUser;
using Microsoft.AspNetCore.Identity;

namespace Application_1.Models.Models{

    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage ="Name is Required")]
        [MaxLength(100)]
        public string? Name{set;get;}
        [Required(ErrorMessage ="Street Address is Required")]
        [MaxLength(100)]
        public string? StreetAddress{set;get;}
        [Required(ErrorMessage ="City is Required")]
        [MaxLength(100)]
        public string? City{set;get;}
        [Required(ErrorMessage ="State is Required")]
        [MaxLength(100)]
        public string? State{set;get;}


        [Required(ErrorMessage ="Postal Code is Required")]
        [MaxLength(20)]
        public string? PostalCode{set;get;}
    }
}