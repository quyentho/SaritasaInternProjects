using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;

namespace UnrealEstate.Models.MappingConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Listing, ListingViewModel>();

            CreateMap<ListingViewModel, Listing>()
                .ForMember(dest=>dest.Comments,opt=>opt.Ignore());
                

            CreateMap<ListingStatus, ListingStatusViewModel>();

            CreateMap<ListingNote, ListingNoteViewModel>();

            CreateMap<ListingPhoto, ListingPhotoViewModel>();

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
