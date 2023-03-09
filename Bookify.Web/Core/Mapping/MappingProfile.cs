namespace Bookify.Web.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Category,CategoryViewModel> ();
            CreateMap<CategoryFormViewModel,Category> ().ReverseMap();
        }
    }
}
