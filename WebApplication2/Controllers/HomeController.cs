﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TMDbLib.Client;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

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
        //Get changes to the timeline to update results.
        [HttpPost]
        public JsonResult SetTimeline(int min, int max)
        {
            SearchContainer<SearchMovie> result = new SearchContainer<SearchMovie>();
            result.Results = new List<SearchMovie>();
            result = client.DiscoverMovies(page: 1, sortBy: DiscoverMovieSortBy.PopularityDescending, releaseDateGreaterThan: new DateTime(min, 1, 1), releaseDateLessThan: new DateTime(max, 12, 31));

            for (int cont = 2; cont <= 15 && cont <= result.TotalPages; cont++)
            {
                result.Results.InsertRange(0, client.DiscoverMovies(page: cont, sortBy: DiscoverMovieSortBy.PopularityDescending, releaseDateGreaterThan: new DateTime(min, 1, 1), releaseDateLessThan: new DateTime(max, 12, 31)).Results);
            }
            result.Results = result.Results.OrderBy(o => o.Popularity).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}