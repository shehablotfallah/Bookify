using Bookify.Web.Core.Models;

namespace Bookify.Web.Core.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Authors
        CreateMap<Author, AuthorViewModel>();

        CreateMap<AuthorFormViewModel, Author>().ReverseMap();

        CreateMap<Author, SelectListItem>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));


        //Books
        CreateMap<Book, BookViewModel>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author!.Name))
            .ForMember(dest => dest.Categories,
                opt => opt.MapFrom(src => src.Categories.Select(c => c.Category!.Name)));


        CreateMap<BookFormViewModel, Book>()
            .ReverseMap()
            .ForMember(dest => dest.Categories, opt => opt.Ignore());


        //BookCopy
        CreateMap<BookCopy, BookCopyViewModel>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book!.Title))
            .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Book!.Id))
            .ForMember(dest => dest.BookThumbnailUrl, opt => opt.MapFrom(src => src.Book!.ImageThumbnailUrl));

        CreateMap<BookCopy, BookCopyFormViewModel>();


        //Categories
        CreateMap<Category, CategoryViewModel>();

        CreateMap<CategoryFormViewModel, Category>().ReverseMap();

        CreateMap<Category, SelectListItem>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));


        //Users
        CreateMap<ApplicationUser, UserViewModel>();

        CreateMap<UserFormViewModel, ApplicationUser>()
            .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
            .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()))
            .ReverseMap();

        //Governorates & Areas
        CreateMap<Governorate, SelectListItem>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

        CreateMap<Area, SelectListItem>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

        //Subscribers
        CreateMap<Subscriber, SubscriberFormViewModel>().ReverseMap();

        CreateMap<Subscriber, SubscriberSearchResultViewModel>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        CreateMap<Subscriber, SubscriberViewModel>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area!.Name))
            .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.Governorate!.Name));

        CreateMap<Subscription, SubscriptionViewModel>();

		//Rentals
		CreateMap<Rental, RentalViewModel>();
		CreateMap<RentalCopy, RentalCopyViewModel>();
	}
}
