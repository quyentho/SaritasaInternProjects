using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace UnrealEstate.Models.ModelConfigs
{
    public static class DatabaseInitializer
    {
        public static void InitializeSeedData(UnrealEstateDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedListingStatuses(context);

            SeedListings(context);

            SeedRoles(roleManager);

            SeedUsers(context, userManager);

            SeedComments(context);


        }

        private static void SeedComments(UnrealEstateDbContext context)
        {
            if (!context.Comments.Any())
            {
                context.Comments.AddRange(new List<Comment>()
                {
                    new Comment {  ListingId = 1, Text = "comment 1", UserId = "4c79e6d0-311c-4004-a9d0-c88e1e83de8d"}, // comment of user 1
                    new Comment {  ListingId = 1, Text = "comment 2", UserId = "4c79e6d0-311c-4004-a9d0-c88e1e83de8d"},// comment of user 1
                    new Comment {  ListingId = 1, Text = "comment 3", UserId = "b7364c11-a384-4fdc-a98b-cad1dd8c36b9"},// comment of user 2
                    new Comment {  ListingId = 10, Text = "comment 4", UserId = "c2ebf714-0538-4b33-a999-ed3606548cb3"},// comment of user 3
                    new Comment {  ListingId = 10, Text = "comment 4", UserId = "c2ebf714-0538-4b33-a999-ed3606548cb3"},// comment of user 3
                    new Comment {  ListingId = 10, Text = "comment 4", UserId = "ca079fa8-3fe9-482a-ac55-652257844bba"},// comment of user 4
                });

                context.SaveChanges();
            }
        }

        private static void SeedUsers(UnrealEstateDbContext context, Microsoft.AspNetCore.Identity.UserManager<User> userManager)
        {
           
            if (!context.Users.Any())
            {
                var user = new User() { Email = "user1@test.com", UserName = "user1@test.com" };
                IdentityResult check = userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
                if (check.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
                }

                user = new User() { Email = "user2@test.com", UserName = "user2@test.com" };
                check = userManager.CreateAsync(user, "password").GetAwaiter().GetResult();

                if (check.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "RegularUser").GetAwaiter().GetResult();
                }

                user = new User() { Email = "user3@test.com", UserName = "user3@test.com" };
                check = userManager.CreateAsync(user, "password").GetAwaiter().GetResult();

                if (check.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "RegularUser").GetAwaiter().GetResult();
                }

                user = new User() { Email = "user4@test.com", UserName = "user4@test.com" };
                check = userManager.CreateAsync(user, "password").GetAwaiter().GetResult();

                if (check.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "RegularUser").GetAwaiter().GetResult();
                }
            }
        }

        private static void SeedListings(UnrealEstateDbContext context)
        {
            if (!context.Listings.Any())
            {
                context.Listings.AddRange(new List<Listing>() {
                new Listing { AddressLine1 = "addressline1", AddressLine2 = "addressline2", Beds = 2, BuiltYear = 2019,Zip= "11111", City = "Hcm", Description = "description", StatusId = 1 },
                new Listing { AddressLine1 = "addressline copy 1", AddressLine2 = "addressline copy 2",Zip= "11111", Beds = 2, BuiltYear = 2019, City = "Hcm", Description = "description", StatusId = 1 },
                new Listing { AddressLine1 = "addressline copy 1", AddressLine2 = "addressline copy 2",Zip= "11111", Beds = 2, BuiltYear = 2019, City = "Hcm", Description = "description", StatusId = 1 },
                new Listing { AddressLine1 = "addressline copy 1", AddressLine2 = "addressline copy 2",Zip= "11111", Beds = 2, BuiltYear = 2019, City = "Hcm", Description = "description", StatusId = 1 },
                new Listing { AddressLine1 = "addressline fake 1", AddressLine2 = "addressline fake 2",Zip= "22222", Beds = 2, BuiltYear = 2019, City = "ha noi", Description = "description", StatusId = 1 },
                new Listing { AddressLine1 = "addressline fake 1", AddressLine2 = "addressline fake 2",Zip= "22222", Beds = 2, BuiltYear = 2019, City = "ha noi", Description = "description", StatusId = 1 },
                new Listing { AddressLine1 = "addressline fake 1", AddressLine2 = "addressline fake 2",Zip= "22222", Beds = 2, BuiltYear = 2019, City = "ha noi", Description = "description", StatusId = 1 },
                new Listing { AddressLine1 = "addressline fake fake 1", AddressLine2 = "addressline fake fake 2",Zip= "22222", Beds = 2, BuiltYear = 2019, City = "ha noi", Description = "description", StatusId = 1 },
                new Listing { AddressLine1 = "addressline fake fake 1", AddressLine2 = "addressline fake fake 2",Zip= "22222", Beds = 2, BuiltYear = 2019, City = "ha noi", Description = "description", StatusId = 1 },
                });

                context.SaveChanges();
            }
        }

        private static void SeedListingStatuses(UnrealEstateDbContext context)
        {
            if (!context.ListingStatuses.Any())
            {
                context.ListingStatuses.AddRange(new List<ListingStatus>() {
                    new ListingStatus {  Name = "Active"},
                    new ListingStatus { Name = "Disable"},
                    new ListingStatus { Name = "Cancel"},
                    new ListingStatus {  Name = "Sold"},
                    new ListingStatus { Name = "Not Available"}
                });

                context.SaveChanges();
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {

            if (!roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }

            if (!roleManager.RoleExistsAsync("RegularUser").GetAwaiter().GetResult())
            {
                var role = new IdentityRole();
                role.Name = "RegularUser";
                roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }


        }
    }
}
