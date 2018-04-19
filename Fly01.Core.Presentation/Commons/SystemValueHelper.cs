using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation;
using Fly01.Core.Helpers;

namespace Fly01.Core.Presentation.Commons
{
    public static class SystemValueHelper
    {
        #region Private Methods
        private static List<KeyValueVM> GetSystemKeyValueAPI(string systemEntity)
        {
            string resource = systemEntity;

            return RestHelper.ExecuteGetRequest<List<KeyValueVM>>(resource);
        }

        private static List<SystemValueVM> GetSystemEntityValues(string systemEntity)
        {
            string resource = string.Format("{0}/{1}", AppDefaults.GetResourceName(typeof(SystemValueVM)), systemEntity);

            return RestHelper.ExecuteGetRequest<List<SystemValueVM>>(resource);
        }

        private static List<KeyValueVM> GetSystemKeyValue(string type, bool fieldStruct = false)
        {
            string systemEntity = "";
            string entityProperty = "";

            if (type.Split('_').Length > 1)
            {
                string[] strValue = type.Split('_');
                entityProperty = strValue[1];
                systemEntity = strValue[0];

                if (fieldStruct)
                    systemEntity = string.Format("{0}.{1}", systemEntity, entityProperty);
            }
            else
                entityProperty = systemEntity = type;

            List<SystemValueVM> items = null;
            if (fieldStruct)
                items = new List<SystemValueVM> { new SystemValueVM { Name = entityProperty, Values = GetSystemKeyValueAPI(systemEntity) } };
            else
                items = GetSystemEntityValues(systemEntity);

            return items.FirstOrDefault(x => x.Name.Equals(entityProperty, StringComparison.InvariantCultureIgnoreCase)).Values;
        }
        #endregion

        #region Public Methods
        public static List<SelectListItem> Get(string type, bool fieldStruct = false, bool defaultValue = true)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            List<KeyValueVM> items = GetSystemKeyValue(type, fieldStruct);
            if (items != null && items.Count > 0)
                result.AddRange(items.Select(x => new SelectListItem() { Text = x.Value, Value = x.Key, Selected = false }));

            if (defaultValue)
                result.Insert(0, new SelectListItem { Text = "Selecione...", Value = "", Selected = false });

            return result.ToList();
        }

        public static IEnumerable<SelectOptionUI> GetUIElementBase(Type enumType, bool defaultValue = false, string selectedValue = "")
        {
            List<SelectOptionUI> result = new List<SelectOptionUI>();
            var items = EnumHelper.GetDataEnumValues(enumType);

            if (items != null && items.Count > 0)
                result.AddRange(items.Select(x => new SelectOptionUI() {
                    Label = x.Value,
                    Value = x.Key,
                    Selected = (x.Value == selectedValue)
                }));

            if (defaultValue)
                result.Insert(0, new SelectOptionUI() { Label = "Selecione...", Value = "" });

            return result.ToList();
        }
        #endregion
    }
}