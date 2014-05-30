using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TMDbLib.Client;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using WebApplication2.Models;

//Controller for the homepage.

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        public static TMDbClient client = new TMDbClient("c83a7fa22254541c8eab822a78b7133c");
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewMovie(int idMovie)
        {
            MovieViewModel model = new MovieViewModel();
            model.links = new List<Link>();
            Movie movie = client.GetMovie(idMovie);
            movie.Credits = client.GetMovieCredits(idMovie);
            model.movie = movie;
            model.root = getMovieCast(movie);
            model.rootCrew = getMovieCrew(movie);
            //model.links.AddRange(links);        
            return View(model);
        }
        private Node getMovieCast(Movie movie) {
            movie.Credits = client.GetMovieCredits(movie.Id);
            Node root = new Node("root", movie.Title, null);
            root._children = null;
            List<int> NodeIds = new List<int>();
            var first15 = movie.Credits.Cast.Count > 15 ? movie.Credits.Cast.GetRange(0, 14) : movie.Credits.Cast;
            foreach (Cast member in first15)
            {
                Node child = new Node("primary", member.Name, null);
                child._children = null;
                TMDbLib.Objects.People.Credits credits = client.GetPersonCredits(member.Id);
                foreach(MovieRole submember in credits.Cast){
                    if (submember.Id != movie.Id) { 
                        Node grandChild = new Node("secondary", submember.Title, null);
                        grandChild.children=null;
                        grandChild._children = null;
                        child.children.Add(grandChild);
                    }
                }
                root.children.Add(child);
            }
            return root;
        }
        private Node getMovieCrew(Movie movie)
        {
            movie.Credits = client.GetMovieCredits(movie.Id);
            Node root = new Node("root", movie.Title, null);
            root._children = null;
            List<int> NodeIds = new List<int>();
            var first15 = movie.Credits.Crew.Count > 15 ? movie.Credits.Crew.GetRange(0, 14) : movie.Credits.Crew;
            foreach (Crew member in first15)
            {
                Node child = new Node("primary", member.Name, null);
                child._children = null;
                TMDbLib.Objects.People.Credits credits = client.GetPersonCredits(member.Id);
                foreach (MovieJob submember in credits.Crew)
                {
                    if (submember.Id != movie.Id)
                    {
                        Node grandChild = new Node("secondary", submember.Title, null);
                        grandChild.children = null;
                        grandChild._children = null;
                        child.children.Add(grandChild);
                    }
                }
                root.children.Add(child);
            }
            return root;
        }
        //Get changes to the timeline to update results.
        [HttpPost]
        public String SetTimeline(int min, int max)
        {
            SearchContainer<SearchMovie> result = new SearchContainer<SearchMovie>();
            result.Results = new List<SearchMovie>();

            result = client.DiscoverMovies(page: 1, sortBy: DiscoverMovieSortBy.PopularityDescending, releaseDateGreaterThan: new DateTime(min, 1, 1), releaseDateLessThan: new DateTime(max, 12, 31));
            for (int cont = 2; cont <= 40 && cont <= result.TotalPages; cont++)
            {
                result.Results.InsertRange(0, client.DiscoverMovies(page: cont, sortBy: DiscoverMovieSortBy.PopularityDescending, releaseDateGreaterThan: new DateTime(min, 1, 1), releaseDateLessThan: new DateTime(max, 12, 31)).Results);
            }
            result.Results.RemoveAll(NotWellRated);
            result.Results = result.Results.OrderByDescending(o => o.VoteAverage).ThenByDescending(o => o.Popularity).ToList();
            String movies="";
            foreach (SearchMovie m in  result.Results) {
                Movie mov = client.GetMovie(m.Id);
                mov.Credits = client.GetMovieCredits(m.Id);
                String dir = "";
                List<Crew> dirs = mov.Credits.Crew.FindAll(DirectorsInMovie);
                foreach(Crew cr in dirs){
                    dir+=cr.Name+" ";
                }
                String writ = "";
                List<Crew> writers = mov.Credits.Crew.FindAll(DirectorsInMovie);
                foreach(Crew cr in writers){
                    writ+=cr.Name+" ";
                }
                int cat = Convert.ToInt32(m.VoteAverage);
                movies+=m.Id+","+m.Title.Replace(",","")+",http://image.tmdb.org/t/p/w92/" + m.PosterPath +","+m.ReleaseDate.Value.Year+","+dir+","+writ+","+m.VoteAverage+","+m.VoteCount+","+cat+", EOL \n";
            }

            return movies;//Json(result, JsonRequestBehavior.AllowGet);
        }
        private static bool NotWellRated(SearchMovie m)
        {
            return m.VoteAverage < 7 || (m.Popularity < 20 && m.VoteAverage < 7) || (m.Popularity < 10 && m.VoteAverage < 8);
        }
        private static bool NotSoWellRated(SearchMovie m)
        {
            return  m.VoteCount < 500 || m.VoteAverage < 7;
        }
        private static bool DirectorsInMovie(Crew c)
        {
            return c.Job=="Director";
        }
        private static bool WritersInMovie(Crew c)
        {
            return c.Job == "Writer";
        }
        [HttpPost]
        public JsonResult FindMovies(string key)
        {
            SearchContainer<SearchMovie> movies = new SearchContainer<SearchMovie>();
            movies = client.SearchMovie(key);
            for (int cont = 2; cont <= 15 && cont <= movies.TotalPages; cont++)
            {
                movies.Results.InsertRange(0, client.SearchMovie(key, page: cont).Results);
            }
            movies.Results = movies.Results.OrderByDescending(o => o.Popularity).ToList();
            return Json(movies, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MostPopular()
        {               
            return View();
        }
        public ActionResult PopularActors()
        {
            return View();
        }

        [HttpPost]
        public String SetTimelineActors(int min, int max)
        {
            SearchContainer<SearchMovie> result = new SearchContainer<SearchMovie>();
            result.Results = new List<SearchMovie>();

            result = client.DiscoverMovies(page: 1, sortBy: DiscoverMovieSortBy.PopularityDescending, releaseDateGreaterThan: new DateTime(min, 1, 1), releaseDateLessThan: new DateTime(max, 12, 31));
            for (int cont = 2; cont <= 15 && cont <= result.TotalPages; cont++)
            {
                result.Results.InsertRange(0, client.DiscoverMovies(page: cont, sortBy: DiscoverMovieSortBy.PopularityDescending, releaseDateGreaterThan: new DateTime(min, 1, 1), releaseDateLessThan: new DateTime(max, 12, 31)).Results);
            }
            result.Results.RemoveAll(NotSoWellRated);
            result.Results = result.Results.OrderByDescending(o => o.VoteAverage).ThenByDescending(o => o.Popularity).ToList();
            
            
            String actors = "";
            foreach (SearchMovie m in result.Results)
            {
                List<int> actorIds = new List<int>();
                Movie mov = client.GetMovie(m.Id);
                mov.Credits = client.GetMovieCredits(m.Id);
                List<Cast> first5 = mov.Credits.Cast.Count > 4 ? mov.Credits.Cast.GetRange(0, 5) : mov.Credits.Cast;
                foreach (Cast c in first5){ 
                    if(!actorIds.Contains(c.Id)){
                        Person p = client.GetPerson(c.Id);
                        actors += ","+c.Name;
                        actors += "," + c.Id;
                        if (p.Images != null && p.Images.Profiles != null && p.Images.Profiles.First() != null)
                        {
                            actors += "," + p.Images.Profiles.First().FilePath;
                        }else { actors += ","; }
                        
                        TMDbLib.Objects.People.Credits  actorCreds = client.GetPersonCredits(c.Id);
                        int firstYear = actorCreds.Cast.FindAll(HasDate).OrderBy(o=>o.ReleaseDate).First().ReleaseDate.Value.Year;
                        int lastYear = actorCreds.Cast.FindAll(HasDate).OrderBy(o=>o.ReleaseDate).Last().ReleaseDate.Value.Year;
                        string interval = firstYear + "-" + lastYear;
                        actors += "," + interval;
                        p.Credits = client.GetPersonCredits(c.Id);
                        List<Movie> myMovies = new List<Movie>();
                        foreach(MovieRole member in p.Credits.Cast){
                            Movie current = client.GetMovie(member.Id);
                            if (current.ReleaseDate != null && current.VoteCount>500 && current.VoteAverage>5)
                            {
                                myMovies.Add(current);
                            }
                        }
                        int z = myMovies.FindAll(delegate(Movie item)
                        {
                            return item.VoteAverage >=9;
                        }).Count;
                        int y = myMovies.FindAll(delegate(Movie item)
                        {
                            return item.VoteAverage >= 8 && item.VoteAverage <9;
                        }).Count;
                        int x = myMovies.FindAll(delegate(Movie item)
                        {
                            return item.VoteAverage >= 7 && item.VoteAverage < 8;
                        }).Count;
                        actors += ","+z+","+y+","+x;
                        int score = (12 * z)+(7*y)+(2*x)+p.Credits.Cast.Count;
                        actors += "," + score;
                        int range = (lastYear - firstYear) / 5;
                        for (int cont = 0; cont < 5; cont++) {
                            Movie mymov = myMovies.FindAll(delegate(Movie item)
                        {
                            return item.ReleaseDate!=null && item.ReleaseDate.Value.Year >= firstYear + (cont * range) && item.ReleaseDate.Value.Year<firstYear+((cont+1) * range);
                        }).OrderBy(o=>o.VoteAverage).FirstOrDefault();
                            if (mymov != null)
                            {
                                actors += "," + mymov.Id + "," + mymov.Title;
                            }
                            else { 
                                actors+=",,";
                            }
                        }
                        
                    }
                }
                actors += ",EOL";
            }
            string ret = actors.Length>0?actors.Substring(1):actors;
            return ret;
        }

        private static bool HasDate(MovieRole m)
        {
            return m.ReleaseDate!=null;
        }
    }
}
