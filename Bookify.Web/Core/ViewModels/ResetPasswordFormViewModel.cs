﻿namespace Bookify.Web.Core.ViewModels;

public class ResetPasswordFormViewModel
{
    public string Id { get; set; } = null!;

    [StringLength(100, ErrorMessage = Errors.MaxMinLength, MinimumLength = 8),
        RegularExpression(RegexPatterns.Password, ErrorMessage = Errors.WeakPassword),
        DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password),
        Compare("Password", ErrorMessage = Errors.ConfirmPasswordNotMatch),
        Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = null!;
}
