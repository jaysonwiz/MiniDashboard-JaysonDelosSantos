using Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class ProductResponse
    {
        public Guid ProductID { get; set; }
        public string? ProductName { get; set; }

        //It compares the current object to another object of ProductResponse type and returns true, if both values are same; otherwise returns false
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(ProductResponse))
                return false;

            ProductResponse countryToCompare = (ProductResponse)obj;

            return this.ProductID == countryToCompare.ProductID && this.ProductName == countryToCompare.ProductName;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    /// <summary>
    /// Extension methods for Product related operations
    /// </summary>
    public static class ProductExtensions
    {
        //Converts from Product object to ProductResponse object
        public static ProductResponse ToProductResponse(this Product product)
        {
            return new ProductResponse
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName
            };
        }
    }
}
