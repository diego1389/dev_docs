using System.Collections.Generic;
using OdeToFood.Core;
using System.Linq;

namespace OdeToFood.Data
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetRestaurantsByName(string name);
        Restaurant GetById(int restaurantId);
    }

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
    }
}
