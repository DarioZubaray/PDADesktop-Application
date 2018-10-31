using Newtonsoft.Json;
using PDADesktop.Model;
using PDADesktop.Model.Dto;
using PDADesktop.Model.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        public static ActionResultDto GetActionResult(string serializedJson)
        {
            return JsonConvert.DeserializeObject<ActionResultDto>(serializedJson);
        }

        public static string GetJsonBodyCreateNewBatch(string storeId, string[] actionsId)
        {
            JsonBodyCreateNewBatch jsonBody = new JsonBodyCreateNewBatch();
            jsonBody.idSucursal = storeId;
            if (actionsId != null)
            {
                jsonBody.idAcciones = actionsId;
            }
            string json = JsonConvert.SerializeObject(jsonBody);
            return json;
        }

        public static string GetJsonBodyModifyAdjustments(ObservableCollection<Ajustes> adjustments, long syncId, string batchId)
        {
            JsonBodyModifyAdjustments jsonBody = new JsonBodyModifyAdjustments();
            jsonBody.idSincronizacion = syncId;
            jsonBody.modificarAjustes = AjusteDto.ParseObservableCollectionToAjusteDto(adjustments, batchId);
            string json = JsonConvert.SerializeObject(jsonBody);
            return json;
        }

        public static ControlBloqueoPDA GetControlBloqueoPDA(string serializedJson)
        {
            return JsonConvert.DeserializeObject<ControlBloqueoPDA>(serializedJson);
        }

        internal static ListView GetAjustesDTO(string serializedJson)
        {
            return JsonConvert.DeserializeObject<ListView>(serializedJson);
        }

        internal static ListView GetRecepciones(string serializedJson)
        {
            return JsonConvert.DeserializeObject<ListView>(serializedJson);
        }

        internal static Dictionary<string, string> GetDiscardReception(string responseDiscardReceptions)
        {
            return JsonConvert.DeserializeObject<Dictionary<String, String>>(responseDiscardReceptions);
        }

        internal static ListView GetListView(string responseReceptionGrid)
        {
            return JsonConvert.DeserializeObject<ListView>(responseReceptionGrid);
        }
    }
}
