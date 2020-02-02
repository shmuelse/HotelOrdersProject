using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Utilities;

namespace BE
{
    public class HostingUnit
    {
        public int HostingUnitKey { get; set; }
        public int AccommodationKey { get; set; }
        public string HostId { get; set; }
        public string UnitNumber { get; set; }
        public decimal UnitPrice { get; set; }
        public int SumOfferOrders { get; set; }
        public int SumCompletedOrders { get; set; }
        public enums.Area Area { get; set; }
        public enums.Districts District { get; set; }
        public FilterElements UnitOptions { get; set; }
        public List<string> Uris { get; set; }

        // tell the XmlSerializer to ignore this Property.
        [XmlIgnore]
        public bool[,] Diary { get; set; }

        //tell the XmlSerializer to name the Array Element as'Diary'  instead of 'DiaryFlatten'
        [XmlArray("Diary")]
        public bool[] DiaryFlatten
        {
            get => Diary.Flatten();
            set => Diary = value.Expand(12);
        }

        public bool this[DateTime index]
        {
            get => Diary[index.Month - 1, index.Day - 1];
            set => Diary[index.Month - 1, index.Day - 1] = value;
        }
        public override string ToString()
        {
            return "Hosting unit key: " + HostingUnitKey;
        }
    }
}