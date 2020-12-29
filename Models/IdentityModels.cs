﻿using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EIRLSSAssignment1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //Additional fields

        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public  bool IsBlackListed { get; set; }
        public bool IsTrustedCustomer { get; set; }
        public int? DrivingLicenseId { get; set; }
        public DrivingLicense DrivingLicense { get; set; }
        public int? SupportingDocumentId { get; set; }
        public SupportingDocument SupportingDocument { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<EIRLSSAssignment1.Models.FuelType> FuelTypes { get; set; }

        public System.Data.Entity.DbSet<EIRLSSAssignment1.Models.Vehicle> Vehicles { get; set; }

        public System.Data.Entity.DbSet<EIRLSSAssignment1.Models.VehicleType> VehicleTypes { get; set; }

        public System.Data.Entity.DbSet<EIRLSSAssignment1.Models.DrivingLicense> DrivingLicenses { get; set; }
    }
}