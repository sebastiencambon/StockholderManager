using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StockholderManager.Data
{
  public class Stock
  {
    [Key]
    public int Id { get; set; }

    public Stockholder Holder { get; set; }

    public Stockholder UsuFructHolder { get; set; }

    public decimal Price { get; set; }

    public bool Saleable { get; set; } 

    public DateTimeOffset? SaleDate { get; set; }

    public Stockholder Buyer { get; set; }
  }
}
