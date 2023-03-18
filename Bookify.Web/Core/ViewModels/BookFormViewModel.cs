﻿using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Bookify.Web.Core.ViewModels
{
    public class BookFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(500, ErrorMessage = Errors.MaxLength)]
        [Remote("AllowItem", null!, AdditionalFields = "Id, AuthorId", ErrorMessage = Errors.DuplicatedBook)]
        public string Title { get; set; } = null!;

        [Display(Name = "Author")]
        [Remote("AllowItem", null!, AdditionalFields = "Id, Title", ErrorMessage = Errors.DuplicatedBook)]
        public int AuthorId { get; set; }
        public IEnumerable<SelectListItem>? Authors { get; set; }

        [MaxLength(200, ErrorMessage = Errors.MaxLength)]
        public string Publisher { get; set; } = null!;

        [Display(Name = "Publishing Date")]
        [AssertThat("PublisherDate <= Today()", ErrorMessage = Errors.NotAllowFutureDates)]
        public DateTime PublisherDate { get; set; } = DateTime.Now;
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }

        [MaxLength(50, ErrorMessage = Errors.MaxLength)]
        public string Hall { get; set; } = null!;

        [Display(Name = "Is available for rental?")]
        public bool IsAvailableForRental { get; set; }
        public string Description { get; set; } = null!;
        [Display(Name = "Categories")]
        public IList<int> SelectedCategories { get; set; } = new List<int>();
        public IEnumerable<SelectListItem>? Categories { get; set; }

    }
}
