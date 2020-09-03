using Moq;
using UnrealEstate.Models;
using UnrealEstate.Models.Repositories;
using Xunit;

namespace UnrealEstate.Services.Tests
{
    public class RegularUserServiceTests
    {

        Mock<IListingRepository> _mockListingRepository = new Mock<IListingRepository>();
        Mock<IUserManager> _mockUserManager = new Mock<IUserManager>();

        RegularUserService _sut;

        public RegularUserServiceTests()
        {
            _sut = new RegularUserService(_mockListingRepository.Object, _mockUserManager.Object);
        }

        [Fact]
        public void CreateListing_ValidNewListing_CallToAddFunctionOfListingRepositoryOneTimes()
        {
            _sut.CreateListing(new Listing());

            _mockListingRepository.Verify(l => l.AddListing(It.IsAny<Listing>()), Times.Once);
        }

        [Fact]
        public void EditListing_WhenCall_CallToUpdateFunctionOfListingRepositoryOneTimes()
        {
            _sut.EditListing(It.IsAny<int>());

            _mockListingRepository.Verify(l => l.UpdateListing(It.IsAny<int>()), Times.Once);
        }

        //[Fact]
        //public void EditListing_NotExistId_ThrowArgumentOutOfRangeException()
        //{
        //    int notExistId = -1;

        //    Action action = () => _sut.EditListing(notExistId);

        //    action.Should().Throw<ArgumentOutOfRangeException>();
        //}
    }
}
