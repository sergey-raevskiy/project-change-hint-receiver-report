using System;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.ProjectSystem;
using System.Collections.Immutable;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using System.Linq;

namespace VisualStudioReport
{
    [Export(typeof(IProjectChangeHintReceiver))]
    [ProjectChangeHintKind(ProjectChangeFileSystemEntityHint.AddedFileAsString)]
    [AppliesTo("CPS")]
    public class TestProjectChangeHintReceiver : IProjectChangeHintReceiver
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IProjectThreadingService threadingService;

        private IVsOutputWindowPane vsOutputWindowPane;

        [ImportingConstructor]
        public TestProjectChangeHintReceiver([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider,
                                             [Import] IProjectThreadingService threadingService)
        {
            this.serviceProvider = serviceProvider;
            this.threadingService = threadingService;
        }

        private IVsOutputWindowPane OpenOutputPane()
        {
            threadingService.VerifyOnUIThread();

            if (vsOutputWindowPane == null)
            {
                Guid myPaneGuid = new Guid("CE63FABC-5F31-46B3-9136-A4C1F742B80C");

                IVsOutputWindow outputWindow = (IVsOutputWindow)serviceProvider.GetService(typeof(SVsOutputWindow));

                outputWindow.CreatePane(ref myPaneGuid, "VisualStudioReportPackage", 1, 1);
                outputWindow.GetPane(ref myPaneGuid, out vsOutputWindowPane);
                vsOutputWindowPane.Activate();
            }

            // Bring output window to front
            Guid outputWindowGuid = VSConstants.StandardToolWindows.Output;

            IVsUIShell uiShell = (IVsUIShell)serviceProvider.GetService(typeof(IVsUIShell));
            IVsWindowFrame outputWindowFrame;

            uiShell.FindToolWindow(0, ref outputWindowGuid, out outputWindowFrame);
            outputWindowFrame.Show();

            return vsOutputWindowPane;
        }

        public async Task HintedAsync(IImmutableDictionary<Guid, IImmutableSet<IProjectChangeHint>> hints)
        {
            await threadingService.SwitchToUIThread();

            IVsOutputWindowPane pane = OpenOutputPane();

            foreach (IProjectChangeHint hint in hints.SelectMany(h => h.Value))
            {
                pane.OutputString("HintedAsync(" + hint + ")\r\n");
            }
        }

        public async Task HintingAsync(IProjectChangeHint hint)
        {
            await threadingService.SwitchToUIThread();

            IVsOutputWindowPane pane = OpenOutputPane();

            pane.OutputString("HintingAsync(" + hint + ")\r\n");
        }
    }
}
