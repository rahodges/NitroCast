using System;
using System.Collections;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for ModelComparer.
	/// </summary>
	public class ModelEntryComparer : IComparer
	{
		ModelEntryCompareKey[] _keys;

		public ModelEntryComparer(params ModelEntryCompareKey[] keys)
		{
			_keys = keys;
		}

		int IComparer.Compare(object a, object b)
		{
			return Compare((IModelEntry) a, (IModelEntry) b);
		}

		public int Compare(IModelEntry a, IModelEntry b)
		{
			int result = 0;

			for(int i = 0; i <= _keys.GetUpperBound(0); i++)
			{
				result = 0;
			
				switch(_keys[i])
				{
					case ModelEntryCompareKey.Name:
						result = string.Compare(a.Name, b.Name);
						break;
				}

				if(result != 0)
					return result;
			}

            return result;			
		}
	}
}
