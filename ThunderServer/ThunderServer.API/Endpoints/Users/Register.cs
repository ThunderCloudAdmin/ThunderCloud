using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using ThunderServer.API.Services.Interfaces;
using ThunderServer.Models;
using static ThunderServer.API.Endpoints.Users.Register;

namespace ThunderServer.API.Endpoints.Users;

public class Register : Endpoint<RegisterRequest, Results<Ok, ValidationProblem>>
{
    private readonly IThunderFileService _thunderFileService;
    private readonly ILogger<Register> _logger;
    private readonly UserManager<ThunderUser> userManager;
    private readonly IUserStore<ThunderUser> userStore;
    private static readonly EmailAddressAttribute _emailAddressAttribute = new();
    private readonly IEmailSender<ThunderUser> emailSender;
    private readonly IHttpContextAccessor httpContext;
    private readonly LinkGenerator linkGenerator;

    public Register(IThunderFileService thunderFileService, ILogger<Register> logger, UserManager<ThunderUser> userManager, IUserStore<ThunderUser> userStore, IEmailSender<ThunderUser> emailSender, IHttpContextAccessor httpContext, LinkGenerator linkGenerator)
    {
        _thunderFileService = thunderFileService ?? throw new ArgumentNullException(nameof(thunderFileService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
        this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        this.httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
        this.linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
    }

    public override void Configure()
    {
        Post("/thunderuser/register");
        AllowAnonymous();        
        Tags($"{nameof(ThunderUser)}");
    }

    public override async Task<Results<Ok, ValidationProblem>> ExecuteAsync(RegisterRequest registration, CancellationToken cancellationToken = default)
    {
        var emailStore = (IUserEmailStore<ThunderUser>)userStore;
        var email = registration.Email;

        if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
        {
            return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
        }

        var user = new ThunderUser()
        {
            FirstName = registration.FirstName,
            LastName = registration.LastName
        };

        await userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, registration.Password);
        if (!result.Succeeded)
        {
            return CreateValidationProblem(result);
        }

        //await SendConfirmationEmailAsync(user, email);

        return TypedResults.Ok();
    }

    async Task SendConfirmationEmailAsync(ThunderUser user, string email, bool isChange = false)
    {
        var code = isChange
            ? await userManager.GenerateChangeEmailTokenAsync(user, email)
            : await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var userId = await userManager.GetUserIdAsync(user);
        var routeValues = new RouteValueDictionary()
        {
            ["userId"] = userId,
            ["code"] = code,
        };

        if (isChange)
        {
            // This is validated by the /confirmEmail endpoint on change.
            routeValues.Add("changedEmail", email);
        }

        var confirmEmailUrl = linkGenerator.GetUriByName(this.httpContext.HttpContext, "confirmEmailEndpointName", routeValues);// ?? throw new NotSupportedException($"Could not find endpoint named '{confirmEmailEndpointName}'.");

        await emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(confirmEmailUrl));
    }

    private static ValidationProblem CreateValidationProblem(string errorCode, string errorDescription) =>
        TypedResults.ValidationProblem(new Dictionary<string, string[]> {
            { errorCode, [errorDescription] }
        });

    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert(!result.Succeeded);
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }

    public sealed record RegisterRequest
    {
        public required string Email { get; init; }

        public required string Password { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }
    }
}