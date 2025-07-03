using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

public class PageModelBase : PageModel
{
    public string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
}

