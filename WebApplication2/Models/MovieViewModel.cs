using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Status { get; set; }
        public string Tagline { get; set; }
        public string Overview { get; set; }
        public string Homepage { get; set; }
        public string BackdropPath { get; set; }
        public string PosterPath { get; set; }
        public bool Adult { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public long Revenue { get; set; }
        public long Budget { get; set; }
        public int? Runtime { get; set; }
        public double Popularity { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }       
    }
}

