using System.Collections.Generic;
using UnrealEstate.Models;
using UnRealEstate.Services;
using Xunit;
using FluentAssertions;
using Moq;
using UnrealEstate.Models.Repositories;
using UnrealEstate.Models.Models;
using Microsoft.EntityFrameworkCore;
using Autofac.Extras.Moq;
using Autofac;
using System;

namespace UnrealEstate.Services.Tests
{
    public class CommonUserServiceTest : IDisposable
    {

        UnrealEstateDbContext _testContext;
        IListingRepository _fakeListingRepository;
        Mock<IUserManager> _mockUserManager = new Mock<IUserManager>();
        List<Listing> _fakeListings;
        CommonUserSerive _sut;
        public CommonUserServiceTest()
        {
            // fake data to test
            _fakeListings = new List<Listing>();
            _fakeListings.Add(new Listing() { Id = 1, StatusId = 1, UserId = 1, Zip = "aaaaa", AddressLine1 = "address1", City = "city1" });
            _fakeListings.Add(new Listing() { Id = 2, StatusId = 1, UserId = 2, Zip = "aaaaa", AddressLine1 = "address2", City = "city2" });
            _fakeListings.Add(new Listing() { Id = 3, StatusId = 1, UserId = 3, Zip = "bbbbb", AddressLine1 = "address3", City = "city3" });
            _fakeListings.Add(new Listing() { Id = 4, StatusId = 1, UserId = 3, Zip = "bbbbb", AddressLine1 = "address4", City = "city4" });
            _fakeListings.Add(new Listing() { Id = 5, StatusId = 2, UserId = 3, Zip = "bbbbb", AddressLine1 = "address4", City = "city4" });

            // create in memory database to test.
            var options = new DbContextOptionsBuilder<UnrealEstateDbContext>()
                         .UseInMemoryDatabase(databaseName: "TestDb")
                         .Options;

            _testContext = new UnrealEstateDbContext(options);
            _testContext.Listings.AddRange(_fakeListings);
            _testContext.SaveChanges();

            _fakeListingRepository = new ListingRepository(_testContext);


            // add data to in memory db.


            // Create system under test.
            _sut = new CommonUserSerive(_fakeListingRepository, _mockUserManager.Object);
        }


        [Fact]
        public void GetListings_ValidRequest_ReturnsListings()
        {
            var expectedListings = _fakeListings;

            List<Listing> resultListings = _sut.GetListings();

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
            Listing listingResult = _sut.GetListing(listingId);

            listingResult.Should().Be(_fakeListings[listingId - 1]);
        }

        [Theory()]
        [InlineData(-1)]
        [InlineData(6)]
        [InlineData(10)]
        public void GetListing_NoneExistingListing_ReturnsNull(int listingId)
        {
            Listing listingResult = _sut.GetListing(listingId);

            listingResult.Should().Be((Listing)null);
        }

        [Theory]
        [InlineData("aaaaa", 0, 2)]
        [InlineData("bbbbb", 2, 2)]
        [InlineData("address2", 1, 1)]
        [InlineData("city4", 3, 1)]
        public void GetActiveListingWithFilter_FilterByAddress_ReturnsExactListings(string address, int startIndex, int count)
        {

            using (var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(_fakeListingRepository).As<IListingRepository>()))
            {
                _sut = mock.Create<CommonUserSerive>();
                var expectedListings = _fakeListings.GetRange(startIndex, count);

                var listingsResult = _sut.GetActiveListingWithFilter(new FilterCriteria() { Address = address });

                listingsResult.Should().BeEquivalentTo(expectedListings);
            }
        }

        public void Dispose()
        {
            _fakeListings.Clear();
            _testContext.Database.EnsureDeleted();
            _testContext.Dispose();
        }
    }
}
