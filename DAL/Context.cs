using Asp.net_E_commerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.DAL
{
    public class Context : IdentityDbContext<AppUser>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        public DbSet<SliderCompany> sliderCompany { get; set; }
        public DbSet<ContactDetails> contactDetails { get; set; }
        public DbSet<FeaturesBanner> featuresBanners { get; set; }
        public DbSet<Subscribe> subscribe { get; set; }
        public DbSet<CustomerMailList> customerMailLists { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Brand> brands { get; set; }
        public DbSet<CategoryBrand> categoryBrands { get; set; }
    }
}