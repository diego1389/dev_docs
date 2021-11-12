using System.Collections.Generic;
using OdeToFood.Core;
using System.Linq;

namespace OdeToFood.Data
{
    public class InMemoryRestaurant : IRestaurantData
    {
        List<Restaurant> restaurants;

        public InMemoryRestaurant()
        {
            restaurants = new List<Restaurant>
            {
                new Restaurant
                {
                    Id = 1,
                    Name = "Scotts Pizza",
                    Location = "Maryland",
                    Cuisine = CuisineType.Italian
                },
                new Restaurant
                {
                    Id = 2,
                    Name = "Cinnamon Club",
                    Location = "San Francisco",
                    Cuisine = CuisineType.None
                },
                  new Restaurant
                {
                    Id = 3,
                    Name = "Joe's Tacos",
                    Location = "New Mexico",
                    Cuisine = CuisineType.Mexican
                }
            };
        }

        public Restaurant GetById(int restaurantId)
        {
            return restaurants.SingleOrDefault(restaurant => restaurant.Id == restaurantId);
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string name = null)
        {
            return from restaurant in restaurants
                   where string.IsNullOrEmpty(name) || restaurant.Name.StartsWith(name)
                   orderby restaurant.Name
                   select restaurant;
        }

        public Restaurant Update(Restaurant updatedRestaurant)
        {
            var restaurant = restaurants.SingleOrDefault(r => r.Id == updatedRestaurant.Id);
            if (restaurant != null)
            {
                restaurant.Name = updatedRestaurant.Name;
                restaurant.Location = updatedRestaurant.Location;
                restaurant.Cuisine = updatedRestaurant.Cuisine;
            }
            return restaurant;
        }

        public int Commit()
        {
            return 0;
        }

        public Restaurant Add(Restaurant newRestaurant)
        {
            restaurants.Add(newRestaurant);
            newRestaurant.Id = restaurants.Max(r => r.Id) + 1;
            return newRestaurant;
        }

        public Restaurant Delete(int id)
        {
            var restaurant = restaurants.FirstOrDefault(r => r.Id == id);
            if (restaurant != null)
            {
                restaurants.Remove(restaurant);
            }
            return restaurant;
        }

        public int GetCountOfRestaurants()
        {
            return restaurants.Count();
        }
    }
}
