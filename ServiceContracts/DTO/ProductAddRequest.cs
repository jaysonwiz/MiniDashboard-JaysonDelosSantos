using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for adding a new category
    /// </summary>
    public class ProductAddRequest
    {
        [Required(ErrorMessage = "Product name can't be blank")]
        [StringLength(50,ErrorMessage = "Product name should not exceed 50 characters")]
        public string? ProductName { get; set; }

        public Product ToProduct()
        {
            return new Product()
            {
                ProductName = ProductName
            };
        }
    }
}
