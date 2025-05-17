using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Linq;

// Your in-memory user store
public static class TestUsers
{
    public static List<TestUser> Users = new List<TestUser>
    {
        new TestUser { SubjectId="1", Username="alice", Password="password" },
        new TestUser { SubjectId="2", Username="bob", Password="password" }
    };
}

public class TestUser
{
    public string SubjectId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginModel : PageModel
{
    [BindProperty]
    public LoginInput Input { get; set; }

    public string ErrorMessage { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        // Validate user against in-memory list
        var user = TestUsers.Users
            .FirstOrDefault(u => u.Username == Input.Username && u.Password == Input.Password);

        if (user != null)
        {
            var claims = new[]
            {
                new Claim("sub", user.SubjectId),
                new Claim("name", user.Username)
            };
            var identity = new ClaimsIdentity(claims, "cookie");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);

            // Redirect to returnUrl or homepage
            var returnUrl = HttpContext.Request.Query["returnUrl"].ToString();
            if (!string.IsNullOrEmpty(returnUrl))
                return LocalRedirect(returnUrl);
            else
                return Redirect("~/");
        }
        else
        {
            ErrorMessage = "Invalid username or password.";
            return Page();
        }
    }
}

public class LoginInput
{
    public string Username { get; set; }
    public string Password { get; set; }
}