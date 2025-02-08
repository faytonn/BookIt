using BookIt.Application.Interfaces.Helpers;
using Microsoft.Extensions.Localization;
using System.Text;

namespace BookIt.Infrastracture.Localizers;

public class ValidationMessagesLocalizer : IValidationMessagesProvider
{
    private readonly IStringLocalizer _stringLocalizer;

    public ValidationMessagesLocalizer(IStringLocalizerFactory stringLocalizer)
    {
        _stringLocalizer = stringLocalizer.Create("ValidationMessages", "BookIt.Presentation");
    }

    public string GetValue(string key)
    {
        return _stringLocalizer.GetString(key);
    }
}
