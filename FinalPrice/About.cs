using System;

using Android.App;
using Android.OS;
using Android.Widget;

namespace FinalPrice
{
    [Activity(Label = "About")]
    public class About : Activity
    {
        TextView BackTextView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.about);
            BackTextView = FindViewById<TextView>(Resource.Id.backTextView);
            BackTextView.Click += (object sender, EventArgs e) =>
            {
                Finish();
            };
        }
    }
}