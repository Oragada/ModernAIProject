using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITradingProjectModel.Model
{
    public class Offer
    {
        private City from;        
        private Edge e;
        private Dictionary<Resource, int> offered; 
        //private Dictionary<Resource, int> resourcesOffered;
        //private Dictionary<Resource, int> resourcesRequired;


        /*public Offer(City offeringCity, Edge edge, Dictionary<Resource, int> offer, Dictionary<Resource, int> required)
        {
            this.from = offeringCity;
            e = edge;
            resourcesOffered = offer;
            resourcesRequired = required;
        }*/

        public override string ToString()
        {
            return string.Format("City {0} offering City {1}. Resources: {2}", from.ID, e.Other(from).ID,
                OfferedString());
        }

        public string ShortString()
        {
            return string.Format("C{0} -> C{1}: {2}", from.ID, e.Other(from).ID,
                ShortOfferedString());
        }

        private object ShortOfferedString()
        {
            StringBuilder strB = new StringBuilder();
            foreach (KeyValuePair<Resource, int> kv in offered)
            {
                if (kv.Value == 0) continue;
                strB.Append(kv.Value > 0 ? "(+" : "(-");

                strB.Append(string.Format("{0}{1}), ", Math.Abs(kv.Value), ShortResource(kv.Key)));
            }
            return strB.ToString();
        }

        private object ShortResource(Resource key)
        {
            switch (key)
            {
                case Resource.Dolls:
                    return "D";
                case Resource.Food:
                    return "F";
                case Resource.Water:
                    return "W";
                default:
                    return "?";
            }
        }

        private object OfferedString()
        {
            StringBuilder strB = new StringBuilder();
            foreach (KeyValuePair<Resource, int> kv in offered)
            {
                if(kv.Value == 0) continue;
                strB.Append(kv.Value > 0 ? "(Gives " : "(Wants ");

                strB.Append(string.Format("{0} {1}), ", Math.Abs(kv.Value), kv.Key));
            }
            return strB.ToString();
        }

        public Offer(City offeringCity, Edge edge, Dictionary<Resource, int> o)
        {
            from = offeringCity;
            e = edge;
            offered = o;
        }

        public City From
        {
            get { return @from; }
        }

        public Edge E
        {
            get { return e; }
        }

        public Dictionary<Resource, int> ResourcesOffered
        {
            get { return offered; }
        }

        /*public Dictionary<Resource, int> ResourcesOffered
        {
            get { return resourcesOffered; }
        }

        public Dictionary<Resource, int> ResourcesRequired
        {
            get { return resourcesRequired; }
        }*/
    }
}
