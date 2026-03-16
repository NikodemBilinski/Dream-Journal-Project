using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Dream_Journal_Project.Models
{
    public class Dream
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public bool LucidDream { get; set; }


        
    }
}
