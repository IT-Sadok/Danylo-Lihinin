using System.Reflection;
using WebApiBooking.Application.Interfaces;

namespace WebApiBooking.Infrastructure;

public class EmbeddedSqlScriptProvider : ISqlScriptProvider
{
    private readonly Assembly _assembly = typeof(EmbeddedSqlScriptProvider).Assembly;
    
    public async Task<string> GetScriptAsync(string scriptName)
    {
        var path = _assembly.GetName().Name + ".Scripts." + scriptName + ".sql";

        var stream = _assembly.GetManifestResourceStream(path);
        if (stream is null)
            throw new KeyNotFoundException("Scripts with this name is not found");
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}