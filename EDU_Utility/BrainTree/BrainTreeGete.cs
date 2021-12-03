using Braintree;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDU_Utility.BrainTree
{
    public class BrainTreeGete : IBrainTreeGate
    {
        public BrainTreeSetting _options { get; set; }

        private IBraintreeGateway braintreeGateway { get; set; }
        public BrainTreeGete(IOptions<BrainTreeSetting> options)
        {
            _options = options.Value;
        }
        public IBraintreeGateway CreateGeteWay()
        {
            return new BraintreeGateway(_options.Environment,_options.MerchantId,_options.PublicKey,_options.PrivateKey);
        }

        public IBraintreeGateway GetGeteWay()
        {
            if (braintreeGateway== null)
            {
                braintreeGateway = CreateGeteWay();
            }
            return braintreeGateway;
        }
    }
}
