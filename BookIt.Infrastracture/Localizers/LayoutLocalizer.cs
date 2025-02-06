using BookIt.Application.Interfaces.Localizers;
using Microsoft.Extensions.Localization;

namespace BookIt.Infrastracture.Localizers;

public class LayoutLocalizer : ILayoutLocalizer
{
    private readonly IStringLocalizer _localizer;
    public LayoutLocalizer(IStringLocalizerFactory factory)
    {
        _localizer = factory.Create("Layout", "BookIt.Presentation"); 
    }
    public string GetValue(string key)
    {
        return _localizer.GetString(key);
    }
}
