using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class Post
    {
        [Key] //primary key making also the property an identity (auto increment)
        public int Id { get; set; }
        public string Title { get; set; }

        //Ctrl+. opens the intellisense for suggestions
        [ForeignKey("Blog")]  //this attribute means that the foreign key represents the Property "Blog" for reference
        public int BlogId { get; set; } //Foreign Key
        public virtual Blog Blog { get; set; } //reference to the other table
        
        public DateTime DateCreated { get; set; }
        public string Author { get; set; }
    }
}
