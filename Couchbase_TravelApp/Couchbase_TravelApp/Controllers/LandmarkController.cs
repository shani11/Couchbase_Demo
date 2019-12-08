﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.Search;
using Couchbase.Search.Queries.Simple;
using Couchbase_TravelApp.Models;
using Microsoft.AspNetCore.Mvc;
using try_cb_dotnet.Models;

namespace Couchbase_TravelApp.Controllers
{
    public class LandmarkController : Controller
    {
        private readonly IBucket bucket;
        public LandmarkController(IBucketProvider bucket)
        {
            this.bucket = bucket.GetBucket("travel-sample");
        }
        [HttpGet]
        public IActionResult SearchAddress(string address = "")
        {
            if (!string.IsNullOrEmpty(address))
            {
                var query = new SearchQuery
                {
                    Index = "fts_index",
                    Query = new MatchQuery(address)
                }
        .Limit(10);
                var result = bucket.Query(query);
                List<Landmark> landmarks = new List<Landmark>();

                ISearchQueryResult searchQueryRows = result;
                foreach (var item in searchQueryRows)
                {
                   var landmark = bucket.GetDocument<Landmark>(item.Id).Document.Content;

                  landmarks.Add(landmark);
                }

                return View(landmarks);
            }
            else
            {
                List<Landmark> landmarks = new List<Landmark>();
                return View(landmarks);
            }

        }
        
    }
}