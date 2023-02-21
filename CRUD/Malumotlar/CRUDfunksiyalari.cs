using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Malumotlar
{
    public class CRUDfunksiyalari : ICRUDfunksiyalari
    {

        private SortedList<double, string> mealNames = new SortedList<double, string>()
        {
            {1.1,"Osh-25000" },
            {1.2,"Lagmon-23000" },
            {2.1,"qatiq-6000" },
            {3.1,"Cola-9000" }

        };

        private string path = "../../../Malumotlar/JsonBaza.json";
        public FoydalanuvchiXossalari Creator(FoydalanuvchiXossalari foydalanuvchiXossalari)
        {
            var data = getAll();
           
            if (data.LastOrDefault() == null)
            {
                foydalanuvchiXossalari.Id = 1;
            }
            else
            {
                foydalanuvchiXossalari.Id = data.LastOrDefault().Id + 1;
            }

            foydalanuvchiXossalari.mealS = foydalanuvchiXossalari.mealS.
                DistinctBy(a => a.FoodID).ToList();
            
            for (int i = 0; i < foydalanuvchiXossalari.mealS.Count; i++)
            {
                bool IsMeal = mealNames.Select(a => a.Key).
                              Contains(foydalanuvchiXossalari.mealS[i].FoodID);

               

                if (IsMeal && foydalanuvchiXossalari.mealS[i].Amount!>=0.5)
                {
                    
                    foydalanuvchiXossalari.mealS[i].foodName =
                        mealNames[foydalanuvchiXossalari.mealS[i].FoodID];
                    IsMeal= false;
                }
                else
                {
                    foydalanuvchiXossalari.mealS.RemoveAt(i);
                    Console.WriteLine("Ovqat ID xato");
                    i--;
                }
            }
            //foydalanuvchiXossalari.mealS = foydalanuvchiXossalari.mealS.DistinctBy(a => a.FoodID).ToList();
            data.Add(foydalanuvchiXossalari);
            string JsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            StreamWriter writer = new StreamWriter(path);
            writer.Write(JsonData);
            writer.Close();

            return foydalanuvchiXossalari;

        }

        public FoydalanuvchiXossalari get(Func<FoydalanuvchiXossalari, bool> func)
        {
            return getAll(func).FirstOrDefault();

        }

        public List<FoydalanuvchiXossalari> getAll(Func<FoydalanuvchiXossalari, bool> funcc = null)
        {
            StreamReader reader = new StreamReader(path);
            string datas = reader.ReadToEnd();
            reader.Close();
            if (string.IsNullOrEmpty(datas))
            {
                StreamWriter writer = new StreamWriter(path);
                writer.Write("[]");
                writer.Close();
                return new List<FoydalanuvchiXossalari>();
            }
            var result =
                JsonConvert.DeserializeObject<List<FoydalanuvchiXossalari>>(datas);
            if (funcc != null)
            {
                return result.Where(funcc).ToList();
            }
            return result;
        }


        public bool Delete(int id)
        {
            var result = getAll();

            bool IsIdDelete = result.Any(a => a.Id == id);
            if (IsIdDelete)
            {
                var res = result.
                    Where(f => f.Id != id);
                StreamWriter writer = new StreamWriter(path);
                string JsonFile = JsonConvert.SerializeObject(res, Formatting.Indented);
                writer.Write(JsonFile);
                writer.Close();
                //return IsIdDelete;
            }
            return IsIdDelete;
        }

        public bool Update(FoydalanuvchiXossalari foydalanuvchiXossalari)
        {
            var result = getAll();

            var resName = result.
                FirstOrDefault(a => a.Id == foydalanuvchiXossalari.Id);
            
            if (resName == null)
            {
                return false;
            }
            else
            {
                if (foydalanuvchiXossalari.Name != null && foydalanuvchiXossalari.mealS == null)
                {

                    resName.Name = foydalanuvchiXossalari.Name;

                    resName.mealS = resName.mealS.Where(a => a.Amount != 0.0).ToList();

                    string JsonFile = JsonConvert.SerializeObject(result, Formatting.Indented);

                    StreamWriter writer = new StreamWriter(path);
                    writer.Write(JsonFile);
                    writer.Close();
                    return true;
                }
                else if (foydalanuvchiXossalari.Name != null && foydalanuvchiXossalari.mealS != null)
                {

                    resName.Name = foydalanuvchiXossalari.Name;
                    
                    for (int i = 0; i < foydalanuvchiXossalari.mealS.Count; i++)
                    {
                        
                        var IsMealName = resName.mealS.
                        FirstOrDefault(m => m.FoodID == foydalanuvchiXossalari.mealS[i].FoodID);
                        
                        bool IsMeal = mealNames.Select(a => a.Key).
                              Contains(foydalanuvchiXossalari.mealS[i].FoodID);
                        
                        if (IsMealName != null)
                        {
                           
                                IsMealName.Amount = foydalanuvchiXossalari.mealS[i].Amount;
                           
                        }

                        else if (IsMealName == null && IsMeal)
                        {
                            foydalanuvchiXossalari.mealS[i].foodName = mealNames[foydalanuvchiXossalari.mealS[i].FoodID];
                            resName.mealS = resName.mealS.OrderBy(a => a.FoodID).ToList();
                           
                        }

                    }
                    resName.mealS = resName.mealS.Where(a => a.Amount != 0.0).ToList();

                    string JsonFile = JsonConvert.SerializeObject(result, Formatting.Indented);

                    StreamWriter writer = new StreamWriter(path);
                    writer.Write(JsonFile);
                    writer.Close();
                    return true;
                }

                else if (foydalanuvchiXossalari.Name == null && foydalanuvchiXossalari.mealS != null)
                {

                    for (int i = 0; i < foydalanuvchiXossalari.mealS.Count; i++)
                    {
                        var IsMealName = resName.mealS.
                        FirstOrDefault(m => m.FoodID == foydalanuvchiXossalari.mealS[i].FoodID);
                        
                        bool IsMeal = mealNames.Select(a => a.Key).
                              Contains(foydalanuvchiXossalari.mealS[i].FoodID);
                        
                        if (IsMealName != null)
                        {
                            
                                IsMealName.Amount = foydalanuvchiXossalari.mealS[i].Amount;
                           
                        }
                        else if (IsMealName == null && IsMeal)
                        {
                            foydalanuvchiXossalari.mealS[i].foodName = mealNames[foydalanuvchiXossalari.mealS[i].FoodID];
                            resName.mealS.Add(foydalanuvchiXossalari.mealS[i]);
                            resName.mealS = resName.mealS.OrderBy(a => a.FoodID).ToList();
                             
                        
                        }

                    }

                   resName.mealS = resName.mealS.Where(a=>a.Amount!= 0.0).ToList();   

                        string JsonFile = JsonConvert.SerializeObject(result, Formatting.Indented);

                    StreamWriter writer = new StreamWriter(path);
                    writer.Write(JsonFile);
                    writer.Close();
                    return true;
                }

                return false;
            }
        }

        public string Cheque(int id)
        {
            var result = getAll();
            var resCheque = result.
                    FirstOrDefault(i => i.Id == id);
            if (result != null && resCheque!=null ) 
            {
                double sum = 0;
                string cheqFood = "";
                for (int i = 0; i < resCheque.mealS.Count; i++)
                {
                    var resFoodName = resCheque.mealS[i].foodName.Split('-').ToList();
                    cheqFood+= (" "+(i + 1) + ". " + resFoodName[0] + "  -  " +$"{int.Parse(resFoodName[1])} * {resCheque.mealS[i].Amount} = " +
                        (int.Parse(resFoodName[1]) * resCheque.mealS[i].Amount)+"\n").ToString();
                    
                    sum += (int.Parse(resFoodName[1])) * resCheque.mealS[i].Amount; 
                }

                string CheqforClient =" Mijoz Id : " +resCheque.Id.ToString() + "\n" + " Mijoz Ismi: "+ 
                    resCheque.Name + "\n\n" +" Sizning buyurtmalaringiz :"+"\n\n"+ 
                    cheqFood+"\n"+$" << Umumiy hisob : {sum} so'm >>" ;
                return CheqforClient;
            }
            return " Id topilmadi yoki hali buyurtmalar mavjud emas . ";
        }
    }
}
