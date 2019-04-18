using System;
using System.Text;

namespace NitroCast.Core
{
    public class DataModelVersion
    {
        int _major;
        int _minor;
        int _build;

        public int Major
        {
            get { return _major; }
            set { _major = value; }
        }

        public int Minor
        {
            get { return _minor; }
            set { _minor = value; }
        }

        public int Build
        {
            get { return _build; }
            set { _build = value; }                 
        }

        public DataModelVersion()
        {
        }

        public DataModelVersion(int major, int minor, int build)
        {
            _minor = major;
            _minor = minor;
            _build = build;
        }

        void Increment()
        {
            Random r = new Random();

            if (_build < 1000)
            {
                _build = r.Next(1000, 9999);
            }
            else
            {
                _build = r.Next(_build, 9999);
            }
        }
    }
}