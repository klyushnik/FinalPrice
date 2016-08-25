using System;

using Android.App;
using Android.OS;
using Android.Widget;

namespace FinalPrice
{
    [Activity(Label = "Help")]
    public class Help : Activity
    {
        TextView BackTextView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.help);
            BackTextView = FindViewById<TextView>(Resource.Id.backTextView);
            BackTextView.Click += (object sender, EventArgs e) =>
            {
                Finish();
            };
        }
    }
}