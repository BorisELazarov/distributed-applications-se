using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DAL.Context
{
    public class CardsAndTransactionsSystemDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Programming\C#\CardsAndTransactionsSystemProject\DAL\Data\CATSDb.mdf;Integrated Security=True")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            modelBuilder.Entity<Card>().HasIndex(x => x.CardNumber).IsUnique();
            modelBuilder.Entity<Card>().HasIndex(x => x.IBAN).IsUnique();
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id=1001,
                    Username = "admin",
                    Password = "admin1234!",
                    FirstName = "Admin",
                    LastName = "Adminov",
                    IsMale=true,
                    BirthDate= new DateOnly(2003, 2, 13)
                },
                new User
                {
                    Id = 1002,
                    Username = "MaIv",
                    Password = "MariaIvanova",
                    FirstName = "Maria",
                    LastName = "Ivanova",
                    IsMale = true,
                    BirthDate = new DateOnly(1985, 7, 12)
                });
            modelBuilder.Entity<Card>().HasData(
                new Card
                {
                    Id = 1001,
                    Title = "Main",
                    IBAN= "BG35NCFP46144127704117",
                    Balance = 650,
                    CardNumber = "4518466768345636",
                    SecurityCode = "Adminov",
                    UserId = 1001,
                    ValidThru = new DateOnly(2024, 7, 30)
                },
                new Card
                {
                    Id = 1002,
                    Title = "Private",
                    IBAN = "BG11PBFKR46144127704117",
                    Balance = 1600,
                    CardNumber = "4512356768345636",
                    SecurityCode = "Adminov",
                    UserId = 1002,
                    ValidThru = new DateOnly(2025, 2, 26)
                }
                //,
                //new Card
                //{
                //    Id = 1003,
                //    Title = "Savings",
                //    IBAN = "BG31GTHHF46144125735117",
                //    Balance = 1275,
                //    CardNumber = "4512356768345636",
                //    SecurityCode = "Adminov",
                //    UserId = 1001,
                //    ValidThru = new DateOnly(2026, 2, 19)
                //}
                );
            modelBuilder.Entity<Transaction>().HasData(
                new Transaction {
                    Id=1001,
                    Title="Rent",
                    Description="Payment for my rent",
                    IBAN= "BG11PBFKR46144127704117",
                    Sum=500,
                    DateOfTransaction= new DateOnly(2024, 5, 26),
                    CardId= 1001
                },
                new Transaction
                {
                    Id = 1002,
                    Title = "Refund",
                    Description = "Return of some of the rent money",
                    IBAN = "BG11PBFKR46144127704117",
                    Sum = 50,
                    DateOfTransaction = new DateOnly(2024, 5, 27),
                    CardId=1002
                }
                );
        }
    }
}
