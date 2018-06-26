using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace StockholderManager.Data
{
  // Add profile data for application users by adding properties to the ApplicationUser class
  public class Stockholder : IdentityUser
  {
    public Stockholder()
    {
      Stocks = new List<Stock>();
    }

    [MaxLength(2)]
    public string CatAS { get; set; }

    public string ZipCode { get; set; }
    public string Address { get; set; }
    public string DCD { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public string City { get; set; }

    public ICollection<Stock> Stocks { get; set; }
  }
}
