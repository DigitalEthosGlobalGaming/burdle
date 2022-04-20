
using Degg.Core;
using Sandbox;
using System.Collections.Generic;
using System.Text.Json;

namespace Burdle
{

	[Library]
	public partial class BurdleStoreItem
	{
		public string Type { get; set; }
		public string Name { get; set; }
		public float Value { get; set; }
		public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
		public BurdleStoreItem(string type, string name,float value)
		{
			Type = type;
			Name = name;
			Value = value;
		}

		public BurdleStoreItem()
		{
			
		}

		public T Get<T>(string key, T def = default(T))
		{
			if ( Properties.ContainsKey(key))
			{
				var value = DeggJsonHelpers.GetJsonElementValue( Properties[key] );
				if ( value is T t )
				{
					return t;
				}
			}

			return def;
		}
		public void Set( string key, object val )
		{
			Properties[key] = val;
		}

		public string Serialize()
		{
			return JsonSerializer.Serialize( this );
		}
		public void LoadFromString(string data)
		{
			BurdleStoreItem obj = Deserialize( data );
			foreach(var i in obj.Properties)
			{
				Set( i.Key, i.Value );
			}
		}

		public static BurdleStoreItem Deserialize(string data)
		{
			return JsonSerializer.Deserialize<BurdleStoreItem>( data ); ;
		}
	}

}
