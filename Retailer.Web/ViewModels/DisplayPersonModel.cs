using System.ComponentModel.DataAnnotations;

namespace Retailer.Web.ViewModels
{
    public class DisplayPersonModel
    {
        [Required]
        [StringLength(15,ErrorMessage ="FirstName is too long")]
        [MinLength(1,ErrorMessage ="FirstName is too short")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "LastName is too long")]
        [MinLength(1, ErrorMessage = "LastName is too short")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
