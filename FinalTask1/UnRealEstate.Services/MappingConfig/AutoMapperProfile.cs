using System.Linq;
using AutoMapper;
using UnrealEstate.Infrastructure.Models;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;
using UnrealEstate.Services.Listing.ViewModel.Request;

namespace UnrealEstate.Services.MappingConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region listing mapping

            CreateMap<Infrastructure.Models.Listing, ListingResponse>().ForMember(dest => dest.StatusName,
                opt => opt.MapFrom(src => src.Status.Name));

            CreateMap<ListingRequest, Infrastructure.Models.Listing>()
                .ForMember(dest => dest.ListingPhoTos, opt => opt.Ignore());

            CreateMap<ListingNote, ListingNoteResponse>();
            CreateMap<ListingNoteRequest, ListingNote>();

            CreateMap<ListingPhoto, ListingPhotoResponse>();
            CreateMap<ListingResponse, ListingRequest>()
                .ForMember(dest => dest.ListingPhoTos, opt => opt.Ignore());
            CreateMap<ListingNoteResponse, ListingNoteRequest>();

            #endregion

            #region comment mapping

            CreateMap<Infrastructure.Models.Comment, CommentResponse>();

            CreateMap<CommentRequest, Infrastructure.Models.Comment>();

            #endregion

            #region user mapping

            CreateMap<ApplicationUser, UserResponse>()
                .ForMember(dest => dest.ListingsCreated, opt => opt.MapFrom(src => src.Listings))
                .ForMember(dest => dest.FavoriteListings,
                    opt => opt.MapFrom(src => src.Favorites.Select(f => f.Listing)));

            CreateMap<UserRequest, ApplicationUser>();

            #endregion

            #region bid mapping

            CreateMap<ListingBidRequest, Bid>();
            CreateMap<Bid, ListingBidResponse>();

            #endregion
        }
    }
}