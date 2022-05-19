using System;
using System.Collections.Generic;
using System.Text;
using Project.Data.Entities;

namespace Project.Data.Entities
{
    public class ProductInCategory
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}