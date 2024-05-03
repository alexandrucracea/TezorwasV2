using Android.Graphics.Drawables;
using Android.Text.Style;
using Android.Text;
using Android.Views;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Android.Content;
using static Android.App.ActionBar;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;

namespace TezorwasV2.Platforms.Android
{
    class CustomShellBottomNavViewAppearanceTracker : ShellBottomNavViewAppearanceTracker
    {
        private readonly IShellContext shellContext;
        public CustomShellBottomNavViewAppearanceTracker(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem)
        {
            this.shellContext = shellContext;
        }



        public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            base.SetAppearance(bottomView, appearance);
            var backgroundDrawable = new GradientDrawable();
            backgroundDrawable.SetShape(ShapeType.Rectangle);
            backgroundDrawable.SetCornerRadius(80);
            //backgroundDrawable.SetColor(appearance.EffectiveTabBarBackgroundColor.ToPlatform());
            Color tabBarColor = Color.FromRgb(241, 242, 244);
            backgroundDrawable.SetColor(tabBarColor.ToPlatform());
            bottomView.SetBackground(backgroundDrawable);
            bottomView.LabelVisibilityMode = LabelVisibilityMode.LabelVisibilityUnlabeled;
            bottomView.ItemIconSize = 100;

            var layoutParams = bottomView.LayoutParameters;
            if (layoutParams is ViewGroup.MarginLayoutParams marginLayoutParams)
            {
                var margin = 30;
                marginLayoutParams.BottomMargin = margin;
                marginLayoutParams.LeftMargin = margin;
                marginLayoutParams.RightMargin = margin;
                bottomView.LayoutParameters = layoutParams;
            }
        }

        protected override void SetBackgroundColor(BottomNavigationView bottomView, Color color)
        {
            base.SetBackgroundColor(bottomView, color);
            Color tabBarBackgroundColor = Color.FromRgb(242, 244, 245);
            //bottomView.RootView?.SetBackgroundColor(shellContext.Shell.CurrentPage.BackgroundColor.ToPlatform());
            bottomView.RootView?.SetBackgroundColor(tabBarBackgroundColor.ToPlatform());
        }
    }
}
