
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
	public class Summary : DialogFragment
	{
		private string mSubTotal;
		private string mTax;
		private string mTotal;
		TextView mSubTotalTextView;
		TextView mTaxTextView;
		TextView mTotalTextView;
		Button dismissButton;

		public void PassSummaryValues(string subtotal, string tax, string total) {
			mSubTotal = subtotal;
			mTax = tax;
			mTotal = total;
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature (WindowFeatures.NoTitle);
			base.OnActivityCreated (savedInstanceState);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.DialogAnim;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.TaDa, container, false);
			mSubTotalTextView = view.FindViewById<TextView> (Resource.Id.SubtotalLabel);
			mTaxTextView = view.FindViewById<TextView> (Resource.Id.TaxLabel);
			mTotalTextView = view.FindViewById<TextView> (Resource.Id.TotalLabel);
			dismissButton = view.FindViewById<Button> (Resource.Id.btnDismissSummary);

			mSubTotalTextView.Text = mSubTotal;
			mTaxTextView.Text = mTax;
			mTotalTextView.Text = mTotal;
			dismissButton.Click += (object sender, EventArgs e) => {this.Dismiss();};
			return view;
		}
	}
}

