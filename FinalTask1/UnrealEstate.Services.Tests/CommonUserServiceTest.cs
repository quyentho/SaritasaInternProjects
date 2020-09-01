using System.Collections.Generic;
using UnrealEstate.Models;
using UnRealEstate.Services;
using Xunit;
using FluentAssertions;
using Moq;
using UnrealEstate.Models.Repositories;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using LinqKit;
using System.Linq.Expressions;
using UnrealEstate.Models.Models;

namespace UnrealEstate.Services.Tests
{
    public class CommonUserServiceTest
    {

        Mock<IListingRepository> _mockListing = new Mock<IListingRepository>();
        Mock<IUserManager> _mockUserManager = new Mock<IUserManager>();
        List<Listing> _fakeListings = new List<Listing>();
        CommonUserSerive _sut;
        public CommonUserServiceTest()
        {
            _fakeListings.Add(new Listing() { Id = 1, StatusId = 1, UserId = 1, Zip = "aaaaa", AddressLine1= "address1", City ="city1" });
            _fakeListings.Add(new Listing() { Id = 2, StatusId = 1, UserId = 2, Zip = "aaaaa", AddressLine1 = "address2", City = "city2" });
            _fakeListings.Add(new Listing() { Id = 3, StatusId = 1, UserId = 3, Zip = "bbbbb", AddressLine1 = "address3", City = "city3" });
            _fakeListings.Add(new Listing() { Id = 4, StatusId = 1, UserId = 3, Zip = "bbbbb", AddressLine1 = "address4", City = "city4" });
            _fakeListings.Add(new Listing() { Id = 5, StatusId = 2, UserId = 3, Zip = "bbbbb", AddressLine1 = "address4", City = "city4" });

            _sut = new CommonUserSerive(_mockListing.Object, _mockUserManager.Object);
        }


        [Fact]
        public void GetListings_ValidRequest_ReturnsListings()
        {
            var expectedListings = _fakeListings;

            _mockListing.Setup(repo => repo.GetListings()).Returns(expectedListings);


            CommonUserSerive commonUserSerive = new CommonUserSerive(_mockListing.Object, _mockUserManager.Object);

            List<Listing> resultListings = commonUserSerive.GetListings();

            resultListings.Should().BeEquivalentTo(expectedListings);
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void Login_GivenEmailAndPassword_ReturnsDependOnVerifyResult(bool isValidUser)
        {
            string expectedEmail = "test@test.com";
            string expectedPassword = "myPassword";

            _mockUserManager
                .Setup(u => u.VerifyLogin(It.Is<string>(s => s == expectedEmail), It.Is<string>(s => s == expectedPassword)))
                .Returns(isValidUser);

            bool result = _sut.Login(expectedEmail, expectedPassword);

            result.Should().Be(isValidUser);
        }

        [Theory()]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetListing_ExistingListing_ReturnsExactListing(int listingId)
        {
            _mockListing.Setup(l => l.GetListing(listingId)).Returns(_fakeListings[listingId - 1]);

            Listing listingResult = _sut.GetListing(listingId);

            listingResult.Should().Be(_fakeListings[listingId - 1]);
        }

        [Theory()]
        [InlineData(-1)]
        [InlineData(6)]
        [InlineData(10)]
        public void GetListing_NoneExistingListing_ReturnsNull(int listingId)
        {
            _mockListing.Setup(l => l.GetListing(listingId)).Returns((Listing)null);

            Listing listingResult = _sut.GetListing(listingId);

            listingResult.Should().Be((Listing)null);
        }
    }
}
