using Autofac;
using Autofac.Extras.Moq;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using UnrealEstate.Models;
using UnrealEstate.Models.Models;
using UnrealEstate.Models.Repositories;
using Xunit;

namespace UnrealEstate.Services.Tests
{
    [Collection("Database collection")]
    public class ListingServiceTests
    {
        ListingService _sut;
        private readonly DatabaseFixture _databaseFixture;

        public ListingServiceTests(DatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture;
        }


        [Theory()]
        [InlineData(-1)]
        [InlineData(-5)]
        [InlineData(6)]
        public void DisableListing_NoneExistingListing_ThrowArgumentOutOfRangeException(int id)
        {
            using (var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(_databaseFixture.FakeListingRepository).As<IListingRepository>()))
            {
                _sut = mock.Create<ListingService>();

                Action result = () => _sut.DisableListing(id);

                result.Should().Throw<ArgumentOutOfRangeException>();
            }
        }

        [Fact]
        public void GetListings_ValidRequest_ReturnsListings()
        {
            using (var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(_databaseFixture.FakeListingRepository).As<IListingRepository>()))
            {
                _sut = mock.Create<ListingService>();
                var expectedListings = _databaseFixture.FakeListings;

                List<Listing> resultListings = _sut.GetListings();

                resultListings.Should().BeEquivalentTo(expectedListings);
            }
        }

        [Theory()]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]

        public void DisableListing_ExistingListing_ShouldNotThrowException(int id)
        {
            using (var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(_databaseFixture.FakeListingRepository).As<IListingRepository>()))
            {
                _sut = mock.Create<ListingService>();

                Action result = () => _sut.DisableListing(id);

                result.Should().NotThrow<ArgumentOutOfRangeException>();
            }
        }

        [Theory()]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetListing_WhenCall_ReturnsExactListing(int listingId)
        {
            using (var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(_databaseFixture.FakeListingRepository).As<IListingRepository>()))
            {
                _sut = mock.Create<ListingService>();
               
                Listing listingResult = _sut.GetListing(listingId);

                listingResult.Should().Be(_databaseFixture.FakeListings[listingId - 1]);
            }
        }

        [Theory()]
        [InlineData(-1)]
        [InlineData(6)]
        [InlineData(10)]
        public void GetListing_NoneExistingListing_ReturnsNull(int listingId)
        {
            using (var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(_databaseFixture.FakeListingRepository).As<IListingRepository>()))
            {
                _sut = mock.Create<ListingService>();

                Listing listingResult = _sut.GetListing(listingId);

                listingResult.Should().Be((Listing)null);
            }
        }


        // This test will be failed if run all tests at once
        [Theory]
        [InlineData("aaaaa", 0, 2)]
        [InlineData("bbbbb", 2, 2)]
        [InlineData("address2", 1, 1)]
        [InlineData("city4", 3, 1)]
        public void GetActiveListingWithFilter_FilterByAddress_ReturnsExactListings(string address, int startIndex, int count)
        {

            using (var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(_databaseFixture.FakeListingRepository).As<IListingRepository>()))
            {
                _sut = mock.Create<ListingService>();
                var expectedListings = _databaseFixture.FakeListings.GetRange(startIndex, count);

                var listingsResult = _sut.GetActiveListingWithFilter(new FilterCriteria() { Address = address });

                listingsResult.Should().BeEquivalentTo(expectedListings);
            }
        }

        [Fact]
        public void CreateListing_ValidNewListing_CallToAddFunctionOfListingRepositoryOneTimes()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var sut = mock.Create<ListingService>();

                sut.CreateListing(new Listing());

                mock.Mock<IListingRepository>().Verify(l => l.AddListing(It.IsAny<Listing>()), Times.Once);
            }

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void EditListing_WhenCall_CallsToUpdateListingOfListingRepositoryOneTime(int id)
        {
            using (var mock = AutoMock.GetLoose())
            {
                var sut = mock.Create<ListingService>();

                sut.EditListing(new Listing() { Id = id });

                mock.Mock<IListingRepository>().Verify(l => l.UpdateListing(It.Is<Listing>(l => l.Id == id)), Times.Once);
            }
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-3)]
        public void EditListing_NotExistId_ThrowArgumentOutOfRangeException(int id)
        {
            using (var mock = AutoMock.GetLoose(cfg => cfg
                .RegisterInstance(_databaseFixture.FakeListingRepository)
                .As<IListingRepository>()))
            {
                var sut = mock.Create<ListingService>();

                Action result = () => sut.EditListing(new Listing() { Id = id });

                result.Should().Throw<ArgumentOutOfRangeException>();
            }
        }
    }

}

