using System;

namespace FinalPrice
{
	public class TaxItem
	{	
		public double StickerPrice { get; set; }
		public double Discount { get; set; }
		public int TaxOption { get; set; }
		public bool isPercent { get; set; }
	}
}

