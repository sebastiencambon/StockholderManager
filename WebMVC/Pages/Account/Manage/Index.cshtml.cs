using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockholderManager.Data;
using StockholderManager.Services;

namespace StockholderManager.Pages.Account.Manage
{
  public partial class IndexModel : PageModel
  {
    private readonly UserManager<Stockholder> _userManager;
    private readonly SignInManager<Stockholder> _signInManager;
    private readonly IEmailSender _emailSender;

    public IndexModel(
        UserManager<Stockholder> userManager,
        SignInManager<Stockholder> signInManager,
        IEmailSender emailSender)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _emailSender = emailSender;
    }

    [Display(Name = "Nom d'utilisateur")]
    public string Username { get; set; }

    public bool IsEmailConfirmed { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

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

    public async Task<IActionResult> OnGetAsync()
    {
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      Username = user.UserName;
      Input = new InputModel
      {
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        Address = user.Address,
        FirstName = user.FirstName,
        LastName = user.LastName
      };

      IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      if (Input.Email != user.Email)
      {
        var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
        if (!setEmailResult.Succeeded)
        {
          throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
        }
      }

      if (Input.PhoneNumber != user.PhoneNumber)
      {
        var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
        if (!setPhoneResult.Succeeded)
        {
          throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
        }
      }

      user.LastName = Input.LastName;
      user.FirstName = Input.FirstName;


      await _userManager.UpdateAsync(user);

      StatusMessage = "Vos information ont été mise à jour";
      return RedirectToPage();
    }
    public async Task<IActionResult> OnPostSendVerificationEmailAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
      await _emailSender.SendEmailConfirmationAsync(user.Email, callbackUrl);

      StatusMessage = "Email de vérification envoyé.";
      return RedirectToPage();
    }
  }
}
