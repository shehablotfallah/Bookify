﻿namespace Bookify.Web.Core.ViewModels;

public class UserFormViewModel
{
    public string? Id { get; set; }

    [MaxLength(100, ErrorMessage = Errors.MaxLength), 
        RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters),
        Display(Name = "Full Name")]
    public string FullName { get; set; } = null!;

    [MaxLength(20, ErrorMessage = Errors.MaxLength),
        Remote("AllowUserName", null!, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated),
        RegularExpression(RegexPatterns.UserName, ErrorMessage = Errors.InvalidUsername),
        Display( Name = "Username")]
    public string UserName { get; set; } = null!;

    [MaxLength(200, ErrorMessage = Errors.MaxLength),
        Remote("AllowEmail", null!, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated),
        RegularExpression(RegexPatterns.Email) ,EmailAddress]
    public string Email { get; set; } = null!;

    [StringLength(100, ErrorMessage = Errors.MaxMinLength, MinimumLength = 8),
        RegularExpression(RegexPatterns.Password, ErrorMessage = Errors.WeakPassword),
        DataType(DataType.Password)]
    [RequiredIf("Id == null", ErrorMessage = Errors.RequiredField)]
    public string? Password { get; set; } = null!;

    [DataType(DataType.Password),
        Compare("Password", ErrorMessage = Errors.ConfirmPasswordNotMatch),
        Display(Name = "Confirm password")]
    [RequiredIf("Id == null", ErrorMessage = Errors.RequiredField)]
    public string? ConfirmPassword { get; set; } = null!;

    [Display(Name = "Roles")]
    public IList<string> SelectedRoles { get; set; } = new List<string>();

    public IEnumerable<SelectListItem>? Roles { get; set; }
}
