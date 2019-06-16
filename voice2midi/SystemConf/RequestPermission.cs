using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Toast;
using Xamarin.Forms;

namespace voice2midi.SystemConf
{
    public class RequestPermission
    {
        public static async Task Check_Permission_Async(Permission permission, Button permissionBtn, params View[] protectedElements) //params one or more
        {
            PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            bool permissionGranted = status == PermissionStatus.Granted;

            foreach (View protectedElement in protectedElements)
            {
                protectedElement.IsEnabled = permissionGranted;
            }
            permissionBtn.IsEnabled = !permissionGranted;
        }

        public static async Task Ask_Permissions_Async(Permission permission, Button permissionBtn, params View[] protectedElements)
        {
            try
            {
                PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                if (status != PermissionStatus.Granted)
                {
                    Dictionary<Permission, PermissionStatus> results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                    status = results[permission];
                }

                if (status == PermissionStatus.Granted)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CrossToastPopUp.Current.ShowToastSuccess("Permission granted");
                    });


                    foreach (View protectedElement in protectedElements)
                    {
                        protectedElement.IsEnabled = true;
                    }
                    permissionBtn.IsEnabled = false;
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CrossToastPopUp.Current.ShowToastError("Permission denied");
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception attempting to ask permission:\n{ex}\n");
            }

        }

    }
}
