using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace BTravel.DAL
{
    public static class DatabaseSeeder
    {
        public static void SeedRolesAndServices(BTravelDbContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new() { Name = "Admin", Description = "Admin", CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true },
                    new() { Name = "Client", Description = "Client", CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true },
                };

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            if (!context.CommonUsers.Any())
            {
                var roleAdminId = context.Roles.FirstOrDefault(r => r.Name == "Admin").RoleId;

                PasswordHasher<DAL.Entities.CommonUser> hasher = new PasswordHasher<DAL.Entities.CommonUser>();
                var hashedPassword = hasher.HashPassword(new DAL.Entities.CommonUser(), "123");

                var newCommonUser = new CommonUser // for emergency login
                {
                    FullName = "Super Admin",
                    PhoneNumber = "1234567890",
                    PrimaryMail = "m@m.com",
                    IsPrimaryMailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = 0,
                    IsDeleted = true,
                    IsActive = false,
                    Password = hashedPassword,
                    CompanyName = "BTravelMate",
                    RoleId = roleAdminId,
                };
                context.CommonUsers.Add(newCommonUser);

                var newCommonUserAdmin = new CommonUser // for admin login
                {
                    FullName = "Mahmoud Deiab",
                    PhoneNumber = "01101139998",
                    PrimaryMail = "Apps@btravelmate.com",
                    IsPrimaryMailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = 0,
                    IsDeleted = false,
                    IsActive = true,
                    Password = hashedPassword,
                    CompanyName = "BTravelMate",
                    RoleId = roleAdminId,
                };
                context.CommonUsers.Add(newCommonUserAdmin);

                context.SaveChanges();
            }

            if (!context.AppServices.Any())
            {
                var appServices = new List<AppService>
                {
                    new() { CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true, Name = "Add_CommonUser", ViewName="Add CommonUser", Description="Add CommonUser", ClassName="Dashboard" },
                    new() { CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true, Name = "View_CommonUser", ViewName="View CommonUser", Description="View CommonUser", ClassName="Dashboard" },
                    new() { CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true, Name = "Delete_CommonUser", ViewName="Delete CommonUser", Description="Delete CommonUser", ClassName="Dashboard" },
                    new() { CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true, Name = "Edit_CommonUser", ViewName="Edit CommonUser", Description="Edit CommonUser", ClassName="Dashboard" },
                    new() { CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true, Name = "Add_Contract", ViewName="Add Contract", Description="Add Contract", ClassName="Dashboard" },
                    new() { CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true, Name = "View_Contract_Dashboard", ViewName="View Contract", Description="View Contract", ClassName="Dashboard" },
                    new() { CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true, Name = "UploadFile_Dashboard", ViewName="Upload File", Description="Upload File", ClassName="Dashboard" },
                    new() { CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true, Name = "Delete_Contract", ViewName="Delete Contract", Description="Delete Contract", ClassName="Dashboard" },
                    new() { CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true, Name = "Edit_Contract_Dashboard", ViewName="Edit Contract", Description="Edit Contract", ClassName="Dashboard" },
                    new() { CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true, Name = "DeleteFile_Dashboard", ViewName="Delete File", Description="Delete File", ClassName="Dashboard" },
                    new() { CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true, Name = "View_Dashboard", ViewName="View Dashboard", Description="View Dashboard", ClassName="Dashboard" },
                    new() { CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true, Name = "View_Contract_Portal", ViewName="View Contract", Description="View Contract", ClassName="Portal" },
                    new() { CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true, Name = "Sign_Contract_Portal", ViewName="Sign Contract", Description="Sign Contract", ClassName="Portal" },
                    new() { CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true, Name = "Download_Contract_Portal", ViewName="Download Contract", Description="Download Contract", ClassName="Portal" },
                };

                context.AppServices.AddRange(appServices);
                context.SaveChanges();

                var portalServices = context.AppServices.Where(s => s.ClassName == "Portal").ToList();
                var roleClientId = context.Roles.FirstOrDefault(r => r.Name == "Client").RoleId;

                for (int i = 0; i < portalServices.Count; i++)
                {
                    var roleAppService = new RoleAppService() { AppServiceId = portalServices[i].AppServiceId, RoleId = roleClientId, CreatedAt = DateTime.UtcNow, IsDeleted = false, IsActive = true };
                    context.RoleAppServices.Add(roleAppService);
                }

                context.SaveChanges();
            }
        }
    }
}