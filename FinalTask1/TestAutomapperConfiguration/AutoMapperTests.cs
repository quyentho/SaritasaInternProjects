using AutoMapper;
using FluentAssertions;
using System;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;
using Xunit;

namespace TestAutomapperConfiguration
{
    public class AutoMapperTests
    {
        [Fact]
        public void ListingFlatteningMappingTest()
        {

            var user = new User() { Email = "test@test.com" };
            var listing = new Listing()
            {
                Status = new ListingStatus() { Name = "Active" },
                User = user
            };

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<Listing, ListingViewModel>());

            var mapper = configuration.CreateMapper();

            ListingViewModel result = mapper.Map<ListingViewModel>(listing);

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

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Listing, ListingViewModel>().ForMember(dest => dest.UserId, opt => opt.Ignore()));

            var mapper = config.CreateMapper();

            ListingViewModel result = mapper.Map<ListingViewModel>(listing);

            result.UserId.Should().BeNull();
        }
    }
}
