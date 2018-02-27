using Microsoft.EntityFrameworkCore;

namespace IWMS.Aider.Service
{
    public class IWMSDbContext : DbContext
    {
        public IWMSDbContext(DbContextOptions<IWMSDbContext> options):base(options)
        {

        }
        public DbSet<TBillUnicodeB2BRecord> TbillUnicodeB2BRecord { get; set; }
        public DbSet<TDefStyle> TDefStyle { get; set; }
        public DbSet<TBillBatchOut> TBillBatchOut { get; set; }
        public DbSet<TDefSku> TDefSku { get; set; }
        public DbSet<TDefWareHouse> TDefWareHouse { get; set; }
    }
}
