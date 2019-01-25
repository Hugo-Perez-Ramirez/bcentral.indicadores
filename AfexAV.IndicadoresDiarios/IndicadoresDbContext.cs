using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AfexAV.IndicadoresDiarios
{
    public class IndicadoresDbContext : DbContext
    {
        public IndicadoresDbContext() : base("IndicadoresDbContext")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<TipoCambio> TipoCambio { get; set; }
    }
}
