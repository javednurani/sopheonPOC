﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Products.Domain
{
   public class MoneyValue
   {
      public string CurrencyCode { get; set; }
      public decimal? Value { get; set; }
   }
}
