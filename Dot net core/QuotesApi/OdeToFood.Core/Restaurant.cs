using System.ComponentModel.DataAnnotations;

namespace OdeToFood.Core
{
    public class Restaurant
    {
        public int Id { get; set; }
        [Required, MaxLength(80)]
        public string Name  { get; set; }
        [Required, MaxLength(256)]
        public string Location { get; set; }
        public CuisineType Cuisine { get; set; }
    }
}
