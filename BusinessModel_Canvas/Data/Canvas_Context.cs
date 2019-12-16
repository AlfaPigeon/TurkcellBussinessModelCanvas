using BusinessModel_Canvas.Models;
using Microsoft.EntityFrameworkCore;
namespace BusinessModel_Canvas.Data
{
    public class Canvas_Context : DbContext
    {

        public Canvas_Context(DbContextOptions<Canvas_Context> options) : base(options){
            



        }
        public DbSet<Firm> Firms { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Communication> Communications { get; set; }
        public DbSet<PrivateLife> PrivateLifes { get; set; }
        public DbSet<Channel> ShippingChannels { get; set; }
        public DbSet<IncomeFlow> IncomeFlows { get; set; }
        public DbSet<OutcomeFlow> OutcomeFlows { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceInfo> ResourceInfo { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<CustomerRelation> CustomerRelations { get; set; }
        public DbSet<R_Partners> R_Partners { get; set; }
        public DbSet<R_Works> R_Works { get; set; }
        public DbSet<R_ProvidesServices> R_ProvidesServices { get; set; }
        public DbSet<R_ProvidesResource> R_ProvidesResources { get; set; }
        public DbSet<R_Channeled> R_Channeled { get; set; }
        public DbSet<R_Relations> R_Relations { get; set; }


    }
}
