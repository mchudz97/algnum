using System;
using System.Collections.Generic;
using System.Text;

namespace ALS_RECOMMENDATION_ALGORITHM
{
    class Rate
    {
        private double value;
        private String product;
        private String user;

        public Rate(double value, String product, String user)
        {
            this.value = value;
            this.product = product;
            this.user = user;


        }
        public override string ToString()
        {
            return "Rate: " + this.value + " Product: " + this.product+ " User: "+this.user;
        }
    }
}
