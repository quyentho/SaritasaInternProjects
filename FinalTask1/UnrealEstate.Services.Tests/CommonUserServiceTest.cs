using Autofac;
using Autofac.Extras.Moq;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using UnrealEstate.Models;
using UnrealEstate.Models.Models;
using UnrealEstate.Models.Repositories;
using UnRealEstate.Services;
using Xunit;

namespace UnrealEstate.Services.Tests
{
    [Collection("Database collection")]
    public class CommonUserServiceTest
    {
        Mock<IUserManager> _mockUserManager = new Mock<IUserManager>();
        CommonUserSerive _sut;
        private readonly DatabaseFixture _databaseFixture;

        public CommonUserServiceTest(DatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture;
            _sut = new CommonUserSerive(_databaseFixture.FakeListingRepository, _mockUserManager.Object);
        }

        [Fact]
        public void GetListings_ValidRequest_ReturnsListings()
        {
            var expectedListings = _databaseFixture.FakeListings;

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
        public void GetListing_WhenCall_ReturnsExactListing(int listingId)
        {
            Listing listingResult = _sut.GetListing(listingId);

            listingResult.Should().Be(_databaseFixture.FakeListings[listingId - 1]);
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

            using (var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(_databaseFixture.FakeListingRepository).As<IListingRepository>()))
            {
                _sut = mock.Create<CommonUserSerive>();
                var expectedListings = _databaseFixture.FakeListings.GetRange(startIndex, count);

                var listingsResult = _sut.GetActiveListingWithFilter(new FilterCriteria() { Address = address });

                listingsResult.Should().BeEquivalentTo(expectedListings);
            }
        }
    }
}
