using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Couchbase_TravelApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Couchbase_TravelApp.Controllers
{
    public class HotelsController : Controller
    {
        private readonly IBucket _bucket;
        public HotelsController(IBucketProvider bucketProvider)
        {
            _bucket = bucketProvider.GetBucket("travel-sample");
        }
        [HttpGet("hotels/Search")]
        public IActionResult Search(int rating)
        {
            string query;
            IEnumerable<Hotel> hotels;

            query = $"SELECT h.name,h.address,h.city,h.description from `travel-sample` as h UNNEST reviews AS r WHERE " +
                $"r.ratings.Rooms <{rating} limit 5";
            hotels = _bucket.QueryAsync<Hotel>(query).Result.ToList();



            return View(hotels);
        }
    }
}