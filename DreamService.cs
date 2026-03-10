using Dream_Journal_Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Dream_Journal_Project
{
    public class DreamService
    {
        public ObservableCollection<Dream> Dreams { get; set; } = new ObservableCollection<Dream>();


        public DreamService()
        {
            // Initialize with some sample data
            Dreams.Add(new Dream(1, "Flying in the sky", "I was flying over a beautiful landscape."));
            Dreams.Add(new Dream(2, "Being chased", "I was being chased by a mysterious figure."));
        }

        public void AddDream(Dream dream)
        {
            dream.Id = Dreams.Count + 1; // Assign a new ID based on the count
            Dreams.Add(dream);
        }
    }
}
