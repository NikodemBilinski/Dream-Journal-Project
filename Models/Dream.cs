using System;
using System.Collections.Generic;
using System.Text;

namespace Dream_Journal_Project.Models
{
    public class Dream
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public Dream(int id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
            DateCreated = DateTime.Now;
        }

        public Dream() { }
        
    }
}
