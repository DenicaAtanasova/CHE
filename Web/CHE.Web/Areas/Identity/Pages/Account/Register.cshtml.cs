namespace CHE.Web.Areas.Identity.Pages.Account
{
    using CHE.Common;
    using CHE.Data.Models;
    using CHE.Services.Data;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<CheUser> _signInManager;
        private readonly UserManager<CheUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ITeachersService _teachersService;
        private readonly IParentsService _parentsService;

        public RegisterModel(
            UserManager<CheUser> userManager,
            SignInManager<CheUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ITeachersService teachersService,
            IParentsService parentsService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._logger = logger;
            this._emailSender = emailSender;
            this._teachersService = teachersService;
            this._parentsService = parentsService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new CheUser { UserName = Input.Username, Email = Input.Email};
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    var assignRole = await _userManager.AddToRoleAsync(user, Input.Role);
                    if (!assignRole.Succeeded)
                    {
                        return Page();
                    }

                    if (await _userManager.IsInRoleAsync(user, GlobalConstants.TeacherRole))
                    {
                        await this._teachersService.CreateAsync(user.Id);
                    }
                    else if(await _userManager.IsInRoleAsync(user, GlobalConstants.ParentRole))
                    {
                        await this._parentsService.CreateAsync(user.Id);
                    }

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        if (Input.Role == GlobalConstants.TeacherRole)
                        {
                            return LocalRedirect("~/Identity/Account/Manage/Profile");
                        }

                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
