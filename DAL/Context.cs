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
        public DbSet<Product> products { get; set; }
        public DbSet<Campaign> campaigns { get; set; }
        public DbSet<ProductPhoto> productPhotos { get; set; }
        public DbSet<Color> colors { get; set; }
        public DbSet<ColorProduct> colorProducts { get; set; }
        public DbSet<Tag> tags { get; set; }
        public DbSet<ProductTag> productTags { get; set; }
        public DbSet<ProductRelation> productRelations { get; set; }
        public DbSet<HomeProductSlider> homeProductSliders { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogPhoto> BlogPhotos { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SalesProduct> SalesProducts { get; set; }
    }
}