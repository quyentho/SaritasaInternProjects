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
            CreateMap<Listing, ListingResponseViewModel>();

            CreateMap<ListingResponseViewModel, Listing>()
                .ForMember(dest => dest.Comments, opt => opt.Ignore());


            CreateMap<ListingStatus, ListingStatusViewModel>();

            CreateMap<ListingNote, ListingNoteResponseViewModel>();

            CreateMap<ListingPhoto, ListingPhotoResponseViewModel>();

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
