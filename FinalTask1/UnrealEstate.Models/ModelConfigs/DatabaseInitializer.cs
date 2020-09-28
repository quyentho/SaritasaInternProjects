using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Infrastructure.ModelConfigs
{
    public static class DatabaseInitializer
    {
        public static void InitializeSeedData(UnrealEstateDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);

            SeedUsers(context, userManager);

            SeedListingStatuses(context);

            SeedListings(context);

            SeedComments(context);


        }

        private static void SeedComments(UnrealEstateDbContext context)
        {
            if (!context.Comments.Any())
            {
                context.Comments.AddRange(new List<Comment>()
                {
                    new Comment {  ListingId = 1, Text = "comment 1", UserId = "2b3bffa2-d5b4-4bac-8a9b-8afa65ec5e85"}, // comment of user 1
                    new Comment {  ListingId = 1, Text = "comment 2", UserId = "2b3bffa2-d5b4-4bac-8a9b-8afa65ec5e85"},// comment of user 1
                    new Comment {  ListingId = 1, Text = "comment 3", UserId = "be26f642-e3b7-46bc-bd68-69f0d574548d"},// comment of user 2
                    new Comment {  ListingId = 1, Text = "comment 4", UserId = "be26f642-e3b7-46bc-bd68-69f0d574548d"},// comment of user 2
                    new Comment {  ListingId = 1, Text = "comment 5", UserId = "be26f642-e3b7-46bc-bd68-69f0d574548d"},// comment of user 2
                });

                context.SaveChanges();
            }
        }

        private static void SeedUsers(UnrealEstateDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {

            if (!context.ApplicationUsers.Any())
            {
                var user = new ApplicationUser() { Id = "2b3bffa2-d5b4-4bac-8a9b-8afa65ec5e85", Email = "user1@test.com", UserName = "user1@test.com" };
                IdentityResult check = userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
                if (check.Succeeded)
                {
                    Claim claim = new Claim(ClaimTypes.Email, user.Email);
                    userManager.AddClaimAsync(user, claim).GetAwaiter().GetResult();

                    userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
                }

                user = new ApplicationUser() { Id = "be26f642-e3b7-46bc-bd68-69f0d574548d", Email = "user2@test.com", UserName = "user2@test.com" };
                check = userManager.CreateAsync(user, "password").GetAwaiter().GetResult();

                if (check.Succeeded)
                {
                    Claim claim = new Claim(ClaimTypes.Email, user.Email);
                    userManager.AddClaimAsync(user, claim).GetAwaiter().GetResult();

                    userManager.AddToRoleAsync(user, "User").GetAwaiter().GetResult();
                }
            }
        }

        private static void SeedListings(UnrealEstateDbContext context)
        {
            if (!context.Listings.Any())
            {
                context.Listings.AddRange(new List<Listing>() {
                new Listing { AddressLine1 = "addressline1", AddressLine2 = "addressline2", Beds = 2, BuiltYear = 2019,Zip= "11111", City = "Hcm", Description = "description", StatusId = 1,UserId = "2b3bffa2-d5b4-4bac-8a9b-8afa65ec5e85"  },
                new Listing { AddressLine1 = "addressline copy 1", AddressLine2 = "addressline copy 2",Zip= "11111", Beds = 2, BuiltYear = 2019, City = "Hcm", Description = "description", StatusId = 1, UserId = "2b3bffa2-d5b4-4bac-8a9b-8afa65ec5e85" },
                new Listing { AddressLine1 = "addressline fake 1", AddressLine2 = "addressline fake 2",Zip= "22222", Beds = 2, BuiltYear = 2019, City = "ha noi", Description = "description", StatusId = 1, UserId = "be26f642-e3b7-46bc-bd68-69f0d574548d"},
                new Listing { AddressLine1 = "addressline fake fake 1", AddressLine2 = "addressline fake fake 2",Zip= "22222", Beds = 2, BuiltYear = 2019, City = "ha noi", Description = "description", StatusId = 1,UserId = "be26f642-e3b7-46bc-bd68-69f0d574548d" },
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
                role.Name = "User";
                roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }


        }
    }
}
