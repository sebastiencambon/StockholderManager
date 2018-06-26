using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StockholderManager.Data;

namespace StockholderManager.Pages
{
  public class ForSaleStocksModel : PageModel
  {
    private readonly ApplicationDbContext context;
    private readonly UserManager<Stockholder> userManager;

    public ForSaleStocksModel(ApplicationDbContext context, UserManager<Stockholder> userManager)
    {
      this.context = context;
      this.userManager = userManager;
    }

    public IEnumerable<Stock> ForSaleStocks { get; private set; }
    public IEnumerable<Stock> StockSaleToProceed { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
      var user = await userManager.GetUserAsync(User);
      this.ForSaleStocks = context.Stocks.Include(f => f.Holder).Include(f => f.Buyer).Where(f => f.Saleable && f.Holder != user).AsEnumerable();
      StockSaleToProceed = context.Stocks.Include(f => f.Holder).Include(f => f.Buyer).Where(f => f.Buyer != null).AsEnumerable();
      return Page();
    }

    public async Task<IActionResult> OnPostBuyAsync(int id)
    {
      var stock = context.Stocks.FirstOrDefault(f => f.Id == id);
      if (stock != null)
      {
        stock.Buyer = await userManager.GetUserAsync(User);
        stock.Saleable = false;
        await context.SaveChangesAsync();
      }
      return RedirectToPage();
    }

  }
}