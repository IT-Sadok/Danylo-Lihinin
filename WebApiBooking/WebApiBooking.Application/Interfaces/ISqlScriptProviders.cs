namespace WebApiBooking.Application.Interfaces;

public interface ISqlScriptProvider
{
    public Task<string> GetScriptAsync(string scriptName);
}