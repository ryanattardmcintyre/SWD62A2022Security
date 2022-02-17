using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        public string Comment { get; set; }
    }
}
