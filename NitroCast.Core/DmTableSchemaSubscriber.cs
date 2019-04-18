using System;

namespace NitroCast.Core
{
	/// <summary>
	/// This is a wrapper for objects that are subscribing to TableSchemaAuthors.
	/// </summary>
	public class DmTableSchemaSubscriber
	{
		object _subscriber;

		public DmTableSchemaSubscriber(object subscriber)
		{
			_subscriber = subscriber;
		}
	}
}
