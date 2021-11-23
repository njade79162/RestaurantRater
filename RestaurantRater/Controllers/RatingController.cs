using RestaurantRater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestaurantRater.Controllers
{
    public class RatingController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();

        // create new ratings
        // POST api/Rating
        [HttpPost]
        public async Task<IHttpActionResult> CreateRating([FromBody]Rating model)
        {
            // check if model is null
            if(model is null)
            {
                return BadRequest("Your request body cannot be empty.");
            }

            // check if ModelState is invalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // find the restaurant by the model.RestaurantId and see that it exists
            var restaurantEntity = await _context.Restaurants.FindAsync(model.RestaurantId);
            if(restaurantEntity is null)
            {
                return BadRequest($"The target restaurant with the ID of {model.RestaurantId} does not exist");
            }

            // create the rating

            //// add to the rating table
            // _context.Ratings.Add(model);

            // add to the restaurant entity
            restaurantEntity.Ratings.Add(model);
        }

        // get a rating by its id

        // get all ratings

        // get all ratings for a specific restaurant by the restaurant id

        // update a rating

        // delete a rating
    }
}
