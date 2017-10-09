using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MCLYGV3.Web
{
	public static class JsonHelp
	{
	    public static string ToJson(this object obj)
	    {
            var jSetting = new JsonSerializerSettings();
            jSetting.NullValueHandling = NullValueHandling.Ignore;
            IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            var str = JsonConvert.SerializeObject(obj, dtConverter);
	        return str;
	    }

	    public static T ToObject<T>(this string obj)
	    {
            T res;
	        res = JsonConvert.DeserializeObject<T>(obj);
	        return res;
	    }

	}
}