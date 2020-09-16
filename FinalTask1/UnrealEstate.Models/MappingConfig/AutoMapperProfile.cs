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

            CreateMap<Listing, ListingResponse>();

            CreateMap<ListingRequest, Listing>().ForMember(dest => dest.ListingPhoTos, opt => opt.Ignore());

            CreateMap<ListingNote, ListingNoteResponse>();
            CreateMap<ListingNoteRequest, ListingNote>();

            CreateMap<ListingPhoto, ListingPhotoResponse>();
            CreateMap<ListingResponse, ListingRequest>()
                .ForMember(dest => dest.ListingPhoTos, opt => opt.Ignore());
            CreateMap<ListingNoteResponse, ListingNoteRequest>();
            #endregion

            #region comment mapping
            CreateMap<Comment, CommentResponse>();

            CreateMap<CommentRequest, Comment>();
            #endregion

            #region user mapping
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.ListingsCreated, opt => opt.MapFrom(src => src.Listings))
                .ForMember(dest => dest.FavoriteListings, opt => opt.MapFrom(src => src.Favorites.Select(f => f.Listing)));

            CreateMap<UserRequest, User>();
            #endregion

            #region bid mapping
            CreateMap<BidRequest, Bid>();
            CreateMap<Bid, BidResponse>();
            #endregion
        }
    }
}
