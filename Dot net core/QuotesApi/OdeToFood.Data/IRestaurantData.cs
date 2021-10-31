using System.Collections.Generic;
using OdeToFood.Core;
using System.Linq;

namespace OdeToFood.Data
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetAll();
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
        public IEnumerable<Restaurant> GetAll()
        {
            return from restaurant in restaurants
                   orderby restaurant.Name
                   select restaurant;
        }
    }
}
