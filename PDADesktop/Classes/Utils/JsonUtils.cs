using Newtonsoft.Json;
using PDADesktop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDADesktop.Classes.Utils
{
    public class JsonUtils
    {
        public static List<Sincronizacion> GetListSinchronization(string serializedJson)
        {
            return JsonConvert.DeserializeObject<List<Sincronizacion>>(serializedJson);
        }

        public static List<String> GetListStringOfAdjustment(string serializedJson)
        {
            return JsonConvert.DeserializeObject<List<string>>(serializedJson);
        }

        public static List<VersionDispositivo> GetVersionDispositivo(string serializedJson)
        {
            return JsonConvert.DeserializeObject<List<VersionDispositivo>>(serializedJson);
        }

        public static List<Accion> GetListAcciones(string serializedJson)
        {
            return JsonConvert.DeserializeObject<List<Accion>>(serializedJson);
        }

        public static List<Actividad> GetListActividades(string serializedJson)
        {
            return JsonConvert.DeserializeObject<List<Actividad>>(serializedJson);
        }

        public static ObservableCollection<Ajustes> GetObservableCollectionAjustes(string serializedJson)
        {
            return JsonConvert.DeserializeObject<ObservableCollection<Ajustes>>(serializedJson);
        }

        public static List<Ajustes> GetListAjustes(string serializedJson)
        {
            return JsonConvert.DeserializeObject<List<Ajustes>>(serializedJson);
        }
    }
}
