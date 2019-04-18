using System;

namespace NitroCast.Core
{
	/// <summary>
	/// Database lock type scenarios associated with class entries in the model.
	/// </summary>
	public enum ConcurrencyType
	{
		None,
        OptimisticFull,         // Checks to make sure record's changed fields match old record fields to update
        OptimisticPartial,      // Checks to make sure record's fields match old record to update
		Pessimistic,
		PessimisticTimeStamp
	}
}