using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace StarNet.StarQL.Tests
{
	public class JsonSerializingList<T> : List<T>, IXunitSerializable
	{
		public JsonSerializingList()
		{
		}

		public void Deserialize(IXunitSerializationInfo info)
		{
			this.Clear();
			int count = info.GetValue<int>("Count");
			for (int i = 0; i < count; i++)
			{
				string typeName = info.GetValue<string>("ItemType" + i);
				Type type = typeof(T).Assembly.GetType(typeName);
				this.Add((T)JsonConvert.DeserializeObject(info.GetValue<string>("Item" + i), type));
			}
		}

		public void Serialize(IXunitSerializationInfo info)
		{
			info.AddValue("Count", this.Count, typeof(int));
			for (int i = 0; i < this.Count; i++)
			{
				info.AddValue("ItemType" + i, this[i].GetType().FullName, typeof(string));
				info.AddValue("Item" + i, JsonConvert.SerializeObject(this[i]), typeof(string));
			}
		}
	}
}