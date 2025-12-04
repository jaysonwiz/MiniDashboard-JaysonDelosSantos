using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class ProductUpdateRequest
    {
        [Required(ErrorMessage = "Product ID can't be blank")]
        public Guid ProductID { get; set; }
        [Required(ErrorMessage = "Product name can't be blank")]
        [StringLength(50, ErrorMessage = "Product name should not exceed 50 characters")]
        public string? ProductName { get; set; }

        public Product ToProduct()
        {
            return new Product()
            {
                ProductID = ProductID,
                ProductName = ProductName
            };
        }
    }
}
