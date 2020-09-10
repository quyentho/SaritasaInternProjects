using AutoMapper;
using System.Linq;
using UnrealEstate.Models.Models;
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

            #region user mapping
            CreateMap<User, UserResponseViewModel>()
                .ForMember(dest => dest.ListingsCreated, opt => opt.MapFrom(src => src.Listings))
                .ForMember(dest => dest.FavoriteListings, opt => opt.MapFrom(src => src.Favorites.Select(f => f.Listing)));

            CreateMap<UserRequestViewModel, User>();
            #endregion

            #region bid mapping
            CreateMap<BidRequestViewModel, Bid>();
            CreateMap<Bid, BidResponseViewModel>();
            #endregion
        }
    }
}
