using Microsoft.AspNetCore.Identity;
using ThunderServer.Models;

internal class EmailSender : IEmailSender<ThunderUser>
{
	public Task SendConfirmationLinkAsync(ThunderUser user, string email, string confirmationLink)
	{
		throw new NotImplementedException();
	}

	public Task SendPasswordResetCodeAsync(ThunderUser user, string email, string resetCode)
	{
		throw new NotImplementedException();
	}

	public Task SendPasswordResetLinkAsync(ThunderUser user, string email, string resetLink)
	{
		throw new NotImplementedException();
	}
}