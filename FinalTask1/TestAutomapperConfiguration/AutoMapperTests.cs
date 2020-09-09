using AutoMapper;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;
using Xunit;

namespace TestAutomapperConfiguration
{
    public class AutoMapperTests
    {
        [Fact]
        public void Listing_FlatteningMappingTest()
        {

            var user = new User() { Email = "test@test.com" };
            var listing = new Listing()
            {
                Status = new ListingStatus() { Name = "Active" },
                User = user
            };

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<Listing, ListingResponseViewModel>());

            var mapper = configuration.CreateMapper();

            ListingResponseViewModel result = mapper.Map<ListingResponseViewModel>(listing);

            result.UserEmail.Should().Be("test@test.com");
            result.StatusName.Should().Be("Active");
        }

        [Fact]
        public void Listing_IgnoreUserIdTest()
        {
            var listing = new Listing()
            {
                UserId = "1"
            };

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Listing, ListingResponseViewModel>().ForMember(dest => dest.UserId, opt => opt.Ignore()));

            var mapper = config.CreateMapper();

            ListingResponseViewModel result = mapper.Map<ListingResponseViewModel>(listing);

            result.UserId.Should().BeNull();
        }

        [Fact]

        public void UserViewModelToUser_NotMap_Favorites_ListingNotes_Comments_Listing()
        {
            var userViewModel = new UserViewModel()
            {
                FavoriteListings = new List<Listing>() { new Listing() { Id = 1 } },

                ListingNotes = new List<ListingNote>() { new ListingNote() { Id = 1 } },

                Comments = new List<Comment>() { new Comment() { Id = 1 } }
            };

            var user = new User()
            {
                Favorites = new List<Favorite>() { new Favorite() { ListingId = 10 } },

                ListingNotes = new List<ListingNote>() { new ListingNote() { Id = 10 } },

                Comments = new List<Comment>() { new Comment() { Id = 10 } },

                Listings = new List<Listing>() { new Listing() { Id = 10 } }
            };

            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserViewModel, User>()
                .ForMember(dest => dest.Favorites, opt => opt.Ignore())
                .ForMember(dest => dest.ListingNotes, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Listings, opt => opt.Ignore()));

            var mapper = config.CreateMapper();

            mapper.Map(userViewModel, user);

            userViewModel.FavoriteListings.First().Id.Should().Be(1);
            userViewModel.ListingNotes.First().Id.Should().Be(1);
            userViewModel.Comments.First().Id.Should().Be(1);
            userViewModel.ListingsCreated.Should().BeNull();
        }
    }
}
