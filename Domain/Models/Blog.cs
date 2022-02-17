using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    //fields are normally private
    //properties are normally public

    public class Blog
    {
        [Key]
        public int Id { get; set; }
        [Required ] //required makes the property/field compulsory
        public string Name { get; set; }

        public string LogoImagePath { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; } //foreign key
        public virtual Category Category { get; set; } //navigational property
    }
}
