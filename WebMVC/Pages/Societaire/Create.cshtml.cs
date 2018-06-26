using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockholderManager.Data;

namespace StockholderManager.Pages.Societaire
{
  public class CreateModel : PageModel
  {
    public class InputModel
    {
      [Required(ErrorMessage = "Champ requis")]
      [EmailAddress(ErrorMessage = "Email invalide")]
      public string Email { get; set; }

      [Required(ErrorMessage = "Champ requis")]
      [Display(Name = "Prénom")]
      public string FirstName { get; set; }

      [Display(Name = "Nom")]
      [Required(ErrorMessage = "Champ requis")]
      public string LastName { get; set; }

      [Display(Name = "Adresse")]
      public string Address { get; set; }

      [Display(Name = "Code postal")]
      public string Zipcode { get; set; }

      [Display(Name = "Ville")]
      public string City { get; set; }

      [Phone(ErrorMessage = "Numéro de téléphone invalide")]
      [Required(ErrorMessage = "Champ requis")]
      [Display(Name = "Numéro de téléphone")]
      public string PhoneNumber { get; set; }
    }

    private readonly UserManager<Stockholder> users;

    public CreateModel(UserManager<Stockholder> users)
    {
      this.users = users;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
      var user = new Stockholder() { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName, Address = Input.Address, ZipCode = Input.Zipcode, City = Input.City, PhoneNumber = Input.PhoneNumber };
      var result = await users.CreateAsync(user);
      if (result.Succeeded)
        await users.AddToRoleAsync(user, "societaire");
      return RedirectToPage("Index");
    }
  }
}