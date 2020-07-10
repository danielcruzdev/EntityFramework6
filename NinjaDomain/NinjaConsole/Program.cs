using NinjaDomain.Classes;
using NinjaDomain.Classes.Enums;
using NinjaDomain.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace NinjaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());
            //InsertMultipleNinja();
            //QueryAndUodateNinja();
            //QueryAndUpdateNinjaDisconnected();
            //SimpleNinjaQueries();
            //RetrieveDataWithFind();
            InsertNinjaWithEquipment();
            Console.ReadLine();
        }

        #region InsertOneNinja
        private static void InsertNinja()
        {
            var ninja = new Ninja
            {
                Name = "Renan",
                ServerdInOniwaban = true,
                DateofBirth = new DateTime(1990, 12, 22),
                ClanId = 2
            };

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                // -> Insert a new ninja!
                context.Ninjas.Add(ninja);
                context.SaveChanges();
            }
        }
        #endregion

        #region InserMultipleNinjas
        private static void InsertMultipleNinja()
        {
            var ninja = new Ninja
            {
                Name = "Maria",
                ServerdInOniwaban = true,
                DateofBirth = new DateTime(1990, 12, 22),
                ClanId = 2
            };

            var ninja2 = new Ninja
            {
                Name = "Tony",
                ServerdInOniwaban = true,
                DateofBirth = new DateTime(1990, 12, 22),
                ClanId = 2
            };

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                // -> Insert a new ninja!
                context.Ninjas.AddRange(new List<Ninja> { ninja, ninja2 });
                context.SaveChanges();
            }
        }
        #endregion

        #region NinjaQueries
        private static void SimpleNinjaQueries()
        {
            using (var context = new NinjaContext())
            {
                // -> Select and show ninjas name.
                var ninjas = context.Ninjas.Where(n => n.Name == "Daniel");
                foreach (var ninja in ninjas)
                {
                    Console.WriteLine(ninja.Name);
                }
            }
        }
        #endregion

        #region UpdateNinja
        private static void QueryAndUodateNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                ninja.ServerdInOniwaban = (!ninja.ServerdInOniwaban);
                context.SaveChanges();
            }
        }
        #endregion

        #region QueryAndUpdateNinjaDisconnected
        private static void QueryAndUpdateNinjaDisconnected()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }

            ninja.ServerdInOniwaban = (!ninja.ServerdInOniwaban);

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Attach(ninja);
                context.Entry(ninja).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        #endregion

        #region RetriveDataWithFind
        private static void RetrieveDataWithFind()
        {
            var keyval = 4;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(keyval);
                Console.WriteLine($"After FIND#1 {ninja.Name}");

                var someNinja = context.Ninjas.Find(keyval);
                Console.WriteLine($"After FIND#2 {someNinja.Name}");
                ninja = null;
            }
        }
        #endregion 

        #region RetriveDataWithStoredProcedure
        private static void RetrieveDataWithStoredProcedure()
        {
            var keyval = 4;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas.SqlQuery("EXEC splNinjas").ToList();
                foreach (var ninja in ninjas)
                {
                    Console.WriteLine(ninja.Name);
                }
            }
        }
        #endregion 

        #region DeleteNinja
        private static void DeleteNinja()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Entry(ninja).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
        #endregion

        #region DeleteNinjaWithKeyValue
        private static void DeleteNinjaWithKeyValue()
        {
            var keyval = 1;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(keyval);
                context.Ninjas.Remove(ninja);
                context.SaveChanges();
            }
        }
        #endregion

        #region DeleteNinjaWithStoreProcedure
        private static void DeleteNinjaWithStoreProcedure()
        {
            var keyval = 3;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Database.ExecuteSqlCommand("spdNinja {0}", keyval);
            }
        }
        #endregion

        #region InsertNinjaWithEquipment
        private static void InsertNinjaWithEquipment()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = new Ninja
                {
                    Name = "Kacy Catazanco",
                    ServerdInOniwaban = false,
                    DateofBirth = new DateTime(2020, 5, 1),
                    ClanId = 1
                };
                var muscles = new NinjaEquipment
                {
                    Name = "Muscles",
                    Type = EquipmentType.Tool,
                };
                var spunk = new NinjaEquipment
                {
                    Name = "Spunk",
                    Type = EquipmentType.Weapon
                };
                context.Ninjas.Add(ninja);
                ninja.EquipmentOwned.Add(muscles);
                ninja.EquipmentOwned.Add(spunk);
                context.SaveChanges();
            }
        }
        #endregion

        #region SimpleNinjaGraphQuery
        private static void SimpleNinjaGraphQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = context.Ninjas.FirstOrDefault(n => n.Name.StartsWith("Kacy"));
                Console.WriteLine($"Ninja Retrieved: {ninja.Name}");
                context.Entry(ninja).Collection(n => n.EquipmentOwned).Load();
            }
        }
        #endregion

        #region ProjectionQuery
        private static void ProjectionQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas
                    .Select(n => new { n.Name, n.DateofBirth, n.EquipmentOwned })
                    .ToList();
            }
        }
        #endregion
    }
}
