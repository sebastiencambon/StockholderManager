using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StockholderManager.Data;

namespace StockholderManager.Pages.Account.Manage
{
  public class StocksModel : PageModel
  {
    private readonly ApplicationDbContext context;
    private readonly UserManager<Stockholder> userManager;

    public StocksModel(ApplicationDbContext context, UserManager<Stockholder> userManager)
    {
      this.context = context;
      this.userManager = userManager;
    }

    public IEnumerable<Stock> Stocks { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
      var user = await userManager.GetUserAsync(User);
      this.Stocks = context.Stocks.Include(f => f.Holder).Include(f => f.Buyer).Where(f => f.Holder == user).AsEnumerable();
      return Page();
    }

    public async Task<IActionResult> OnPostSaleAsync(int id)
    {
      var stock = context.Stocks.Include(f => f.Buyer).Include(f => f.Holder).FirstOrDefault(f => f.Id == id);
      if (stock != null)
      {
        stock.Saleable = true;
        await context.SaveChangesAsync();
      }
      return RedirectToPage();
    }
  }
}