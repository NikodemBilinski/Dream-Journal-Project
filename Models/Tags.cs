using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dream_Journal_Project.Models
{
   
        public class Tag
        {
            [PrimaryKey, AutoIncrement]

            public int Id { get; set; }

            public string Name { get; set; }

            public string ColorHex { get; set; }

            public bool IsActive { get; set; }
        }
    
}
