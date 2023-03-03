﻿using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Core.ViewModels
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "Max length cannot be more than 100 letters!")]
        [Remote("AllowItem", null, ErrorMessage = "Category with the same name is already exists!")]
        public string Name { get; set; } = null!;

    }
}
