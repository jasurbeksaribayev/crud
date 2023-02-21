using ConsoleTables;
using CRUD;
using CRUD.Malumotlar;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


internal class Program
{
    private static void Main(string[] args)
    {
        ICRUDfunksiyalari functions = new CRUDfunksiyalari();


        functions.Creator(new FoydalanuvchiXossalari
        {
            Name = "Jasur",
            mealS = new List<Meals>
                    {
                        new Meals {FoodID = 1.1,Amount = 6 }, 
                        new Meals {FoodID = 3.1,Amount = 2 },
                        new Meals {FoodID = 1.2,Amount = 0.5 },
                        new Meals {FoodID = 2.1,Amount = 3 },

        }
        });

  
        Console.ReadKey();
    }
}