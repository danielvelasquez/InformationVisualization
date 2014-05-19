using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TMDbLib.Objects.Movies;

namespace WebApplication2.Models
{
    public class MovieViewModel
    {
        public Movie movie;
        public List<Link> links;
        public Node root;
        public Node rootCrew;
    }
}

