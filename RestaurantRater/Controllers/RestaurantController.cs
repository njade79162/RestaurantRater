﻿using RestaurantRater.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestaurantRater.Controllers
{
    public class RestaurantController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();
        
        // POST (create)
        // api/Restaurant
        [HttpPost]
        public async Task<IHttpActionResult> CreateRestaurant([FromBody] Restaurant model)
        {
            if(model is null)
            {
                return BadRequest("Your request body cannot be empty.");
            }

            // if the model is valid
            if (ModelState.IsValid)
            {
                // store the model in the database
                _context.Restaurants.Add(model);
                int changeCount = await _context.SaveChangesAsync();

                return Ok("Your restaurant was created");
            }

            // the model is not valid, reject it
            return BadRequest(ModelState);
        }

        // GET ALL
        // api/Restaurant
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Restaurant> restaurants = await _context.Restaurants.ToListAsync();
            return Ok(restaurants);
        }

        // GET BY ID
        // api/Restaurant/{id}
        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if(restaurant != null)
            {
                return Ok(restaurant);
            }

            return NotFound();
        }

        // PUT (update)
        // api/Restaurant/{id}
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRestaurant([FromUri] int id, [FromBody] Restaurant updatedRestaurant)
        {
            // check if ids match
            if(id != updatedRestaurant?.Id)
            {
                return BadRequest("Ids do not match.");
            }

            // check the ModelState
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // find the restaurant in the database
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            // if the restaurant does not exist then do something
            if (restaurant is null)
            {
                return NotFound();
            }

            // update the properties
            restaurant.Name = updatedRestaurant.Name;
            restaurant.Address = updatedRestaurant.Address;

            // save the changes
            await _context.SaveChangesAsync();

            return Ok("The restaurant was updated!");
        }

        // DELETE (delete)
        // api/Restaurant/{id}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRestaurant([FromUri] int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if(restaurant is null)
            {
                return NotFound();
            }

            _context.Restaurants.Remove(restaurant);

            if(await _context.SaveChangesAsync() == 1)
            {
                return Ok("The restaurant was deleted");
            }

            return InternalServerError();
        }
    }
}
