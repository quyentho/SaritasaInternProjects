using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnrealEstate.Models.ViewModels;

namespace UnrealEstate.Models.MappingConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Listing, ListingViewModel>()
                .ReverseMap();

            CreateMap<ListingStatus, ListingStatusViewModel>();

            CreateMap<ListingNote, ListingNoteViewModel>();

            CreateMap<ListingPhoto, ListingPhotoViewModel>();
            
            CreateMap<Comment, CommentViewModel>().ReverseMap();

            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.ListingsCreated, opt => opt.MapFrom(src => src.Listings))
                .ForMember(dest => dest.FavoriteListings, opt => opt.MapFrom(src => src.Favorites.Select(f => f.Listing)));
        }
    }
}
