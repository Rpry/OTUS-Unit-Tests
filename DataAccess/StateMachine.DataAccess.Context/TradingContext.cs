using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Trading.StateMachine.DataAccess.Models;

namespace Trading.StateMachine.DataAccess.Context
{
    public class TradingContext : DbContext
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options"></param>
        public TradingContext(DbContextOptions<TradingContext> options) : base(options)
        {
        }

        /// <summary>
        /// Заявки
        /// </summary>
        public DbSet<Application> Applications { get; set; }

        /// <summary>
        /// Лоты процедуры
        /// </summary>
        public DbSet<Lot> Lots { get; set; }

        /// <summary>
        /// Ошибки лота
        /// </summary>
        public DbSet<LotError> LotErrors { get; set; }

        /// <summary>
        /// Документы лота
        /// </summary>
        public DbSet<LotDocument> LotDocuments { get; set; }

        /// <summary>
        /// Доступные типы аукционов
        /// </summary>
        public DbSet<ProcedureType> ProcedureTypes { get; set; }

        /// <summary>
        /// Ресурсы типов процедур
        /// </summary>
        public DbSet<ProcedureTypeResource> ProcedureTypeResources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.UseSerialColumns();

            modelBuilder.Entity<Application>()
                .HasOne(p => p.Lot)
                .WithMany(t => t.Applications)
                .HasForeignKey(p => p.LotId)
                .IsRequired();

            modelBuilder.Entity<Application>()
                .Property(a => a.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<ProcedureType>()
                .ToTable("ProcedureTypes")
                .HasKey(p => p.Id);

            modelBuilder.Entity<ProcedureTypeResource>()
                .HasOne(p => p.ProcedureType)
                .WithMany(x => x.Resources)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lot>()
                .HasOne(a => a.ProcedureType)
                .WithMany(x => x.Lots)
                .HasForeignKey(x => x.ProcedureCode)
                .IsRequired();

            modelBuilder.Entity<LotError>()
                .HasOne(a => a.Lot)
                .WithMany(x => x.Errors);

            modelBuilder
                .Entity<Lot>()
                .Property(e => e.Created)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            
            modelBuilder
                .Entity<Lot>()
                .Property(e => e.Published)
                .HasConversion(v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);
            
            modelBuilder
                .Entity<Lot>()
                .Property<int>("SerialNumber")
                .ValueGeneratedOnAdd()
                .HasColumnType("serial")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            modelBuilder
                .Entity<LotDocument>()
                .Property(e => e.Created)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            modelBuilder
                .Entity<LotDocument>()
                .Property(e => e.SigningEndDateTime)
                .HasConversion(v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);

            modelBuilder
                .Entity<Application>()
                .Property(e => e.Created)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            modelBuilder
                .Entity<Application>()
                .Property(e => e.Published)
                .HasConversion(v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);

            modelBuilder
                .Entity<Application>()
                .Property<int>("SerialNumber")
                .ValueGeneratedOnAdd()
                .HasColumnType("serial")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();
            return result;
        }
    }
}
