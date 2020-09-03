using Autofac;
using Autofac.Extras.Moq;
using FluentAssertions;
using Moq;
using System;
using UnrealEstate.Models;
using UnrealEstate.Models.Repositories;
using Xunit;

namespace UnrealEstate.Services.Tests
{
    [Collection("Database collection")]
    public class RegularUserServiceTests
    {
        private readonly DatabaseFixture _databaseFixture;

        public RegularUserServiceTests(DatabaseFixture databaseFixture)
        {
            this._databaseFixture = databaseFixture;
        }

        [Fact]
        public void CreateListing_ValidNewListing_CallToAddFunctionOfListingRepositoryOneTimes()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var sut = mock.Create<RegularUserService>();

                sut.CreateListing(new Listing());

                mock.Mock<IListingRepository>().Verify(l => l.AddListing(It.IsAny<Listing>()), Times.Once);
            }

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void EditListing_ExistingListing_ShouldNotThrowException(int id)
        {
            using (var mock = AutoMock.GetLoose(cfg => cfg
                .RegisterInstance(_databaseFixture.FakeListingRepository)
                .As<IListingRepository>()))
            {
                var sut = mock.Create<RegularUserService>();

                Action result = () => sut.EditListing(new Listing() { Id = id });

                result.Should().NotThrow();
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
                var sut = mock.Create<RegularUserService>();

                Action result = () => sut.EditListing(new Listing() { Id = id });

                result.Should().Throw<ArgumentOutOfRangeException>();
            }
        }
    }
}
