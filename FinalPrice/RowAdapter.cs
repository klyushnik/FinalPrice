using Android.Content;
using Android.Widget;
using Android.Views;
using System.Collections.Generic;
using Android.Graphics;
using Android.App;

namespace FinalPrice
{
    public class RowAdapter : BaseAdapter<TaxItem>
	{
		private ISharedPreferences preferences = Application.Context.GetSharedPreferences("TaxInfo", FileCreationMode.Private);
		public List<TaxItem> mItems;
		private Context mContext;

		//constructor
		public RowAdapter(Context context, List<TaxItem> items)
		{
			mItems = items;
			mContext = context;
		}

		//mItems count
		public override int Count {
			get {return mItems.Count;}
		}

		//returns mItems position in list
		public override long GetItemId (int position)
		{ 
			return position;
		}

		//returns an actual item from the list
		public override TaxItem this[int index] {
			get { return mItems [index]; }
		}



		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View row = convertView;

			//if row is empty, inflate it using an instance of listview_row
			if (row == null) {
				row = LayoutInflater.From (mContext).Inflate (Resource.Layout.listview_row, null, false);
			}

			TextView stickerPriceTxt = row.FindViewById<TextView> (Resource.Id.stickerPriceText);
			stickerPriceTxt.Text = mItems [position].StickerPrice.ToString("C");

			TextView discountTxt = row.FindViewById<TextView> (Resource.Id.discountText);
			discountTxt.Text = "DISC: " + mItems [position].Discount.ToString("N2");
			if (mItems [position].isPercent) {
				discountTxt.Text += "%";
			} else {
				discountTxt.Text += "$";
			}

			TextView taxOptionsTxt = row.FindViewById<TextView> (Resource.Id.taxOptionsText);
			//taxOptionsTxt.Text = mItems [position].TaxOption;

			switch (mItems [position].TaxOption) {
			case 0:
				taxOptionsTxt.Text = "";
				break;
			case 1:
				taxOptionsTxt.Text = "Tax Exempt";
				break;
			case 2:
				taxOptionsTxt.Text = preferences.GetString ("tax1Desc", "ERROR");
				break;
			case 3:
				taxOptionsTxt.Text = preferences.GetString ("tax2Desc", "ERROR");
				break;
			case 4:
				taxOptionsTxt.Text = preferences.GetString ("tax3Desc", "ERROR");
				break;
			default:
				taxOptionsTxt.Text = "UNDEFINED";
				break;
			}
            
			if (position % 2 == 0) {
				row.SetBackgroundColor (Color.Rgb (233, 245, 255));
			} else {
				row.SetBackgroundColor (Color.Rgb (255, 255, 255));
			} //new layout broke alternating colors, so have to set to white as well

			return row;
		}
	}
}

