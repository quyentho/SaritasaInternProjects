using Autofac;
using Autofac.Extras.Moq;
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
    [Collection("Database collection")]
    public class UserServiceTests
    {

        private readonly DatabaseFixture _databaseFixture;
        private UserService _sut;

        public UserServiceTests(DatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture;
        }

        [Fact]
        public void GetUsers_WhenCall_ReturnsListUsers()
        {
            using (var mock = AutoMock
                .GetLoose(cfg => cfg.RegisterInstance(_databaseFixture.FakeUserRepository).As<IUserRepository>()))
            {
                _sut = mock.Create<UserService>();
                List<UnrealEstateUser> result = _sut.GetUsers();

                result.Should().BeEquivalentTo(_databaseFixture.FakeUsers);
            }

        }

        [Theory()]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetUser_ValidUserId_ReturnsExactUser(int validId)
        {
            using (var mock = AutoMock
                .GetLoose(cfg => cfg.RegisterInstance(_databaseFixture.FakeUserRepository).As<IUserRepository>()))
            {
                _sut = mock.Create<UserService>();
                UnrealEstateUser user = _sut.GetUser(validId);

                user.Should().Be(_databaseFixture.FakeUsers[validId - 1]);
            }

        }

        [Fact]
        public void AddUser_ValidNewUser_CallsToAddUserFunctionOfUserRepository()
        {
            using (var mock = AutoMock.GetLoose())
            {
                _sut = mock.Create<UserService>();
                _sut.AddUser(new UnrealEstateUser());

                mock.Mock<IUserRepository>().Verify(u => u.AddUser(It.IsAny<UnrealEstateUser>()), Times.Once);
            }
        }

        [Theory()]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void UpdateUser_WhenCall_CallToUpdateUserFunctionOfUserRepository(int id)
        {
            using (var mock = AutoMock.GetLoose())
            {
                _sut = mock.Create<UserService>();

                _sut.UpdateUser(new UnrealEstateUser() { Id = id });

                mock.Mock<IUserRepository>().Verify(u => u.UpdateUser(It.Is<UnrealEstateUser>(u => u.Id == id)), Times.Once);
            }
        }

        [Theory()]
        [InlineData(-1)]
        [InlineData(5)]
        [InlineData(6)]
        public void UpdateUser_NoneExistingUser_ThrowArgumentOutOfRangeException(int id)
        {
            using (var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(_databaseFixture.FakeUserRepository).As<IUserRepository>()))
            {
                _sut = mock.Create<UserService>();

                Action result = () => _sut.UpdateUser(new UnrealEstateUser() { Id = id });

                result.Should().Throw<ArgumentOutOfRangeException>();
            }
        }

        [Fact]
        public void RemoveUser_WhenCall_CallToRemoveUserFunctionOfUserRepository()
        {
            using (var mock = AutoMock.GetLoose())
            {
                _sut = mock.Create<UserService>();

                _sut.RemoveUser(It.IsAny<int>());

                mock.Mock<IUserRepository>().Verify(u => u.RemoveUser(It.IsAny<int>()), Times.Once);
            }
        }

        [Theory()]
        [InlineData(-1)]
        [InlineData(5)]
        [InlineData(6)]
        public void RemoveUser_NoneExistingUser_ThrowArgumentOutOfRangeException(int id)
        {
            using (var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(_databaseFixture.FakeUserRepository).As<IUserRepository>()))
            {
                _sut = mock.Create<UserService>();

                Action result = () => _sut.RemoveUser(id);

                result.Should().Throw<ArgumentOutOfRangeException>();
            }
        }
    }
}
