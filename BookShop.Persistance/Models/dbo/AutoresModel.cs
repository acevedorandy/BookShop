

namespace BookShop.Persistance.Models.dbo
{
    public class AutoresModel
    {
        public int AutorID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Pais { get; set; }

    }
}
