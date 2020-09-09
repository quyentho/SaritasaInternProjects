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
                
            
            CreateMap<Comment, CommentViewModel>();

            CreateMap<User, UserViewModel>();
        }
    }
}
