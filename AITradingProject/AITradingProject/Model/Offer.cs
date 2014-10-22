using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITradingProject.Model
{
    public class Offer
    {
        private City from;        
        private Edge e;
        private Dictionary<Resource, int> resourcesOffered;
        private Dictionary<Resource, int> resourcesRequired;


        public Offer(City offeringCity, Edge edge, Dictionary<Resource, int> offer, Dictionary<Resource, int> required)
        {
            this.from = offeringCity;
            e = edge;
            resourcesOffered = offer;
            resourcesRequired = required;
        }

    }
}
