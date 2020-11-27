using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using UnrealEstate.Models;
using UnrealEstate.Models.Repositories;

namespace UnrealEstate.Services.Tests
{
    public class DatabaseFixture : IDisposable
    {
        UnrealEstateDbContext _testContext;
        public List<Listing> FakeListings { get; private set; }
        public IListingRepository FakeListingRepository { get; private set; }
        public List<User> FakeUsers { get; private set; }
        public IUserRepository FakeUserRepository { get; private set; }

        public DatabaseFixture()
        {
            #region Fake Listing data
            // fake data to test
            FakeListings = new List<Listing>();
            FakeListings.Add(new Listing() { Id = 1, StatusId = 1, UserId = "1", Zip = "aaaaa", AddressLine1 = "address1", City = "city1" });
            FakeListings.Add(new Listing() { Id = 2, StatusId = 1, UserId = "2", Zip = "aaaaa", AddressLine1 = "address2", City = "city2" });
            FakeListings.Add(new Listing() { Id = 3, StatusId = 1, UserId = "3", Zip = "bbbbb", AddressLine1 = "address3", City = "city3" });
            FakeListings.Add(new Listing() { Id = 4, StatusId = 1, UserId = "3", Zip = "bbbbb", AddressLine1 = "address4", City = "city4" });
            FakeListings.Add(new Listing() { Id = 5, StatusId = 2, UserId = "3", Zip = "bbbbb", AddressLine1 = "address4", City = "city4" });
            #endregion

            #region Fake User Data
            FakeUsers = new List<User>();
            FakeUsers.Add(new User() { Id = "1", Email = "user@test.com", Status = true });
            FakeUsers.Add(new User() { Id = "2", Email = "user@test.com", Status = true });
            FakeUsers.Add(new User() { Id = "3", Email = "user@test.com", Status = true });
            #endregion

            // create in memory database to test.
            var options = new DbContextOptionsBuilder<UnrealEstateDbContext>()
                         .UseInMemoryDatabase(databaseName: "TestDb")
                         .Options;

            // add data to in memory db.
            _testContext = new UnrealEstateDbContext(options);
            _testContext.Listings.AddRange(FakeListings);
            _testContext.Users.AddRange(FakeUsers);
            _testContext.SaveChanges();

            FakeListingRepository = new ListingRepository(_testContext);
            FakeUserRepository = new UserRepository(_testContext);
        }

        public void Dispose()
        {
            FakeListings.Clear();
            _testContext.Database.EnsureDeleted();
            _testContext.Dispose();
        }
    }
}
