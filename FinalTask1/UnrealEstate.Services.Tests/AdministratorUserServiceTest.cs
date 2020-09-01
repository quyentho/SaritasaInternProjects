using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnrealEstate.Models;
using Xunit;

namespace UnrealEstate.Services.Tests
{
    public class AdministratorUserServiceTest
    {
        Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
        List<UnrealEstateUser> _fakeUsers;
        AdministratorUserService _sut;
        public AdministratorUserServiceTest()
        {
            _fakeUsers = new List<UnrealEstateUser>();
            _fakeUsers.Add(new UnrealEstateUser() { Id = 1, Email = "user@test.com", Status = true });
            _fakeUsers.Add(new UnrealEstateUser() { Id = 2, Email = "user@test.com", Status = true });
            _fakeUsers.Add(new UnrealEstateUser() { Id = 3, Email = "user@test.com", Status = true });
            _sut = new AdministratorUserService(_mockUserRepository.Object);
        }

        [Fact]
        public void GetUsers_WhenCall_ReturnsListUsers()
        {


            _mockUserRepository.Setup(u => u.GetUsers()).Returns(_fakeUsers);

            List<UnrealEstateUser> result = _sut.GetUsers();

            result.Should().BeEquivalentTo(_fakeUsers);
        }

        [Theory()]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetUser_ValidUserId_ReturnsExactUser(int validId)
        {
            _mockUserRepository.Setup(u => u.GetUser(validId)).Returns(_fakeUsers[validId - 1]);

            UnrealEstateUser user = _sut.GetUser(validId);

            user.Should().Be(_fakeUsers[validId - 1]);
        }

        [Fact]
        public void AddUser_ValidNewUser_ReturnsTrue()
        {
            _mockUserRepository.Setup(m => m.AddUser(It.IsAny<UnrealEstateUser>())).Returns(true);

            bool addResult = _sut.AddUser(new UnrealEstateUser());

            addResult.Should().BeTrue();
        }

        [Theory()]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void UpdateUser_ExistingUser_ReturnsTrue(int id)
        {
            _mockUserRepository.Setup(m => m.UpdateUser(id, It.IsAny<UnrealEstateUser>())).Returns(true);

            bool updateResult = _sut.UpdateUser(id, new UnrealEstateUser());

            updateResult.Should().BeTrue();
        }

        [Theory()]
        [InlineData(-1)]
        [InlineData(5)]
        [InlineData(6)]
        public void UpdateUser_NoneExistingUser_ReturnsFalse(int id)
        {
            _mockUserRepository.Setup(m => m.UpdateUser(id, It.IsAny<UnrealEstateUser>())).Returns(false);

            bool updateResult = _sut.UpdateUser(id, new UnrealEstateUser());

            updateResult.Should().BeFalse();
        }

        [Theory()]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void RemoveUser_ExistingUser_ReturnsTrue(int id)
        {
            _mockUserRepository.Setup(m => m.RemoveUser(id)).Returns(true);

            bool removeResult = _sut.RemoveUser(id);

            removeResult.Should().BeTrue();
        }

        [Theory()]
        [InlineData(-1)]
        [InlineData(5)]
        [InlineData(6)]
        public void RemoveUser_NoneExistingUser_ReturnsTrue(int id)
        {
            _mockUserRepository.Setup(m => m.RemoveUser(id)).Returns(false);

            bool removeResult = _sut.RemoveUser(id);

            removeResult.Should().BeFalse();
        }
    }
}
