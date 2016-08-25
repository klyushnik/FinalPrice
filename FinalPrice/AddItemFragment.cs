
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace FinalPrice
{
	public class OnAddItem : EventArgs {
		private TaxItem taxItem;
		public TaxItem AddItem {
			get { return taxItem; }
			set { taxItem = value; }
		}
		public OnAddItem (TaxItem x) : base() {
			AddItem = x;
		}
	}
	public class AddItemFragment : DialogFragment
	{
		public event EventHandler<OnAddItem> mOnAddItem;
		private TextView AddItemTextView;

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature (WindowFeatures.NoTitle);
			base.OnActivityCreated (savedInstanceState);
			//Dialog.Window.Attributes.WindowAnimations = Resource.Style.DialogAnim;
			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.addItem, container, false);

			ISharedPreferences preferences = Application.Context.GetSharedPreferences("TaxInfo", FileCreationMode.Private);

			EditText ItemPriceEditText = view.FindViewById<EditText>(Resource.Id.itemPriceEditText);
			EditText DiscountEditText = view.FindViewById<EditText> (Resource.Id.discountEditText);
			RadioButton PercentRadioButton = view.FindViewById<RadioButton> (Resource.Id.percentRadioButton);
			RadioButton Tax0RadioButton = view.FindViewById<RadioButton> (Resource.Id.taxOption0RadioButton);
			RadioButton Tax1RadioButton = view.FindViewById<RadioButton> (Resource.Id.taxOption1RadioButton);
			RadioButton Tax2RadioButton = view.FindViewById<RadioButton> (Resource.Id.taxOption2RadioButton);
			RadioButton Tax3RadioButton = view.FindViewById<RadioButton> (Resource.Id.taxOption3RadioButton);
			RadioButton Tax4RadioButton = view.FindViewById<RadioButton> (Resource.Id.taxOption4RadioButton);

			Tax2RadioButton.Text = preferences.GetString ("tax1Desc", "ERROR");
			Tax3RadioButton.Text = preferences.GetString ("tax2Desc", "ERROR");
			Tax4RadioButton.Text = preferences.GetString ("tax3Desc", "ERROR");

			AddItemTextView = view.FindViewById<TextView> (Resource.Id.addItemTextView);
			AddItemTextView.Click += (object sender, EventArgs e) => {
				double stickerPrice;
				double discount;

				TaxItem mTaxItem = new TaxItem();
				double.TryParse(ItemPriceEditText.Text, out stickerPrice);
				double.TryParse(DiscountEditText.Text, out discount);

				mTaxItem.StickerPrice = stickerPrice;
				mTaxItem.Discount = discount;
				mTaxItem.isPercent = PercentRadioButton.Checked;

				if (Tax0RadioButton.Checked) mTaxItem.TaxOption = 0;
				if (Tax1RadioButton.Checked) mTaxItem.TaxOption = 1;
				if (Tax2RadioButton.Checked) mTaxItem.TaxOption = 2;
				if (Tax3RadioButton.Checked) mTaxItem.TaxOption = 3;
				if (Tax4RadioButton.Checked) mTaxItem.TaxOption = 4;

				mOnAddItem.Invoke(this, new OnAddItem(mTaxItem));
				Dismiss();

			};
			return view;
		}
	}
}

