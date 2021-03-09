 using System;
 using System.ComponentModel;

namespace Infraestructura.Core.Comun.Dato
{
    [TypeConverter(typeof(IdTypeConverter))]
    public struct Id 
    {
        [NonSerialized]
        private decimal _id;
       
        public Id(int value)
        {
            _id = value;
        }



        public Id(long value)
        {
            _id = value;
        }

        public Id(string value)
        {
            _id = decimal.Parse(value);
        }

        public Id(decimal value)
        {
            _id = value;
        }


        public override string ToString()
        {
            return _id.ToString();
        }

        public decimal Valor
        {
            get { return _id; }
        }

        public static bool operator !=(Id a, Id b)
        {
            return a._id != b._id;
        }

        public static bool operator ==(Id a, Id b)
        {
            return (a._id == b._id);
        }

        public bool IsDefault()
        {
            return _id == 0;
        }
    }
}
