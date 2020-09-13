using System;
using System.ComponentModel.DataAnnotations;

namespace EnpalSharpTemplate.Model
{
    public class PayloadModel
    {
        public Guid Id { get; set; }
        [Key]
        [Required]
        public string Key { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
