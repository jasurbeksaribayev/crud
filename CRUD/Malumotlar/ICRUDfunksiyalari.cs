using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Malumotlar
{
    public interface ICRUDfunksiyalari
    {
        public FoydalanuvchiXossalari Creator(FoydalanuvchiXossalari foydalanuvchiXossalari);

        public FoydalanuvchiXossalari get(Func<FoydalanuvchiXossalari,bool> func);
        public List<FoydalanuvchiXossalari> getAll(Func<FoydalanuvchiXossalari, bool> funcc);
        public bool Delete(int id);
        public bool Update(FoydalanuvchiXossalari foydalanuvchiXossalari);
        public string Cheque(int id);
    }
}
