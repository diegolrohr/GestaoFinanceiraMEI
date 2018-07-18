using Fly01.Core.Config;
using Fly01.Core.ViewModels;
using Fly01.uiJS.Classes.Elements;

namespace Fly01.Core.Presentation.Commons
{
    public static class ElementUIHelper
    {
        public static AutoCompleteUI GetAutoComplete(AutoCompleteUI target, string resourceHash)
        {
            if (!SessionManager.Current.UserData.UserCanPerformOperation(resourceHash, EPermissionValue.Write))
            {
                target.DataUrlPost = null;
                target.DataUrlPostModal = null;
                target.DataPostField = null;
            }

            return target;
        }
    }
}