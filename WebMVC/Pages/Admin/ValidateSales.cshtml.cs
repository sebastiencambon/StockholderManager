using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StockholderManager.Data;

namespace StockholderManager.Pages.Admin
{
  public class ValidateSalesModel : PageModel
  {
    private readonly ApplicationDbContext context;
    private readonly UserManager<Stockholder> userManager;

    public ValidateSalesModel(ApplicationDbContext context, UserManager<Stockholder> userManager)
    {
      this.context = context;
      this.userManager = userManager;
    }

    public IEnumerable<Stock> StockSaleToProceed { get; private set; }

    public IActionResult OnGet()
    {
      StockSaleToProceed = context.Stocks.Include(f => f.Holder).Include(f => f.Buyer).Where(f => f.Buyer != null).AsEnumerable();
      return Page();
    }

    public async Task<IActionResult> OnPostCancelSaleAsync(int id)
    {
      var stock = context.Stocks.Include(f => f.Buyer).FirstOrDefault(f => f.Id == id);
      if (stock != null)
      {
        stock.Buyer = null;
        stock.Saleable = true;
        await context.SaveChangesAsync();
      }
      return RedirectToPage();
    }


    public async Task<IActionResult> OnPostValidateSaleAsync(int id)
    {
      var stock = context.Stocks.Include(f => f.Buyer).Include(f => f.Holder).FirstOrDefault(f => f.Id == id);
      if (stock != null && stock.Buyer != null)
      {
        stock.Holder = stock.Buyer;
        stock.Buyer = null;
        stock.SaleDate = DateTimeOffset.UtcNow;
        await context.SaveChangesAsync();
      }
      return RedirectToPage();
    }
    public async Task<IActionResult> OnPostValidateSelectedSalesAsync(IEnumerable<int> id)
    {
      var stocks = context.Stocks.Include(f => f.Buyer).Include(f => f.Holder).Where(f => id.Contains(f.Id));
      foreach (var stock in stocks)
      {
        if (stock != null && stock.Buyer != null)
        {
          stock.Holder = stock.Buyer;
          stock.Buyer = null;
          stock.SaleDate = DateTimeOffset.UtcNow;
        }
      }
      await context.SaveChangesAsync();
      return RedirectToPage();
    }
  }
}