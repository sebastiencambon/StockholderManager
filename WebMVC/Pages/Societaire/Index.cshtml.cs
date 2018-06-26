using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StockholderManager.Data;

namespace StockholderManager.Pages.Societaire
{
  public class IndexModel : PageModel
  {
    private readonly UserManager<Stockholder> users;
    private readonly ApplicationDbContext context;

    public IndexModel(UserManager<Stockholder> users, ApplicationDbContext context)
    {
      this.users = users;
      this.context = context;
      Stocks = new List<Stock>();
    }

    public string SelectedSocietaire { get; set; }

    public IList<Stockholder> Societaires { get; set; }
    public IEnumerable<Stock> Stocks { get; private set; }

    public IActionResult OnGet(string id)
    {
      SelectedSocietaire = id;
      var societaires = users.Users.ToList();

      Societaires = context.Users.Include(f => f.Stocks).Where(f => societaires.Any(s => s.Id == f.Id)).ToList();
      if (id != null)
        Stocks = context.Stocks.Include(f => f.Holder).Where(f => f.Holder.Id == id).AsEnumerable().ToList();

      return Page();
    }
  }
}