using System.ComponentModel.DataAnnotations;

namespace BookIt.Domain.Enums;

public enum LanguageType
{
    [Display(Name = "English")]
    English,

    [Display(Name = "Azerbaijani")]
    Azerbaijani,

    [Display(Name = "Czech")]
    Czech
}
