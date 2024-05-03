using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace TezorwasV2.Platforms.Android
{
    public partial class CustomShellHandler : ShellRenderer
    {
        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            return new CustomShellBottomNavViewAppearanceTracker(this, shellItem.CurrentItem);
        }
    }
}
