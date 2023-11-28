using System.ComponentModel.DataAnnotations.Schema;

namespace ALLUP2.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public double Costprice { get; set; }
        public double Saleprice { get; set; }
        public double DiscountPercent { get; set; }
        public List<ProductTag>? ProductTags { get; set; } = new List<ProductTag>();
        [NotMapped]
        public List<int> TagIds { get; set; }
    }
}
