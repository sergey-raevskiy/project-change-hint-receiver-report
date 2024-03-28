using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace VisualStudioReport
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(VisualStudioReportPackage.PackageGuidString)]
    public sealed class VisualStudioReportPackage : AsyncPackage
    {
        public const string PackageGuidString = "eee1eeeb-e1f0-4643-830d-b7f9f914aaab";
    }
}
