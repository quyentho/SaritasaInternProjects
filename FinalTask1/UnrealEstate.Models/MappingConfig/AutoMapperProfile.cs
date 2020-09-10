using AutoMapper;
using System.Linq;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;

namespace UnrealEstate.Models.MappingConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region listing mapping
            CreateMap<Listing, ListingResponseViewModel>();

            CreateMap<ListingRequestViewModel, Listing>();

            CreateMap<ListingStatus, ListingStatusResponseViewModel>();

            CreateMap<ListingNote, ListingNoteResponseViewModel>();
            CreateMap<ListingNoteRequestViewModel, ListingNote>();

            CreateMap<ListingPhoto, ListingPhotoResponseViewModel>();
            CreateMap<ListingPhotoRequestViewModel, ListingPhoto>();
            #endregion

            #region comment mapping
            CreateMap<Comment, CommentResponseViewModel>();

            CreateMap<CommentRequestViewModel, Comment>();
            #endregion


            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.ListingsCreated, opt => opt.MapFrom(src => src.Listings))
                .ForMember(dest => dest.FavoriteListings, opt => opt.MapFrom(src => src.Favorites.Select(f => f.Listing)));

            CreateMap<UserViewModel, User>()
                .ForMember(dest => dest.Favorites, opt => opt.Ignore())
                .ForMember(dest => dest.ListingNotes, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Listings, opt => opt.Ignore());
        }
    }
}
