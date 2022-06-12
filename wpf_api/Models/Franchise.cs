using plan;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace wpf_api.Models
{
    public class Franchise
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int YearOfCreation { get; set; }
        public string Country { get; set; }

        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }
    }
}
