using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ModelConfigs
{
    public static class DatabaseInitializer
    {
        public static void InitializeSeedData(UnrealEstateDbContext context)
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

            if (!context.Comments.Any())
            {
                context.Comments.AddRange(new List<Comment>()
                {
                    new Comment {  ListingId = 1, Text = "comment 1", UserId = "1e9b47a2-9300-4514-b313-0f73279ab7c6"},
                    new Comment {  ListingId = 1, Text = "comment 2", UserId = "1e9b47a2-9300-4514-b313-0f73279ab7c6"},
                    new Comment {  ListingId = 1, Text = "comment 3", UserId = "8a6f739a-0b69-4468-813d-4dc96a02f4d5"},
                    new Comment {  ListingId = 10, Text = "comment 4", UserId = "8a6f739a-0b69-4468-813d-4dc96a02f4d5"},
                    new Comment {  ListingId = 10, Text = "comment 4", UserId = "942243bb-9d77-4d48-92f5-438d31cfc9a4"},
                    new Comment {  ListingId = 10, Text = "comment 4", UserId = "942243bb-9d77-4d48-92f5-438d31cfc9a4"},
                });
            }

            if (!context.Users.Any())
            {
                context.Users.AddRange(new List<User>()
                {
                    new User { Email = "test@gmail.com"  },
                    new User { Email = "test2@gmail.com"  },
                    new User { Email = "test3@gmail.com"  },
                    new User { Email = "test4@gmail.com"  }
                });

                context.SaveChanges();
            }
        }
    }
}
