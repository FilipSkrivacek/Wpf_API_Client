using wpf_api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace plan
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Calories { get; set; }
        public int Weight { get; set; }

        [JsonIgnore]
        public Franchise? Franchise { get; set; }

        [ForeignKey("Id")]
        public int FranchiseId { get; set; }


    }
}