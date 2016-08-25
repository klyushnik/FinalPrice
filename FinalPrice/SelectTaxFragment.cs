
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
	public class OnSelectTax : EventArgs {
		private int taxOption;
		public int TaxOption {
			get { return taxOption; }
			set { taxOption = value; }
		}
		public OnSelectTax (int x) : base() {
			TaxOption = x;
		}
	}
	public class SelectTaxFragment : DialogFragment
	{
		static public event EventHandler<OnSelectTax> mOnSelectTax;
		private TextView StandardTax;
		private TextView TaxExempt;
		private TextView TaxOption1;
		private TextView TaxOption2;
		private TextView TaxOption3;
		private TextView CancelTxt;

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature (WindowFeatures.NoTitle);
			base.OnActivityCreated (savedInstanceState);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			ISharedPreferences preferences = Application.Context.GetSharedPreferences("TaxInfo", FileCreationMode.Private);

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.SelectTax, container, false);
			StandardTax = view.FindViewById<TextView> (Resource.Id.OptionStandardTax);
			TaxExempt = view.FindViewById<TextView> (Resource.Id.OptionTaxExempt);
			TaxOption1 = view.FindViewById<TextView> (Resource.Id.Option1);
			TaxOption2 = view.FindViewById<TextView> (Resource.Id.Option2);
			TaxOption3 = view.FindViewById<TextView> (Resource.Id.Option3);
			CancelTxt = view.FindViewById<TextView> (Resource.Id.OptionCancel);

			TaxOption1.Text = preferences.GetString("tax1Desc", "ERROR");
			TaxOption2.Text = preferences.GetString("tax2Desc", "ERROR");
			TaxOption3.Text = preferences.GetString("tax3Desc", "ERROR");

			StandardTax.Click += (object sender, EventArgs e) => {Buttons_Click(0);};
			TaxExempt.Click += (object sender, EventArgs e) => {Buttons_Click(1);};
			TaxOption1.Click += (object sender, EventArgs e) => {Buttons_Click(2);};
			TaxOption2.Click += (object sender, EventArgs e) => {Buttons_Click(3);};
			TaxOption3.Click += (object sender, EventArgs e) => {Buttons_Click(4);};
			CancelTxt.Click += (object sender, EventArgs e) => {Dismiss();};

			return view;
		}

		void Buttons_Click (int opt)
		{
			mOnSelectTax.Invoke (this, new OnSelectTax(opt));
			Dismiss ();
		}
	}
}

